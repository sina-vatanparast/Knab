# Knab Cryptocurrency Solution

## Overview
This solution fetches and displays cryptocurrency data and exchange rates using .NET 8 and Onion Architecture with Domain-Driven Design (DDD). The flow is as follows:  
1. Fetches a list of cryptocurrencies from the CoinMarketCap API (https://coinmarketcap.com/).  
2. After selecting a cryptocurrency, fetches a quote for it against a default fiat currency (configurable in `appsettings.json`).  
3. Calls the ExchangeRates API (https://exchangeratesapi.io/) to get exchange rates for the default fiat currency against other currencies listed in `appsettings.json`.  
4. Displays the exchange rates on the frontend.

## Features
- Fetches cryptocurrency data and exchange rates.  
- Implements Onion Architecture for maintainability and scalability.  
- Includes CQRS with MediatR, global exception handling, and Serilog for logging.

## Project Structure
1. **Domain**: Core domain logic and models (`Currency`, `CryptoCurrency`, `Pair`, `AppSettings`).  
2. **Infrastructure**: Interacts with external APIs and maps data to domain models.  
3. **Application**: Business logic using CQRS and MediatR.  
4. **Presentation**: RESTful APIs and a simple JavaScript + Tailwind CSS client.

## How to Run
1. Clone the repository and rebuild the solution to install NuGet packages.  
2. Set `Knab.Cryptocurrency.Presentation` as the startup project.  
3. Replace the following API keys in `appsettings.json`:  
   - CoinMarketCap: `b20758e6-a181-487e-9c79-fc9b26453ff2`  
   - ExchangeRates: `3df3ba12170fa132f65accd56db5e00c`  
4. Run the project.

## Endpoints
1. `GET /cryptocurrencies`: Returns a list of cryptocurrencies.  
2. `GET /quotes/{cryptoCurrency}`: Returns exchange rates for the selected cryptocurrency.

## Notes
- No authentication or authorization.  
- Includes partial unit test coverage for infrastructure and business logic.  
- Logs errors to the console with Serilog.  
