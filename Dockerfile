FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

LABEL "com.github.actions.name"="Microsoft Teams (Generic)"
LABEL "com.github.actions.description"="Send a message to the designated channel in Microsoft Teams"
LABEL "com.github.actions.icon"="message-square"
LABEL "com.github.actions.color"="purple"

LABEL "repository"="http://github.com/aliencube/microsoft-teams-actions"
LABEL "homepage"="http://github.com/aliencube"
LABEL "maintainer"="Justin Yoo <no-reply@aliencube.com>"

COPY *.sln .
COPY src/ ./src/

ADD entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]
