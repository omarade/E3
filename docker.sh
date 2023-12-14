docker build -t oaddev/platformservice:latest -f PlatformService/Dockerfile ./PlatformService
docker push oaddev/platformservice

docker build -t oaddev/commandsservice:latest -f CommandsService/Dockerfile ./CommandsService
docker push oaddev/commandsservice