FROM node:16-slim as node-build-env
WORKDIR /app

COPY /Client/package.json /Client/package-lock.json ./

RUN npm config set fetch-retry-mintimeout 20000
RUN npm config set fetch-retry-maxtimeout 120000

RUN npm install

COPY /Client/ ./
RUN npm run build -- --prod

FROM mcr.microsoft.com/dotnet/sdk:3.1 as dotnet-build-env
WORKDIR /app
COPY /API/*.csproj ./API/
COPY /*.sln .
RUN dotnet restore

COPY /API/ ./API/
COPY --from=node-build-env /app/dist/client/ ./API/wwwroot/
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=dotnet-build-env /app/out/ ./
ENTRYPOINT ["dotnet", "API.dll"]