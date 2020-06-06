FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

# Create Dirs
RUN mkdir /jeepee
RUN mkdir /jeepee/app

WORKDIR /jeepee/app

# Get jeepee
RUN curl -L https://github.com/goeaway/jeepee.io/releases/download/1.4/jeepee-receiver.tar.gz --output jeepee-receiver.tar.gz
RUN tar -xzf jeepee-receiver.tar.gz

EXPOSE 80
# Run jeepee
ENTRYPOINT ["dotnet", "/jeepee/app/Jeepee.IO.Receiver.Presentation.API.dll"]