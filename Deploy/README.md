# FourthDown

The `./Deploy` directory contains a docker-compose file to deploy monitoring resources locally.

To get started, create a docker newtork by running `docker network create monitoring`.

From this directory run `docker-compose up -d` to pull the images and run the containers.

You can visit `http://localhost:16686` to view the API traces.
