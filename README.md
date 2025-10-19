# POS System

## Prereqs
- Docker & Docker Compose
- Node 20 (nvm recommended)
- .NET SDK 8

## Run with Docker
```bash
cd docker
docker compose up -d --build
```
- API: http://localhost:5000/swagger
- SQL: localhost:1433 (sa/Your_strong_password123!)

## Local development
- Backend
```bash
dotnet build Pos.sln
dotnet run --project backend/Pos.Api
```
- Frontend
```bash
cd frontend
. "$HOME/.nvm/nvm.sh" && nvm use 20
npm install
npm start
```

## Database
- SQL schema and procedures are in `database/` and mounted into the SQL container at startup.
