FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
# Create Dirs
RUN mkdir /jeepee
RUN mkdir /jeepee/app

WORKDIR /jeepee/app

COPY . .
RUN dotnet publish -c Debug -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /jeepee/app
COPY --from=build-env /jeepee/app/out .

EXPOSE 80
# Run jeepee
ENTRYPOINT ["dotnet", "Jeepee.IO.Receiver.Presentation.API.dll"]