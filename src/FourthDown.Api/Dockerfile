FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src

COPY FourthDown.Api.csproj .
RUN dotnet restore FourthDown.Api.csproj

COPY . .

WORKDIR /src

RUN dotnet publish --no-restore -c Release -o /app/publish FourthDown.Api.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

WORKDIR /app

COPY --from=build /app/publish .
COPY /entrypoint.sh .

EXPOSE 80/tcp

RUN chmod +x ./entrypoint.sh

CMD /bin/bash ./entrypoint.sh
