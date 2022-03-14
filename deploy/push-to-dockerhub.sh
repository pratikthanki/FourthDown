#!/bin/bash
set -e

trap cleanup ERR TERM INT

echo "Building Api docker image"
docker build -f src/FourthDown.Api/Dockerfile . -t pratikthanki9/fourthdown-api:latest

echo "Pushing image to registry"
docker push pratikthanki9/fourthdown-api:latest

echo "done"
