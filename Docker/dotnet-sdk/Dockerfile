FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN /bin/cp /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
RUN echo 'deb http://deb.debian.org/debian/ sid main'>> /etc/apt/sources.list
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash - \ 
	&& apt update \
	&& apt install -y nodejs build-essential openjdk-8-jre zip unzip \
        && rm -rf /var/lib/apt/lists/*
ENV LANG C.UTF-8
