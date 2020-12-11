#!/bin/bash
set -e

trap cleanup ERR TERM INT

echo "Creating monitoring docker network if needed"
docker network inspect monitoring >/dev/null 2>&1 || \
    docker network create --driver bridge monitoring

echo "docker-compose'ing up"
docker-compose -f monitoring/docker-compose.yml up -d

echo "done"
