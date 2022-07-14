# !/bin/bash

imtag=$(uuidgen |sed 's/-//g')
docker build -f Dockerfile -t snowball-api:${imtag} .
docker stop snowball-api
docker rm snowball-api
docker run -dit --restart=always --name snowball-api -p 80:80 -p 443:443 -v ${HOME}/.aspnet/https:/https/ snowball-api:${imtag}
