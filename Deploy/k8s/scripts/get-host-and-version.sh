#!/bin/bash

# update the URL

url=http://40.69.205.193/

while [ 1 -eq 1 ]; do
    curl $url -s -o /tmp/chess.html

    host=$(grep -Po "Host: \K[a-z0-9-]*"  /tmp/chess.html)
    build=$(grep -Po "Build: \K[a-z0-9-]*" /tmp/chess.html)

    echo "Build: $build    Host: $host"
done

## TODO
#
#  - make the url a parameter
#  - save the curl output to a file and 
