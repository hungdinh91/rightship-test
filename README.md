# rightship-test
Rightship Assignment

# I. Setup locally
#1. Prerequisites
- Windows OS
- Docker Desktop
- .Net SDK 8.0
- Visual studio

#2. Steps
- Extract source code (rightship-test.zip) to folder rightship-test. This folder should includes src folder
- Open docker desktop to start docker engine
- Turn off any services running on port 1433 (will be used by sql server), 6379 (will be used by Redis), 8080 and 8081 (will be used by API service) 
- Run this command in folder rightship-test: 
	`docker compose up` 
- Wait a few minutes to services get up
- Now, sql server is up on 1433, Redis is up on 6379, API service is up on https 8081 and http 8080
- Ensure appsettings.json file of OrderService.ConsoleApp have this:
`{
  "ConnectionStrings": {
    "OrderApiHost": "https://localhost:8081/"
  },
  "Logging": {
    "LogLevel": {
      "Default": "None",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}`

- Go to folder `rightship-test\src\OrderService.ConsoleApp\`
- Run command `dotnet run` to run console application


 
