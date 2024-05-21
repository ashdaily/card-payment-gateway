# Card Payment Gateway

Card Payment Gateway is a dummy, payment processing solution written in C#. This project is designed to handle card transactions, integrating with multiple payment providers to ensure reliable and seamless transactions. 

## Features

- **Multi-provider integration:** Supports multiple payment providers for redundancy and reliability.
- **Extensibility:** Easily extendable to include additional payment providers or custom features.

## Getting Started

### Prerequisites

Ensure you have the following installed:

- .NET 8.0 SDK
- Docker (for containerized deployment)
- PostgreSQL (recommended database)

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/yourusername/card-payment-gateway.git
   cd card-payment-gateway
   ```

2. **Set up the database:**

   Ensure you have PostgreSQL running and create a database for the application. Update the connection string in `appsettings.json` located in the `src/PaymentGateway.Api` directory.

3. **Run the application:**

   ```bash
   cd src/PaymentGateway.Api
   dotnet run
   ```

   The application will start and be accessible at `http://localhost:5000`.

### Docker Setup

To run the application using Docker:

1. **Build the Docker image:**

   ```bash
   docker build -t card-payment-gateway .
   ```

2. **Run the Docker container:**

   ```bash
   docker-compose up
   ```

   The application will start and be accessible at `http://localhost:5000`.

### Running Tests

To ensure everything is set up correctly and working as expected, run the test suite:

1. **Navigate to the test project:**

   ```bash
   cd test/PaymentGateway.Api.Tests
   ```

2. **Run the tests:**

   ```bash
   dotnet test
   ```

## Usage

After setting up and running the server, you can access the payment gateway API at `http://localhost:5000/api/payments`. Refer to the API documentation for details on how to perform transactions and interact with the gateway.

### API Endpoints

- **POST /api/payments**: Initiate a new payment.
- **GET /api/payments/{id}**: Retrieve the status of a specific payment.


### Advanced Configuration

You can deploy the application using Docker:

1. **Build the Docker image:**

   ```bash
   docker build -t card-payment-gateway .
   ```

2. **Run the Docker container:**

   ```bash
   docker run -d -p 5000:80 card-payment-gateway
   ```

### API Documentation

Comprehensive API documentation can be generated using tools like Swagger integrated within the project.

---

