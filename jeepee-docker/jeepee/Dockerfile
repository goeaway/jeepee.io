FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7

# Create Dirs
RUN mkdir /jeepee
RUN mkdir /jeepee/app

WORKDIR /jeepee/app

# Get jeepee
RUN curl -L https://github.com/goeaway/jeepee.io/releases/latest/download/app.tar.gz --output app.tar.gz
RUN tar -xzf app.tar.gz

EXPOSE 80
EXPOSE 443
# Run jeepee
ENTRYPOINT ["dotnet", "/jeepee/app/Jeepee.IO.Receiver.Presentation.API.dll"]