# !/bin/bash

imtag=$(uuidgen |sed 's/-//g')
docker build -f Dockerfile -t snowball-job-scheduler:${imtag} .
docker stop snowball-job-scheduler
docker rm snowball-job-scheduler
docker run -dit --restart=always --name snowball-job-scheduler -v /etc/localtime:/etc/localtime:ro snowball-job-scheduler:${imtag}
