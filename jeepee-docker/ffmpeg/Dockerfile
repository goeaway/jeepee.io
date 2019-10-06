FROM balenalib/rpi-raspbian

RUN apt-get update -qq && apt-get -y install \
  autoconf \
  automake \
  build-essential \
  cmake \
  git-core \
  libass-dev \
  libfreetype6-dev \
  libsdl2-dev \
  libtool \
  libva-dev \
  libvdpau-dev \
  libvorbis-dev \
  libxcb1-dev \
  libxcb-shm0-dev \
  libxcb-xfixes0-dev \
  pkg-config \
  texinfo \
  wget \
  zlib1g-dev

RUN mkdir /ffmpeg
ENV HOME /ffmpeg
RUN mkdir $HOME/ffmpeg_sources $HOME/bin

# x264
WORKDIR $HOME/ffmpeg_sources 
RUN git -C x264 pull 2> /dev/null || git clone --depth 1 https://code.videolan.org/videolan/x264.git 

WORKDIR $HOME/ffmpeg_sources/x264
RUN PATH="$HOME/bin:$PATH" PKG_CONFIG_PATH="$HOME/ffmpeg_build/lib/pkgconfig" ./configure --prefix="$HOME/ffmpeg_build" --bindir="$HOME/bin" --enable-static --enable-pic && \
PATH="$HOME/bin:$PATH" make && \
make install -j4

# ffmpeg
WORKDIR $HOME/ffmpeg_sources
RUN wget -O ffmpeg-snapshot.tar.bz2 https://ffmpeg.org/releases/ffmpeg-snapshot.tar.bz2 && \
tar xjvf ffmpeg-snapshot.tar.bz2

WORKDIR $HOME/ffmpeg_sources/ffmpeg
RUN PATH="$HOME/bin:$PATH" PKG_CONFIG_PATH="$HOME/ffmpeg_build/lib/pkgconfig" ./configure \
  --prefix="$HOME/ffmpeg_build" \
  --pkg-config-flags="--static" \
  --extra-cflags="-I$HOME/ffmpeg_build/include" \
  --extra-ldflags="-L$HOME/ffmpeg_build/lib" \
  --extra-libs="-lpthread -lm" \
  --bindir="$HOME/bin" \
  --enable-gpl \
  --enable-libass \
  --enable-libfreetype \
  --enable-libvorbis \
  --enable-libx264 \
  --enable-nonfree && \
PATH="$HOME/bin:$PATH" make && \
make install -j4 && \
hash -r

# add what's explained here to get uv4l
# https://stackoverflow.com/a/45268920

# assert it doesn't error
CMD ["$HOME/bin/ffmpeg", "--help"]