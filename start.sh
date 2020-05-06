#!/bin/sh
if ["$0" -eq "-d"]
    then
        docker run --name=wallhaven.discord -d wallhaven.discord
    else
        docker run --name=wallhaven.discord wallhaven.discord
fi