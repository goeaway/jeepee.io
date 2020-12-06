# Jeepee.io

The first thing I wanted to do with a Raspberry Pi when I heard about them was to chuck it on an RC vehicle and be able to control it from my desk. If it was smart enough to get a beer from the fridge for me that'd be a huge bonus.

This thing has gone through a few iterations and has spent a lot of time gathering dust between each of those. I started out with a simple program that could send infrared signals to a Lego IR receiver upon receiving HTTP requests. I quickly scrapped that solution after a friend burst out laughing at the thought of it sending an infrared signal less than an inch from the transmitter to the receiver. He found it hilarious that the transmitter was onboard the vehicle with the receiver, and after thinking about it myself, I agreed with him. So did the only thing I knew how at that point, leave it alone for ages and hope the future me would come along and try and make it a bit better.

Which fortunately I did, after a couple of years...

In its current state, the software is slightly more sophisticated. I have a .NET Core Web API running in a Docker container on the Pi, which upon receiving update requests from a controller via web sockets sets GPIO pins that are connected directly to a set of Lego power functions motor. I also have a camera onboard which can stream fairly low latency/not terrible quality video.
I like to think that friend wouldn't scoff at this version.

If you'd like to learn how I did this, or want to copy it to get your own version up and running, read on...

## The hardware

