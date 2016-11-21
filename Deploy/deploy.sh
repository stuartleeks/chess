#!/bin/bash

echo klil ssh...
kill -kill $(pgrep ssh-agent)
kill -kill $(pgrep ssh)

echo load keys...
eval "$(ssh-agent -s)"
ssh-add /ssh/acs-stuart

exitCode=$?
if [ $exitCode -ne 0 ]
then
    exit $exitCode
fi;

echo ssh tunnel...
ssh -L 2375:localhost:2375 -N stuart@slacs-swarmmgmt.northeurope.cloudapp.azure.com -p2200 &

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
