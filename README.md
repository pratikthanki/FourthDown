# FourthDown

[![FourthDown API Pipeline](https://github.com/pratikthanki/FourthDown/actions/workflows/dotnet-core.yml/badge.svg)](https://github.com/pratikthanki/FourthDown/actions/workflows/dotnet-core.yml) 
![Github last-commit](https://img.shields.io/github/last-commit/pratikthanki/fourthdown)

[![codecov](https://codecov.io/gh/pratikthanki/FourthDown/branch/main/graph/badge.svg?token=ZXXUQNM47R)](https://codecov.io/gh/pratikthanki/FourthDown)

[![Docker Pulls](https://img.shields.io/docker/pulls/pratikthanki9/fourthdown-api.svg)](https://hub.docker.com/repository/docker/pratikthanki9/fourthdown-api)

## Background

Most tools to analyse NFL data is limited to Python and R packages or miscellaneous data 
from GitHub repos. This means all the C#/.NET devs (and most other languages) are missing 
out or needing to create data pipelines to actually make doing any analysis work-able!

The aim of this API is to make it easier to work with NFL data, without having to do much 
more (formatting/parsing) around it. The benefits here for a NuGet Package is that this can 
be language agnostic and extensible. This Web API is written in ASP.NET Core 3.1.

There are a set of API endpoints to query team, schedule and various aspects of game data. 
You can find more details on the [Swagger](https://pratikthanki.github.io/FourthDown/) page.

## Try it out

If you want to see how the API works and if it can meet your needs you can pull the docker 
image from [dockerhub](https://hub.docker.com/repository/docker/pratikthanki9/fourthdown-api).

Loosely from your favourite command line tool you can pull the image and run the API locally.

```shell

$ docker pull pratikthanki9/fourthdown-api:latest

$ docker images

REPOSITORY                        TAG         IMAGE ID        CREATED      SIZE
pratikthanki9/fourthdown-api      latest      f50f9524513f    1 days ago   85.1 MB

$ docker run pratikthanki9/fourthdown-api:latest

```

Or you can add it to your docker-compose and spin it up with other service, an example could 
look like this:

```yml

version: '3.4'

services:
  api:
    image: pratikthanki9/fourthdown-api:latest
    container_name: fourthdown-api
    ports:
    - 5000:5000

```

And then `docker-compose up -d` from the directory of your compose file.

You can then query the api at `http://localhost:5000`.

The `./monitoring` directory contains a docker-compose file to deploy the api and monitoring 
resources locally. Run the shell script to create the docker network and services from root: 
`./monitoring/build-monitoring.sh`

On linux-based systems you'll need permissions to execute the script. To do so run the 
following: `chmod +x monitoring/build-monitoring.sh`

View stats at the following:

- Jaeger: <http://localhost:16686>
- Prometheus: <http://localhost:9090>
- Grafana: <http://localhost:3000>

## Projects 

- `FourthDown.Api`: client-facing project (deployed on Azure)
- `FourthDown.Client`: nextJS web app
- `FourthDown.Collector`: project for writing records to the database
- `FourthDown.Shared`: library of shared models and helper methods

## Thanks

Shoutout to the guys of the R package [nflfastR](https://github.com/mrcaseb/nflfastR) 
([Ben Baldwin](https://twitter.com/benbbaldwin) and [Sebastian Carl](https://twitter.com/mrcaseb)) 
on the game data and modelling of EPA, WP and CPOE amongst other things available.

[Lee Sharpe](https://twitter.com/LeeSharpeNFL) for the game schedule data going back to 1999.

## Feedback

You can reach out to me on [Twitter](https://twitter.com/pratikthanki) with any feedback or 
questions. I also write about all things CS on my [blog](http://pratikthanki.github.io/).

Should you have any thoughts, questions, bugs or suggestions on the Fourth Down API you can also 
raise an [Issue](https://github.com/pratikthanki/FourthDown/issues) with details and I will aim 
to fix or expand capabilities!

Thanks and I hope this can be of use to you.

Pratik Thanki ✌️
