Development helpers

Start both backend API and frontend (dev server) together using the included PowerShell script.

From repository root (Windows PowerShell):

```powershell
cd engage-gov-api\scripts
.\start-all.ps1
```

Options:
- -ApiPort (default 5000)
- -ApiHttpsPort (default 5001)
- -FrontendPort (default 5173)

The script runs `dotnet run` for the API and `npm run dev` for the frontend (assumes Node/npm installed). Logs are prefixed with [API] and [FRONT].

Docker-compose option (no local Node required)
-------------------------------------------
You can run both backend and frontend in containers using docker-compose from the `engage-gov-api` folder.

From repository root:

```powershell
cd engage-gov-api
docker compose up --build
```

- API will be available proxied at http://localhost:5001 (mapped to container port 80).
- Frontend (Vite) will be available at http://localhost:5173 and is configured to call the API at the internal service address `http://engagegov-api:80`.
