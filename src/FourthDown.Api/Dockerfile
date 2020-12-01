FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

RUN apt-get update && apt-get upgrade -y

WORKDIR /src

COPY FourthDown.Api.csproj Api/
RUN dotnet restore -r linux-x64 Api/FourthDown.Api.csproj

COPY . Api/

WORKDIR /src/Api

RUN dotnet publish --no-restore -c Release -r linux-x64 -o /app/publish FourthDown.Api.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000-5001

ENTRYPOINT ["dotnet", "/app/FourthDown.Api.dll"]
