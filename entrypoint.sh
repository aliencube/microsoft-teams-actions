#!/bin/sh -l

cd /app

echo "URI: $WEBHOOK_URI"
echo "Title: $TITLE"
echo "Summary: $SUMMARY"
echo "Text: $TEXT"
echo "Color: $THEME_COLOR"
echo "Sections: $SECTIONS"
echo "Actions: $ACTIONS"

dotnet restore
dotnet build
dotnet run --project src/GitHubActions.Teams.ConsoleApp -- \
    --webhook-uri "$WEBHOOK_URI" \
    --title "$TITLE" \
    --summary "$SUMMARY" \
    --text "$TEXT" \
    --theme-color "$THEME_COLOR" \
    --sections "$SECTIONS" \
    --actions "$ACTIONS"
