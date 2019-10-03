#
# Dockerfile for ffmpeg
#
FROM buildpack-deps:stretch

RUN mkdir /ffmpeg
WORKDIR /ffmpeg

RUN apt-get update | apt-get install git

RUN git clone --depth 1 http://git.videolan.org/git/x264

WORKDIR /ffmpeg/x264

RUN ./configure --host=arm-unknown-linux-gnueabi --enable-static --disable-opencl --enable-shared
RUN make -j4
RUN make install

WORKDIR /ffmpeg
RUN git clone git://source.ffmpeg.org/ffmpeg --depth=1

WORKDIR /ffmpeg/ffmpeg
RUN ./configure --arch=armel --target-os=linux --enable-gpl --enable-libx264 --enable-nonfree --enable-shared
RUN make -j4
RUN make install

ENTRYPOINT ["ffmpeg"]
CMD ["--help"]