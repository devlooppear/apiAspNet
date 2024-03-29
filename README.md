# apiAspNet 💻

apiAspNet is a robust .NET API application built with clean architecture principles, JWT authentication, PostgreSQL database, Swagger for API documentation, and Nginx for serving the application in a production environment.

![PostgreSQL Logo](https://img.shields.io/badge/PostgreSQL-336791?logo=postgresql&logoColor=white)
![JWT Logo](https://img.shields.io/badge/JSON%20Web%20Token-000000?logo=json-web-tokens&logoColor=white)
![.NET Logo](https://img.shields.io/badge/.NET-512BD4?logo=.net&logoColor=white)
![C# Logo](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)
![EF Logo](https://img.shields.io/badge/Entity%20Framework-512BD4?logo=.net&logoColor=white)
![Docker Logo](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![OpenSSL Logo](https://img.shields.io/badge/OpenSSL-721412?logo=open-ssl&logoColor=white)
![Swagger Logo](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=black)
![Nginx Logo](https://img.shields.io/badge/Nginx-269539?logo=nginx&logoColor=white)


## 📋 Table of Contents

1. [Getting Started](#getting-started)
   - [Prerequisites](#prerequisites)
   - [Installation](#installation)
2. [Configuration](#configuration)
   - [Environment Variables](#environment-variables)
   - [JWT Configuration](#jwt-configuration)
3. [Database Setup](#database-setup)
   - [Migrations and Seeders](#migrations-and-seeders)
4. [Running the Application](#running-the-application)
   - [Development](#development)
   - [Production with Docker](#production-with-docker)
5. [API Documentation](#api-documentation)
6. [Contributing](#contributing)
7. [License](#license)

## 🚀 Getting Started

### Prerequisites

Before running the apiAspNet application, ensure you have the following prerequisites installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started) (optional for production setup)
- [OpenSSL](https://www.openssl.org/) (for generating JWT secret)

### Installation

1. **Clone the repository**:

   ```bash
   git clone <repository-url>
   cd apiAspNet
   ```

2. **Copy the environment file**:

   ```bash
   cp .env.example .env
   ```

3. **Generate a JWT secret**:

   ```bash
   openssl rand -base64 128
   ```

   Copy the generated hash and paste it into the `.env` file under `JWT_SECRET`.

4. **Install dependencies**:
   ```bash
   dotnet restore
   ```

## ⚙️ Configuration

### Environment Variables

Update the `.env` file with the following environment variables:

- `JWT_SECRET`: Secret key for JWT authentication.
- `JWT_ISSUER`: JWT issuer.
- `JWT_AUDIENCE`: JWT audience.
- `POSTGRES_DB`: PostgreSQL database name.
- `POSTGRES_USER`: PostgreSQL username.
- `POSTGRES_PASSWORD`: PostgreSQL password.
- `DB_HOST`: PostgreSQL host.

### JWT Configuration

JWT configuration is provided in the `.env` file. Update the JWT related environment variables according to your requirements.

## 🗃️ Database Setup

### Migrations and Seeders

Run the following commands to set up the database with migrations and seeders:

```bash
dotnet tool install --global dotnet-ef
```

```bash
dotnet ef migrations add InitialCreate
```

```bash
dotnet ef database update
```

## 🏃 Running the Application

- Development
  To run the application in development mode, use the following command:

```bash
dotnet run
```

The application will be accessible at http://localhost:5277.

## Production with Docker

For production deployment using Docker, build and run the Docker containers with the provided Dockerfiles. Ensure you have Docker installed and running.

```bash
docker-compose up --build
```

The application will be accessible at port 80.

`Note`: Ensure you have a dedicated PostgreSQL database for production use. I recommend use [Neon](https://neon.tech/)
for hosting your PostgreSQL database. It provides a reliable and scalable solution for hosting databases in production environments free. Good to try

## 📚 API Documentation

API documentation is provided using Swagger. You can access the documentation at http://localhost:5277/swagger/index.html.
