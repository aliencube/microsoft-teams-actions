#!/bin/sh -l

cd /app

dotnet restore
dotnet build
dotnet run --project src/GitHubActions.Teams.ConsoleApp -- \
    --webhook-uri "$WEBHOOK_URI" \
    --title "$TITLE" \
    --summary "$SUMMARY"  \
    --text "$TEXT" \
    --theme-color "$THEME_COLOR" \
    --sections "$SECTIONS" \
    --actions "$ACTIONS"
