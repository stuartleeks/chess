#!/bin/bash

sudo apt-get install dotnet-dev-1.0.1

export DOTNET_CLI_TELEMETRY_OPTOUT=1

dotnet restore

rm -rf $(pwd)/publish

dotnet publish $(pwd)/Chess.Web/project.json -o $(pwd)/publish/chess

docker build --build-arg BUILD_DATE=`date -u +%Y-%m-%dT%H:%M:%SZ` --build-arg VCS_REF=`git rev-parse --short HEAD` --build-arg BUILD_BUILDNUMBER=$BUILD_BUILDNUMBER -t stuartleeks/chesstest:$BUILD_BUILDNUMBER .