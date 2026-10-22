#!/usr/bin/env bash
set -euo pipefail

echo "Stage1 Find Updates"
KENDO_VERSION="${2:-}"
THEMES_VERSION="${3:-}"
if [[ -z "$KENDO_VERSION" && -z "$THEMES_VERSION" ]]; then
    KENDO_VERSION=$(curl -fsSL https://api.github.com/repos/telerik/kendo-ui-core/releases | jq -er '[.[] | select(.prerelease == false)][0].tag_name | sub("^v"; "")')
    THEMES_VERSION=$(curl -fsSL https://api.github.com/repos/telerik/kendo-themes/releases | jq -er '[.[] | select(.prerelease == false)][0].tag_name | sub("^v"; "")')
elif [[ -z "$KENDO_VERSION" || -z "$THEMES_VERSION" ]]; then
    echo "Provide both Kendo and themes versions, or leave both blank to use the latest stable releases." >&2
    exit 1
fi
LATEST_RELEASE="$KENDO_VERSION"
LATEST_THEMES_RELEASE="$THEMES_VERSION"
echo "Selected Kendo version is $LATEST_RELEASE"
echo "Selected themes version is $LATEST_THEMES_RELEASE"

function getCurrentVersion {
    CURRENT_GLOBAL_VERSION=$(grep -RhoP --include='*.cshtml' 'kendo-ui@\K[0-9]{4}\.[0-9]+\.[0-9]+' . | sed -n '1p' || true)
}

function getCurrentThemesVersion {
    CURRENT_GLOBAL_THEMES_VERSION=$(grep -RhoP --include='*.cshtml' 'kendo-theme-[^@/]+@\K[0-9]+\.[0-9]+\.[0-9]+' . | sed -n '1p' || true)
}

getCurrentVersion
getCurrentThemesVersion
if [[ -z "$CURRENT_GLOBAL_VERSION" || -z "$CURRENT_GLOBAL_THEMES_VERSION" ]]; then
    echo "Could not determine pinned Kendo and theme versions from Razor layouts" >&2
    exit 1
fi

    echo "Current version is $CURRENT_GLOBAL_VERSION"
    echo "Current themes version is $CURRENT_GLOBAL_THEMES_VERSION"

node .github/workflows/update-kendo-assets.js "$LATEST_RELEASE" "$LATEST_THEMES_RELEASE"


for file in `find . -type f -name "*.cshtml"`  
do
    sed -i "s/$CURRENT_GLOBAL_VERSION/$LATEST_RELEASE/g" $file
    sed -i "s/$CURRENT_GLOBAL_THEMES_VERSION/$LATEST_THEMES_RELEASE/g" $file
done
for file in `find . -type f -name "*.csproj"`  
do
    sed -i "s/$CURRENT_GLOBAL_VERSION/$LATEST_RELEASE/g" $file
    sed -i "s/$CURRENT_GLOBAL_THEMES_VERSION/$LATEST_THEMES_RELEASE/g" $file
done

echo "Stage2 Commit the change"
DATE=${DATE:-$(date +%F)}
reviewers="Dimitar-Goshev,MilenaCh"
echo $reviewers
BRANCH_NAME="update-dependencies"
PRs=$(gh pr list | grep "$BRANCH_NAME" || true)
echo "PRs are:"
echo $PRs
echo "Branch is:"
echo $BRANCH_NAME
if [ ! -z $PRs ]; then
    echo "Unmerged pr $BRANCH_NAME"
else
    git fetch origin
    git pull
    git checkout -b $BRANCH_NAME
    git config user.email "kendo-bot@progress.com"
    git config user.name "kendo-bot"
    git add . && git commit -m "chore: update dependencies"
    git pull
    git push -u origin $BRANCH_NAME
    gh pr create --base master --head $BRANCH_NAME --reviewer $reviewers \
    --title "Update dependencies $DATE" --body 'Please review and update dependencies'

    git diff
fi
