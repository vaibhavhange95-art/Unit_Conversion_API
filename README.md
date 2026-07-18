# Unit Conversion API

## Overview

Unit Conversion API is an ASP.NET Core Web API application that allows users to convert numerical values between different units of measurement.

The API supports multiple conversion categories including:

- Length
- Weight / Mass
- Temperature

The application is designed with a clean and maintainable architecture. Conversion data is currently maintained in code using an in-memory configuration approach. No database is required for the current implementation.

---

# Technology Stack

- ASP.NET Core Web API
- .NET 10
- C#
- Swagger / OpenAPI
- Dependency Injection
- RESTful API Architecture

---

# Features

## Supported Conversions

### Length

Supported units:

- Meter
- Kilometer
- Foot
- Mile

Examples:
Meter → Foot
Kilometer → Mile
---

### Weight / Mass

Supported units:

- Kilogram
- Gram
- Pound

Examples:
Kilogram → Pound
Gram → Kilogram



---

### Temperature

Supported units:

- Celsius
- Fahrenheit
- Kelvin

Examples:


Celsius → Fahrenheit
Fahrenheit → Celsius
Celsius → Kelvin



---

# Solution Architecture

The project follows a layered architecture approach:

Unit_Conversion_API

│
├── Controllers
│ API endpoints and HTTP request handling
│
├── DTOs
│ Request and response models
│
├── Models
│ Domain models
│
├── Services
│ Business logic implementation
│
│ ├── Interfaces
│ │ Service contracts
│ │
│ └── Implementation
│ Conversion logic
│
├── Constants
│ In-memory conversion configuration
│
├── Exceptions
│ Custom business exceptions
│
├── Middleware
│ Global exception handling
│
└── Program.cs


## Service Layer

Business logic is separated from controllers using interfaces and dependency injection.

Flow:
API Controller
|
|
IConversionService
|
|
ConversionService
|
|
Conversion Configuration


# API Documentation

Swagger UI is enabled for testing and exploring the API.

After running the application, open:
https://localhost:<port>/swagger

---

# API Endpoint

## Convert Units

### Request


POST /api/conversion


---

## Example Request

```json
{
  "category": "Length",
  "fromUnit": "Meter",
  "toUnit": "Foot",
  "value": 10
}

Example Response
{
  "originalValue": 10,
  "fromUnit": "Meter",
  "toUnit": "Foot",
  "convertedValue": 32.80839895013123,
  "category": "Length"
}


Error Handling

The API includes global exception handling middleware.

Example invalid request:

{
  "category": "Length",
  "fromUnit": "Meter",
  "toUnit": "Unknown",
  "value": 10
}

Response:

{
  "statusCode": 400,
  "message": "Source or target unit is not supported."
}

The API does not expose internal stack traces to consumers.




How to Run Locally
Prerequisites

Install:

Visual Studio 2022/2026
.NET 10 SDK
Steps
1. Clone Repository
git clone https://github.com/vaibhavhange95-art/Unit_Conversion_API/tree/Unit_Conversion_API
2. Open Solution

Open:

Unit_Conversion_API.sln

in Visual Studio.

3. Restore Dependencies

Visual Studio will automatically restore NuGet packages.

Alternatively:

dotnet restore

4. Run Application

Run using:

Visual Studio → F5

or:

dotnet run
5. Open Swagger

Navigate to:

https://localhost:<port>/swagger


Testing Examples
Length Conversion

Request:

{
  "category": "Length",
  "fromUnit": "Meter",
  "toUnit": "Foot",
  "value": 5
}

Expected result:

16.404 feet approximately



Weight Conversion

Request:

{
  "category": "Weight",
  "fromUnit": "Kilogram",
  "toUnit": "Pound",
  "value": 10
}

Expected result:

22.046 pounds approximately
Temperature Conversion

Request:
{
  "category": "Temperature",
  "fromUnit": "Celsius",
  "toUnit": "Fahrenheit",
  "value": 25
}
Expected result:

77 Fahrenheit


Future Improvements

Possible future enhancements:

Database support for unit management
Admin API to add/update units
Authentication and authorization
Unit validation rules
Automated unit tests
Logging and monitoring
Containerization using Docker
CI/CD pipeline integration


Git Workflow

The project follows standard Git practices:

Example:

git add .
git commit -m "Meaningful commit message"
git push

Each major feature is committed separately to maintain clear project history.




License

This project is created as a technical demonstration project.






