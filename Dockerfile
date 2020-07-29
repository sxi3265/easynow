FROM sxi3265/dotnet-core-sdk:3.1-node12.x AS publish
WORKDIR /src
COPY . .
RUN cd /src/EasyNow.Web&&dotnet restore "EasyNow.Web.csproj"
RUN cd /src/EasyNow.Web&&dotnet publish "EasyNow.Web.csproj" -c Release -o /app

FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf