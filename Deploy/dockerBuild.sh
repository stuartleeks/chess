#!/bin/bash

dotnet restore

rm -rf $(pwd)/publish

dotnet publish $(pwd)/Chess.Web/project.json -o $(pwd)/publish/chess

docker build --build-arg BUILD_DATE=`date -u +%Y-%m-%dT%H:%M:%SZ` --build-arg VCS_REF=`git rev-parse --short HEAD` --build-arg VCS_REF=$BUILD_BUILDNUMBER -t stuartleeks/chesstest:$BUILD_BUILDNUMBER .