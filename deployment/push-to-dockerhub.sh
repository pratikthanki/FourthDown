#!/bin/bash
set -e

trap cleanup ERR TERM INT

echo "Building Api docker image"
docker build . -t pratikthanki9/fourthdown-api:latest

echo "Pushing image to registry"
docker push pratikthanki9/fourthdown-api:latest

echo "done"
