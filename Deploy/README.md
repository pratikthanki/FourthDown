# FourthDown

The `./Deploy` directory contains a docker-compose file to deploy monitoring resources locally.

To get started create a docker newtork by running `docker network create monitoring`.

From the directory run `docker-compose up -d` to pull the images and run the containers.

You can visit the following to see API tracing:

- Jaeger: http://localhost:16686
- Prometheus: http://localhost:9090
- Grafana: http://localhost:3000
