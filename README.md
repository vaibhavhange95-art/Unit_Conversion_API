# Unit Conversion API

## Overview

Unit Conversion API is a RESTful Web API built using ASP.NET Core that converts numerical values between different units of measurement.

The API currently supports:

- Length
- Weight / Mass
- Temperature
- Volume (Dynamic Category)

The application is designed using a layered architecture and stores all unit definitions in memory. New categories and units can be added dynamically at runtime without changing the application code or using a database.
The **Unit_Conversion_API** provides REST endpoints for converting units, searching available units, Adding categories and units, adding required formulas and updating conversion factors.

---

# Features

- Convert values between supported units
- Search available units and categories
- Dynamic in-memory unit repository
- Add new conversion categories
- Define and manage conversion formulas
- Add new units to existing or new categories
- Update conversion factors dynamically via REST APIs
- Global exception handling
- Swagger/OpenAPI support
- Repository Pattern
- Service Layer Architecture
- Clean folder structure
- No database dependency

---

# Technologies Used

- ASP.NET Core (.NET 10 SDK)
- C#
- Swagger / OpenAPI
- Visual Studio 2026
- Git & GitHub

---

# Project Structure

```
Unit_Conversion_API
│
├── Controllers
│   ├── ConvertController.cs
│   └── UnitController.cs
│
├── DTOs
│
├── Exceptions
│
├── Middleware
│
├── Models
│
├── Repositories
│   ├── Interfaces
│   └── Implementation
│
├── Services
│   ├── Interfaces
│   └── Implementation
│
├── Program.cs
│
└── README.md
```

---

# Design Overview

The project follows a layered architecture.

```
Controller
     │
     ▼
Service Layer
     │
     ▼
Repository Layer
     │
     ▼
In-Memory Unit Store
```

Responsibilities:

- Controller
    - Handles HTTP Requests
    - Returns HTTP Responses

- Service
    - Contains business logic
    - Performs unit conversion

- Repository
    - Stores categories and units
    - Supports dynamic addition of new units

---

# Supported Categories

## Length

- Meter
- Kilometer
- Centimeter
- Millimeter
- Micrometer
- Nanometer
- Inch
- Foot
- Yard
- Mile

---

## Weight

- Kilogram
- Gram
- Milligram
- MetricTon
- Pound
- Ounce
- Stone
- Carat
- Microgram

---

## Temperature

- Celsius
- Fahrenheit
- Kelvin

---

## Volume
- Milliliter (mL)
- Liter (L)
- Cubic Meter (m³)
- Gallon 
- Quart

--- and can be added many more
  
---

# Dynamic Unit Management

The application stores all units inside an in-memory repository.

New categories and units can be added at runtime using the Add Unit API.

## API Endpoints

### 1. Convert Units
**POST** `/api/Convert`

Converts a value from one unit to another within the same category.
 
Sample Request

```json
{
  "category": "Length",
  "fromUnit": "Meter",
  "toUnit": "Kilometer",
  "value": 5000
}
```

Sample Response

```json
{
  "originalValue": 5000,
  "fromUnit": "Meter",
  "toUnit": "Kilometer",
  "convertedValue": 5,
  "category": "Length"
}
```
---

### 2. Search Units
**GET** `/api/Unit/SearchUnits?category={category}`

Returns all available units for the specified category.

**Example**
```
GET /api/Unit/SearchUnits?category=Len

## Example Categories

- `Len` – Length
- `Temp` – Temprature
```


Example Response

```json
[
  {
    "category": "Length",
    "units": [
      {
        "name": "Meter",
        "toBaseFactor": 1
      },
      {
        "name": "Kilometer",
        "toBaseFactor": 1000
      }
    ]
  }
]
```
---

### 3. Add New Unit
**POST** `/api/AddUnit`

Adds a new unit to an existing category.

S## Add Unit API

The `POST /api/AddUnit` endpoint is designed to add **one unit at a time**. ```

### Current Behavior

- ✅ Accepts a single unit per request.
- ✅ Creates the category automatically if it does not exist.
- ✅ Prevents duplicate unit names within the same category.
- ✅ Does support adding multiple units in a single request.

### Future Enhancement

A bulk insert endpoint (`POST /api/AddUnits`) to adding multiple units in a single request.

**Example Request**

```
[
  {
    "category": "Time",
    "name": "Millisecond",
    "toBaseFactor": 0.001
  },
  {
    "category": "Time",
    "name": "Second",
    "toBaseFactor": 1
  },
  {
    "category": "Time",
    "name": "Minute",
    "toBaseFactor": 60
  },
  {
    "category": "Time",
    "name": "Hour",
    "toBaseFactor": 3600
  }
]
```
 
If the unit already exists:

```json
{
  "message": "Unit 'Hour' already exists in category 'Time'."
}
```
- (Additional categories can be added as needed.)


### 4. Update Unit Base Factor
**PUT** `/api/UpdateUnitBaseFactor`

Updates the `ToBaseFactor` of an existing unit.

**Request Body**
```json
{
  "category": "Len",
  "name": "Kilometer",
  "toBaseFactor": 1200
}
```

---

## Response

- **200 OK** –Unit updated successfully.
- **404 Not Found** – Unit or category not found.
- **400 Bad Request** – Invalid request.

## 5. Get Available Unit Categories

**Endpoint**

```http
GET /api/Unit/AvailableUnitCategories
```

**Description**

Returns all supported unit categories available in the API.

**Sample Response**

```json
[
  "Length",
  "Weight",
  "Temperature",
  "Area",
  "Volume",
  "Speed",
  "Time"
]
```

No application restart or database changes are required.

---

# Error Handling

The API uses Global Exception Middleware.

Example Response

```json
{
    "statusCode": 400,
    "message": "Source or target unit is not supported."
}
```

---

# How to Run

## Prerequisites

- .NET SDK 10
- Visual Studio 2026 (or later)

---

## Steps

Clone repository

```
git clone https://github.com/vaibhavhange95-art/Unit_Conversion_API/tree/Unit_Conversion_API
```

Navigate to project

```
cd Unit_Conversion_API
```

Restore packages

```
dotnet restore
```

Build

```
dotnet build
```

Run

```
dotnet run
```

Open Swagger

```
https://localhost:<port>/swagger
```

---

# Design Decisions

- Repository Pattern is used to separate data access from business logic.
- In-memory storage is used instead of a database to keep the solution lightweight.
- Conversion logic is isolated in the Service Layer.
- Global Exception Middleware provides consistent error responses.
- New categories and units can be added dynamically at runtime.
- Duplicate units are prevented within the same category.
- The architecture can be extended to support persistent storage with minimal changes.

---

# Future Enhancements

- Database integration (SQL Server / PostgreSQL)
- Unit aliases (m, km, cm, ft, etc.)
- Update/Delete Unit APIs
- Authentication & Authorization
- Unit history and audit logging
- Caching
- Unit validation rules
- Support for hundreds or thousands of units using persistent storage

---

# Author

Vaibhav Hange