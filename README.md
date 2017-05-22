# CareBreeze
Register patients and book a consultation

Project is built with ASP.NET Core MVC and EF Core using Visual Studio 2017

## Install .NET Core 1.1.0
Follow the steps outlined [here](https://www.microsoft.com/net/core#windowscmd)

## Run
1. Navigate to root folder, e.g. `C:\CareBreeze`
2. Run `dotnet restore`
3. Navigate to project directory `cd src\CareBreeze.WebApp`
4. Run `dotnet run`

The console should display    
```
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

## Seed data

Data is seeded from the file `InitializationData.json` that is located in root folder.

To load new data

1. Replace file content
2. Run `dotnet ef database drop` from directory `src\CareBreeze.WebApp`
3. Start application and database will be created and seeded with the data

## API
All resource endpoints return an envelope containing any errors, pagination results or other related meta data

**Example**
```
{
	"errors" : [
		{ 
			"key": <id for error>,
			"parameter": <paramater> (if null then 'message' is global),
			"message": <error message>
		},
		"data" : {}
	]
}
```

### Register patient
Resource: /patient/    
Http: POST    
Body    
```json
{ 
	"name": <string>,
	"condition":  Any of ["Flu", "Breast", "Head & Neck"]
}
```
Response iff success (Header.Location points to details)     
```
{ "patientId": <id>}
```

### View registered patients
Resource: /patient    
Http: GET    
Response    
```
{
	"patients": [
		{
			"id": 1,
			"name": "John Doe",
			"registered": "2017-05-21T22:49:51.6066667",
			"condition": "Head & Neck"
		}
	]
}
```

### View schedulated consultations
Resource: /consultation/    
Http: GET    
Response    
```
{
	"consultations": [
		{
			"patient": "John Doe",
			"doctor": "John",
			"room": "One",
			"conditation": "Head & Neck",
			"consultationDate": "2017-05-22T00:00:00",
			"registrationDate": "2017-05-21T22:49:52.3133333"
		}
	]
}
```