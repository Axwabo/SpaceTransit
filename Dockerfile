FROM unityci/editor:ubuntu-6000.2.10f1-webgl-3.2.0

ARG BLENDER_SHORT_VERSION=4.5
ARG BLENDER_FULL_VERSION=4.5.4

RUN echo "BLENDER_SHORT_VERSION: $BLENDER_SHORT_VERSION"
RUN echo "BLENDER_FULL_VERSION: $BLENDER_FULL_VERSION"
RUN apt-get update && \
    apt-get install -y wget && \
    wget https://download.blender.org/release/Blender$BLENDER_SHORT_VERSION/blender-$BLENDER_FULL_VERSION-linux-x64.tar.xz && \
    tar -xf blender-$BLENDER_FULL_VERSION-linux-x64.tar.xz && \
    rm blender-$BLENDER_FULL_VERSION-linux-x64.tar.xz

ENV PATH="$PATH:/blender-$BLENDER_FULL_VERSION-linux-x64"
