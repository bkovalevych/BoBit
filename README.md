# Bitcoin Price Tracker (BoBit)

This project is a microservice-based application that consists of three main components:
1. **Fetcher Service**: Fetches Bitcoin prices from the [CoinDesk API](https://api.coindesk.com/v1/bpi/currentprice.json) at regular intervals and stores them in a database.
2. **API Provider Service**: Provides an API to retrieve historical Bitcoin price data from the database for a specified time range.
3. **Angular App**: Displays a real-time chart of Bitcoin prices, along with the highest and average prices for the selected period.

The application uses **Docker Compose** to orchestrate the services.

Run the following command to build and start all services:
docker-compose up --build