#!/bin/bash

# echo kill ssh...
# kill -kill $(pgrep ssh-agent)
kill -kill $(pgrep ssh)

# echo load keys...
# eval "$(ssh-agent -s)"
# ssh-add /ssh/acs-stuart

# exitCode=$?
# if [ $exitCode -ne 0 ]
# then
#     exit $exitCode
# fi;

echo ssh tunnel...
# TODO parameterise this!
# Turned off StrictHostKeyChecking to simplify setting up new agents to build ;-)
ssh -L 2375:localhost:2375 -N stuart@slacs-swarmmgmt.northeurope.cloudapp.azure.com -p2200 -i /ssh/acs-stuart -oStrictHostKeyChecking=no &
# ssh -L 2375:localhost:2375 -N stuart@slacs-dcosmgmt.northeurope.cloudapp.azure.com -p2200 -i /ssh/acs-stuart -oStrictHostKeyChecking=no &

exitCode=$?
if [ $exitCode -ne 0 ]
then
    exit $exitCode
fi;

sleep 1s

echo docker compose pull...
export DOCKER_HOST=:2375
docker-compose pull

exitCode=$?
if [ $exitCode -ne 0 ]
then
    exit $exitCode
fi;

echo docker compose up...
docker-compose -p chess up -d 

echo docker compose done
kill -kill $(pgrep ssh)
