ARG ASPNET_VERSION=3.1-buster-slim-arm32v7
FROM mcr.microsoft.com/dotnet/core/aspnet:${ASPNET_VERSION}

# Create Dirs
RUN mkdir /jeepee
RUN mkdir /jeepee/app

WORKDIR /jeepee/app

# Get jeepee
RUN curl -L https://github.com/goeaway/jeepee.io/releases/latest/download/app.tar.gz --output app.tar.gz
RUN tar -xzf app.tar.gz

EXPOSE 80
# Run jeepee
ENTRYPOINT ["dotnet", "Jeepee.IO.Receiver.Presentation.API.dll"]