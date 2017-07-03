FROM microsoft/dotnet:1.1.2-runtime

# copy our code
COPY ./publish/chess /app
WORKDIR /app

EXPOSE 5000/tcp

#ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["dotnet", "Chess.Web.dll"]

ARG BUILD_DATE
ARG VCS_REF
ARG BUILD_BUILDNUMBER
LABEL org.label-schema.build-date=$BUILD_DATE \
        org.label-schema.name="Chess" \
        org.label-schema.description="Demo Chess site" \
        org.label-schema.vcs-ref=$VCS_REF \
        org.label-schema.vendor="Stuart Leeks" \
        org.label-schema.version=$BUILD_BUILDNUMBER \
        org.label-schema.schema-version="1.0" \
        org.label-schema.vcs-url="https://github.com/stuartleeks/chess" \
#        org.label-schema.url="e.g. http://chess.faux.ninja/" \
