FROM microsoft/dotnet:latest

# Create a layer that includes cached dependencies :-)
COPY ./DockerStuff/project.json /DockerStuff/project.json
WORKDIR /DockerStuff
RUN ["dotnet", "restore"]

# copy our code
COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]

# build (speed startup)
WORKDIR /app/Chess.Web
RUN ["dotnet", "build"]

EXPOSE 5000/tcp

ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:5000

CMD ["dotnet", "run", "--server.urls", "http://0.0.0.0:5000"]
