# FourthDown

The `./monitoring` directory contains a docker-compose file to deploy the api and monitoring resources locally.

Run the shell script to create the docker network and services from root: `./monitoring/build-monitoring.sh`

On linux-based systems you'll need permissions to execute the script. To do so run the following: `chmod +x monitoring/build-monitoring.sh`

View stats at the following:

- Jaeger: <http://localhost:16686>
- Prometheus: <http://localhost:9090>
- Grafana: <http://localhost:3000>
