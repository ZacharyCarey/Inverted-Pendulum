#!/bin/sh
echo -ne '\033c\033]0;InvertedPendulum\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/InvertedPendulum.x86_64" "$@"