So here it is, in all it's glory. I used a Lego set as the base because it meant I would have a well built model to work with, instead of the shoddy rubbish I would have been able to build myself. I've added a couple of modifications to the [base set](https://www.amazon.co.uk/LEGO-42065-Tracked-Racer-Building/dp/B01J41LWFW/ref=asc_df_B01J41LWFW/), mainly to provide a load more space for all the components I need. I also upgraded the motors to be L motors, because the ones that come with the set don't have enough POWER for all the added weight (or at least that's the excuse we're going with).

![jeepee tank](https://joetm.space/assets/articleimages/jeepee_2.jpg)

![internals](https://joetm.space/assets/articleimages/jeepee_3.jpg)

Here are all the important bits. The Pi itself receives commands from the controller, and using the web api I built converts those commands into signals sent to the Lego motors. The motors themselves need 9v of power, but the Pi is only capable of providing 5v. To deal with this, we have a 9v battery on board as well to provide the required power. 

The L293d chip in the middle is used to "upscale" the voltage of the signals from the Pi so they're powerful enough for the motors. Or something, admittedly I'm not very good with electronics so couldn't say exactly what's going on here but I got the idea for this method from [this excellent article](https://www.hackster.io/Notthemarsian/take-control-over-lego-power-functions-ee0bfa).

I then connected up the live and ground wires of each lego motor to that chip. Each of these motors actually has four wires, but for simple on/off control like we're doing here, you only need to use the middle two. I think the outer two are used for speed control.

Here's a detailed schematic of the electronic components, based of the system by Patrick in the above article. I also added an LED that would turn on when the 9v battery pack was on, so it was easy to see if I'd left it on by accident!

![PCB schematics](https://joetm.space/assets/articleimages/jeepee_9.jpg)

If you happen to be an electrical engineer you may be squirming just looking at this, but it does get the job done! The jumper wires labelled xP (purple), xG (green) and xB (blue) are the wires that connect to the GPIO pins on the Pi, and a set of these make up what I'm calling a "channel", the controller then sends signals to update a channel, which will result in a motor turning on or off, or changing direction. The purple wire is the enable pin for the channel, the blue wire is the "one" pin and the green is the "two" pin. 

The below image is of the schematic's real counter part, in all it's jumper wire glory. Originally I had this circuit working on a breadboard, but I wanted to move it over to a PCB to be more permenant and to have a smaller form factor. It took a few tries and a slightly sore back from hunching over it for hours but I'm very happy with the result. I think of the whole project I learnt the most at this point.

![Real PCB](https://joetm.space/assets/articleimages/jeepee_4.jpg)

![Pi Connections](https://joetm.space/assets/articleimages/jeepee_5.jpg)

The above image shows the connections from the Pi to the PCB, You can kind of choose where the purple, blue and green pins of each channel connect to, as long as the pins on the board support GPIO operations. Later on you'll see where to set the channels in the web api.

## The software

The below shows a diagram of the system architecture.

![software arch](https://joetm.space/assets/articleimages/jeepee_6.jpg)

I made the decision early on to use Docker because I wanted to be able to easily install this software on multiple RC vehicles. This choice turned out to really help because I ended up needing to completely wipe the Pi a few times when things went wrong! 

![Controller UI](https://joetm.space/assets/articleimages/jeepee_7.jpg)

The controller site itself is built with React and accepts a few different methods of control. Initially I'd planned to have a website running on my computer, but to be more inline with my philosophy of making it easy to reinstall, I moved the site onto the Pi as well, alongside the Web API receiver (flashbacks of IR transmitter next to the receiver) (thousand yard stare) (credence clearwater revival)...

The controller sends requests to the API via web sockets using SignalR, but the request is reverse proxied by Nginx. This is running in a container on the Pi and listens on port 80. The Nginx container also routes traffic for the controller if it's running onboard.


```
public class UpdateChannelHandler : IRequestHandler<UpdateChannel>
{
    private readonly ILogger _logger;
    private readonly IPinSetter _pinSetter;
    private readonly HardwareOptions _hardwareOptions;

    public UpdateChannelHandler(ILogger logger, IPinSetter pinSetter, HardwareOptions hardwareOptions)
    {
        _logger = logger;
        _pinSetter = pinSetter;
        _hardwareOptions = hardwareOptions;
    }

    public async Task<Unit> Handle(UpdateChannel request, CancellationToken cancellationToken)
    {
        _logger.Information("Update channel request started CH: {Channel}, D: {Direction}, O: {On}", request.Channel, request.Direction, request.On);

        var channel = _hardwareOptions.Channels[request.Channel];
        await _pinSetter.SetPinAsync(channel.Enable, request.On);
        await _pinSetter.SetPinAsync(channel.One, request.Direction);
        await _pinSetter.SetPinAsync(channel.Two, !request.Direction);

        return Unit.Value;
    }
}
```

The code snippet above shows the channel update handler in the api. I'm using Mediatr to split up the code into different sections based on feature. I really like this architectural style as it makes adding new stuff and testing really easy.

The handler will find the channel info from the injected hardware options and then use an IPinSetter interface to update the pins. The enable pin of the channel is set based on the On property of the request. On means that the user is pressing down the button for the channel. the "one" pin is set based on the direction value of the request and the "two" is set as the inverse of that. I added the IPinSetter interface so that if I needed to run the API on my windows computer I could debug the pin setting. 

The IPinSetter interface is from a nuget package I created for the abstraction, this allows you to swap in and out multiple IPinAdapters depending on where the API is running, if you want to. The most important IPinAdapter is the UnosquarePinAdapter, which makes use of a great library [unosquare](https://github.com/unosquare/raspberryio) that actually sets the GPIO pins for us. Its really really easy to use, as you can see below...

```
public void SetPin(int pinNumber, bool on)
{
    var pin = Pi.Gpio[pinNumber];
    pin.PinMode = GpioPinDriveMode.Output;
    pin.Write(on);
}
```

To manage these containers on the Pi, I use docker-compose, which allows me to define the config for the whole system in a yaml file:

```
version: "2"
networks:
  app_bridge:
    driver: bridge
services:
  # nginx service - handles incoming requests to the server and forwards to app if applicable
  nginx:
    image: nginx
    container_name: nginx-container
    restart: always
    ports: 
      - "80:80"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf:ro
      - ./index.html:/controller/index.html:ro
      - ./bundle.js:/controller/bundle.js:ro
    depends_on:
      - jeepee
    networks:
      - app_bridge
      
  # jeepee receiver service - application, sits behind nginx and controls the pins, and monitoring modules
  jeepee:
    image: siouija/jeepee-receiver
    container_name: jeepee-receiver-container
    privileged: true # required so we can interact with the pins
    restart: always
    networks:
      - app_bridge
    volumes:
      - ./hardware.json:/jeepee/app/hardware.json:ro
```
An important bit to note is the privileged key on the jeepee definition. This must be true for us to interact with the pins, because docker doesn't allow that kind of operation without it.

Finally, to be able to get a camera feed on the Pi, I used uv4l with some extras to get it working. I initially tried to use this in another container, but couldn't get it working because it can't find the camera's device in the container, even with the privileged flag set. Something to look at in future. Having said that, it's really simple to get working. [check this out to see how](https://joetm.space/article/uv4l)

## Demo

// add video of my just controlling it from website on my desk, just show that me using controller in chrome moving sticks moves the tank tracks

## Installation

This is a quick guide on how to install the software part and assumes you've got your hardware set up and ready to go.

1. You may have already set up your raspberry pi, but if you haven't yet, [check out this tutorial](https://joetm.space/article/rpi-setup) I made on how to get the basics set up.
2. Set up docker and docker-compose, you can use this [guide I made](https://joetm.space/article/docker)
3. 


// link again to lego set + lego motors

// add links to setup PI, docker, uv4l

// explain to just copy all the files from the docker compose folder in the repo

// set hardware.json data to correct pins you added

## Wrapping up

So hopefully this guide has been useful to anyone that wants to try it out themselves. I had a huge amount of fun with this project and have learnt a huge amount from soldering wires and electronics to working with Docker. I think it turned out a lot better than I ever expected but there are few things I'd like to improve on it. 

Although it's pretty okay to install, I'd really like to streamline that process and get uv4l working in its own docker container. I'd also like to route the controller website through the API itself and then add some kind of authentication to the API so only people that are logged in can access the controls and feed. 

In future I'd love to make a public facing website so people could actually control the vehicle from elsewhere in the world, but I think I'll leave that for a future version.



If you'd like to set this project up yourself and are having trouble, [create an issue here](https://github.com/goeaway/jeepee.io/issues) and I'll try to help out.
