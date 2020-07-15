FROM resin/raspberrypi3-debian:stretch

RUN apt-get update && apt-get install -y --no-install-recommends curl

RUN curl http://www.linux-projects.org/listing/uv4l_repo/lrkey.asc | apt-key add -

RUN echo "deb http://www.linux-projects.org/listing/uv4l_repo/raspbian/ stretch main" >> /etc/apt/sources.list

RUN apt-get install -y --no-install-recommends \
    uv4l \
    uv4l-raspicam \
    uv4l-raspicam-extras \
    uv4l-webrtc && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

EXPOSE 8080

ENTRYPOINT [ "/usr/bin/uv4l" ]

CMD ["--driver raspicam", "--driver-config-file=/etc/uv4l/uv4l-raspicam.conf"]