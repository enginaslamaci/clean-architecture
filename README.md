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
	
## Docker Compose Run:
    * sql migrate:
	  -> connectionString server section should be 'Server=localhost,1433' when db migrate and include sqlserver domain infos
    
	* powershell command:
	   cd project_main_directory
	   docker compose up -d  (-d -> run in detach mode)
	
	api_container: http://localhost:5000/swagger/index.html
	web_container: http://localhost:8000
	
---------------------------
	
## Create Database:
    select CleanArch.Infrastructure as default project from package manager console
    add-migration v1
    update-database
	
---------------------------
	
## Postman Collection:	
	* path: docs/CleanArchitecture.postman_collection.json
