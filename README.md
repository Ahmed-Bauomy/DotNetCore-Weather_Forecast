# WeatherForecast API

A **Clean Architecture .NET Core minimal Web API** built with **.NET 8** that retrieves weather forecasts by city name.  
The project demonstrates **JWT authentication**, **caching**, and **testability** with **unit and integration tests**.

---

## 📂 Project Structure

```
WeatherForecast/
├── src/
│   ├── Core/
│   │   ├── WeatherForecast.Domain/        # Entities and core domain models
│   │   └── WeatherForecast.Application/   # Application logic, interfaces, DTOs, services
│   └── External/
│   │   ├── WeatherForecast.API/           # Minimal API (endpoints, middleware, auth)
│   │   └── WeatherForecast.Infrastructure/# EF Core, repositories, DB
├   └── UnitTests/
│       └── WeatherForecast.UnitTest/                # Unit tests
│       └── WeatherForecast.IntegrationTest/         # integration tests
└── README.md
```

---

## 🚀 Features

- **JWT Authentication**
  - User Registration (`/api/auth/register`)
  - User Login (`/api/auth/login`)
- **Weather Endpoint**
  - `GET /api/weather?city=CityName`
  - Requires authentication (JWT Bearer token)
  - Uses mocked weather service with caching
- **Caching**
  - In-memory caching for performance optimization
- **Testing**
  - Unit & Integration tests with `xUnit`

---

## 🛠️ Tech Stack

- .NET 8
- ASP.NET Core Minimal API
- Entity Framework Core (SQL Server LocalDB)
- JWT Authentication
- In-Memory Caching
- xUnit for Unit & Integration Tests

---

## ⚙️ Setup Instructions

### 1. Clone the Repository
```bash
git clone [https://github.com/Ahmed-Bauomy/DotNetCore-Weather_Forecast]
cd DotNetCore-Weather_Forecast
```

### 2. Database Migration (SQL Server LocalDB)
Make sure you have **SQL Server LocalDB** installed (comes with Visual Studio).  
migration will automatically be applied, but if error occurs in migrations
You can Run the following inside the `WeatherForecast.Infrastructure` project directory:

```bash
dotnet ef database update
```


### 3. Run the Application
From the `WeatherForecast.API` project:

```bash
dotnet run
```

The API will be available at:
```
http://localhost:5219
```

### 4. Available Endpoints
| Method | Endpoint              | Description           |
|--------|------------------------|-----------------------|
| POST   | `/api/auth/register`  | Register new user     |
| POST   | `/api/auth/login`     | Authenticate user     |
| GET    | `/api/weather?city=xx`| Get weather forecast  |

---

## 🧪 Running Tests

Navigate to the `tests/WeatherForecast.UnitTests` folder and run:

```bash
dotnet test
```

Both **unit** and **integration tests** will run.  
Integration tests use `WebApplicationFactory` to spin up the API in-memory.

---

## 📌 Notes

- No ASP.NET Identity Framework used (custom auth implementation).  
- SQL Server LocalDB is used for persistence (in-memory DB could also be swapped for tests).  
- Weather service is **mocked** for simplicity, but can be extended to call real APIs.
