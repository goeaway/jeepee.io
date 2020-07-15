FROM resin/raspberrypi3-debian:stretch

RUN curl http://www.linux-projects.org/listing/uv4l_repo/lpkey.asc | sudo apt-key add -

RUN echo 'deb http://www.linux-projects.org/listing/uv4l_repo/raspbian/stretch stretch main' | sudo tee -a /etc/apt/sources.list

RUN apt update

RUN apt install -y --no-install-recommends \
    uv4l \
    uv4l-raspicam \
    uv4l-raspicam-extras \
    uv4l-webrtc

EXPOSE 8080

ENTRYPOINT [ "/usr/bin/uv4l" ]

CMD ["--driver raspicam", "--driver-config-file=/etc/uv4l/uv4l-raspicam.conf"]