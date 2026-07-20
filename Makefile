SOLUTION := ./blazor-boilerplate.slnx
SERVER_PROJECT := ./blazor-boilerplate/blazor-boilerplate/blazor-boilerplate.csproj
DEV_PROFILE ?= http
PROD_URLS ?= http://localhost:5000

.PHONY: help restore build dev prod publish clean

help:
	@echo "Targets:"
	@echo "  make dev      Run the app in Development with dotnet watch"
	@echo "  make prod     Run the app in Production with Release configuration"
	@echo "  make build    Build the solution"
	@echo "  make restore  Restore NuGet packages"
	@echo "  make publish  Publish a Release build to ./publish"
	@echo "  make clean    Clean the solution"

restore:
	dotnet restore $(SOLUTION)

build:
	dotnet build $(SOLUTION)

dev:
	dotnet watch --project $(SERVER_PROJECT) run --launch-profile $(DEV_PROFILE)

prod:
	dotnet run --project $(SERVER_PROJECT) --configuration Release --no-launch-profile -e ASPNETCORE_ENVIRONMENT=Production -e DOTNET_ENVIRONMENT=Production -- --urls "$(PROD_URLS)"

publish:
	dotnet publish $(SERVER_PROJECT) --configuration Release --output ./publish

clean:
	dotnet clean $(SOLUTION)
