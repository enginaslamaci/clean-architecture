# README #
This README would normally document whatever steps are necessary to get your application up and running.

## Technologies:
    ASP.NET Core 8
    Entity Framework Core 8
    ASP.NET Core Identity 2.2
    Docker / Docker Compose
    Rest API
    JWT
    OAuth 2.0 (Google) / OpenID
    MediatR
    RateLimit
    Serilog
    FluentValidation
    AutoMapper
    Refit
	
---------------------------

## Create Database:
    set startup project CleanArch.API and select CleanArch.Persistence as default project from package manager console
    add-migration v1
    update-database
	
---------------------------
	
## Docker Compose Run:
    powershell command:
      cd project_main_directory
      docker compose up -d  (-d -> run in detach mode)

    sql migrate:
      while database container migrate, appsettings.Development.json should be include connectionString server section 'Server=localhost,1433' and sqlserver domain login infos
	
    api_container: http://localhost:5000/swagger/index.html
    web_container: http://localhost:8000
	
---------------------------
		
## Postman Collection:	
    path: docs/CleanArchitecture.postman_collection.json
