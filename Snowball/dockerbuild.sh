# !/bin/bash

imtag=$(uuidgen |sed 's/-//g')
docker build -f Dockerfile -t snowball:${imtag} .
docker stop snowball
docker rm snowball
docker run -dit --restart=always --name snowball -p 80:80  snowball:${imtag}
