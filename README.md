# FourthDown

![.NET Core](https://github.com/pratikthanki/FourthDown/workflows/.NET%20Core/badge.svg)

## Background

Most tools to analyse NFL data is limited to Python and R packages or miscellaneous data from GitHub repos. This means all the C#/.NET devs (and most other languages) are missing out or needing to create data pipelines to actually make doing any analysis work-able!

The aim of this API is to make it easier to work with NFL data, without having to do much more (formatting/parsing) around it. The benefits here for a NuGet Package is that this can be language agnostic and extensible. This Web API is written in ASP.NET Core 3.1.

There are a set of API endpoints to query team, schedule and various aspects of game data. You can find more details on the [Swagger](https://pratikthanki.github.io/FourthDown/) page.

## Try it out

You don't need an API Key to get started, if you want to see how the API works and if it can meet your needs you can pull the docker image from [dockerhub](https://hub.docker.com/repository/docker/pratikthanki9/fourthdown).

Loosely from your favourite command line tool you can pull the image and run the API locally.

```shell

$ docker pull pratikthanki9/fourthdown:latest

$ docker images

REPOSITORY                      TAG      IMAGE ID        CREATED      SIZE
pratikthanki9/fourthdown       latest   f50f9524513f    1 days ago   85.1 MB

$ docker run pratikthanki9/fourthdown:latest

```

## Thanks

Shoutout to the following people and their projects for making this API feasible.

Game data and modelling of EPA, WP and CPOE amongst othe things available by the guys of the R package [nflfastR](https://github.com/mrcaseb/nflfastR) ([Ben Baldwin](https://twitter.com/benbbaldwin) and [Sebastian Carl](https://twitter.com/mrcaseb)).

Game schedule data going back to 1999 from [Lee Sharpe](https://twitter.com/LeeSharpeNFL).

## Feedback

You can reach out to me on [Twitter](https://twitter.com/pratikthanki) with any feedback or questions. I also write about all things CS on my [blog](http://pratikthanki.github.io/).

Should you have any thoughts, questions, bugs or suggestions on the Fourth Down API you can also raise an [Issue](https://github.com/pratikthanki/FourthDown/issues) with details and I will aim to fix or expand capabilities!

Thanks and I hope this can be of use to you.

Pratik Thanki ✌️
