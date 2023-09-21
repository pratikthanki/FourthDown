FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

COPY src/FourthDown.Shared/FourthDown.Shared.csproj .
RUN dotnet restore FourthDown.Shared.csproj
COPY src/FourthDown.Shared/ /FourthDown.Shared/

COPY src/FourthDown.Api/FourthDown.Api.csproj .
RUN dotnet restore FourthDown.Api.csproj
COPY src/FourthDown.Api/ .

RUN dotnet publish --no-restore -c Release -o /app/publish FourthDown.Api.csproj

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .
COPY src/FourthDown.Api/entrypoint.sh .

EXPOSE 5000

RUN chmod +x ./entrypoint.sh

CMD /bin/bash ./entrypoint.sh
