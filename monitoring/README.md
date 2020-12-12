# FourthDown

The `./Deploy` directory contains a docker-compose file to deploy monitoring resources locally.

Run the shell script to create the docker network and services from root: `monitoring/build-monitoring.sh`

On linux-based systems you'll need permissions to execute the script. To do so run the following `chmod +x monitoring/build-monitoring.sh`

Check it out API tracing at:

- Jaeger: <http://localhost:16686>
- Prometheus: <http://localhost:9090>
- Grafana: <http://localhost:3000>
