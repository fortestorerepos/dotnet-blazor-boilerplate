SOLUTION := ./blazor-boilerplate.slnx
SERVER_PROJECT := ./Server/blazor-boilerplate.csproj
DEV_PROFILE ?= http
PROD_URLS ?= http://localhost:5000
DOTNET ?= dotnet

ifneq (,$(wildcard .env))
include .env
export
endif

.DEFAULT_GOAL := build

.PHONY: help restore build dev start console publish clean

help:
	@echo "Targets:"
	@echo "  make dev      Run the app in Development with dotnet watch"
	@echo "  make start    Run the app in Production with Release configuration"
	@echo "  make console  Run an app console command, e.g. make console ARGS=\"list\""
	@echo "  make build    Build the solution"
	@echo "  make restore  Restore NuGet packages"
	@echo "  make publish  Publish a Release build to ./publish"
	@echo "  make clean    Clean the solution"

restore:
	$(DOTNET) restore $(SOLUTION)

build:
	$(DOTNET) build $(SOLUTION)

dev:
	$(DOTNET) watch --project $(SERVER_PROJECT) run --launch-profile $(DEV_PROFILE)

start:
	$(DOTNET) run --project $(SERVER_PROJECT) --configuration Release --no-launch-profile -e ASPNETCORE_ENVIRONMENT=Production -e DOTNET_ENVIRONMENT=Production -- --urls "$(PROD_URLS)"

console:
	$(DOTNET) run --project $(SERVER_PROJECT) -- console $(ARGS)

publish:
	$(DOTNET) publish $(SERVER_PROJECT) --configuration Release --output ./publish

clean:
	$(DOTNET) clean $(SOLUTION)
