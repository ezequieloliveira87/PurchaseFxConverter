# Purchase FX Converter

Small API project for handling purchase transactions in USD and converting them to other currencies using exchange rates from the U.S. Treasury.

## 🔧 Tech Stack

- ASP.NET Core 9
- Mapster
- Flunt
- NUnit + Moq
- Swagger
- HttpClient + JSON APIs

## 🚀 Running the Project

1. Clone the repo:
   ```bash
   git clone https://github.com/ezequieloliveira87/PurchaseFxConverter.git
   ```

2. Set the environment (optional):
   ```bash
   export ASPNETCORE_ENVIRONMENT=Development
   ```

3. Run the API:
   ```bash
   dotnet run --project PurchaseFxConverter.Api
   ```

4. Access Swagger at:
   ```
   https://localhost:5001/swagger
   ```

## 📦 Features

- Create and fetch purchase transactions.
- Convert USD to other currencies based on exchange rates.
- Validations using Flunt.
- Custom error formatting via middleware.
- Mockable external service for tests.

## ✅ Sample Payload

```json
POST /api/transactions
{
  "description": "Laptop purchase",
  "transactionDate": "2024-06-30T00:00:00Z",
  "amountUSD": 1000
}
```

## 🧪 Running Tests

```bash
dotnet test
```

Test coverage is ~97%.

## 🌍 Currency Info

Exchange rates are provided by the U.S. Treasury:
https://fiscaldata.treasury.gov/datasets/exchange-rates

Currency codes accepted by the API are based on the field `country_currency_desc`.

## 📁 Project Structure

```
PurchaseFxConverter/
│
├── Api/                   # Entry point and controllers
├── Application/           # DTOs, services, view models
├── Domain/                # Entities, enums, validations
├── Infra/                 # External services (e.g., Treasury)
├── Tests/                 # Unit and integration tests
```

## 📌 Notes

- No database is required — in-memory persistence is used.
- Logging and error handling are centralized.