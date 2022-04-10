#!/usr/bin/env bash

set -euo pipefail

version="$(jq -r '.version' package.json)"
docUrl="https://careboo.github.io/serially/$version"

echo "[[ Setting documentation urls to $docUrl ]]"

echo "Updating package.json..."
jq --arg url "$docUrl" \
    '.documentationUrl = $url | .changelogUrl = $url + "/changelog" | .licensesUrl = $url + "/license"' \
    package.json > package.json.tmp \
    && rm package.json \
    && mv package.json.tmp package.json

echo "Updating .docfx/docfx.json..."
jq --arg url "$docUrl" \
    '.build.sitemap.baseUrl = $url' \
    .docfx/docfx.json > docfx.json.tmp \
    && rm .docfx/docfx.json \
    && mv docfx.json.tmp .docfx/docfx.json
