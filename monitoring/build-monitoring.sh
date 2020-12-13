#!/bin/bash
set -e

trap cleanup ERR TERM INT

echo "Building Api docker image"
docker build src/FourthDown.Api/ -t api

echo "Creating monitoring docker network if needed"
docker network inspect monitoring >/dev/null 2>&1 || \
    docker network create --driver bridge monitoring

echo "docker-compose'ing up"
docker-compose -f monitoring/docker-compose.yml up -d --remove-orphans

echo "done"
