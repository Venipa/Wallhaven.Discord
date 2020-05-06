#!/bin/sh
if ["$0" -eq "-d"]
    then
        docker run -d wallhaven.discord
    else
        docker run wallhaven.discord
fi