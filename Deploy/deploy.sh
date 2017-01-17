#!/bin/bash
#

user="stuart"
sshPath="/ssh/acs-stuart"
endpoint=""
while [[ $# -gt 0 ]]
do
    case "$1" in
        --endpoint | -e)
            endpoint="$2"
            shift 2
            ;;
        --user | -u)
            user="$2"
            shift 2
            ;;
        --ssh-path)
            sshPath="$2"
            shift 2
            ;;
        *)
            echo "unknown option $1"
            exit 1
    esac
done

if [ -z $endpoint ]; then
    echo "endpoint (-e  | --endpoint) not specified"
    exit 2
fi


echo kill ssh...
kill -kill $(pgrep ssh)

echo ssh tunnel...
# Turned off StrictHostKeyChecking to simplify setting up new agents to build ;-)
ssh -L 23750:localhost:2375 -N $user@$endpoint -p2200 -i $sshPath -oStrictHostKeyChecking=no &

exitCode=$?
if [ $exitCode -ne 0 ]
then
    exit $exitCode
fi;

sleep 1s

echo docker compose pull...
export DOCKER_HOST=:23750
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
