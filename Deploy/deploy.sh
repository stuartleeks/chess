#!/bin/bash

echo load keys...

eval "$(ssh-agent -s)"
ssh-add ~/.ssh/acs-stuart

# start ssh tunnel
#kill -kill $(pgrep ssh)
ssh -L 2375:localhost:2375 -N stuart@slacsswarmmgmt.northeurope.cloudapp.azure.com -p2200 &

export DOCKER_HOST=:2375
docker-compose pull
docker-compose up -d
