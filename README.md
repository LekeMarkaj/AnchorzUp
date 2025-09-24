# AnchorzUp - URL Shortener

A modern, full-stack URL shortening service built with .NET 8, React, and SQLite. Transform long URLs into short, memorable links with comprehensive tracking, flexible expiration dates, and QR code generation.

## 🚀 Features

- **URL Shortening**: Convert long URLs to short, shareable links with 8-character codes
- **Click Tracking**: Monitor how many times each link has been clicked with detailed analytics
- **Expiration Management**: Set custom expiration dates (1 minute to 5 hours) or use default 30-day expiration
- **QR Code Generation**: Generate and download QR codes for easy sharing
- **Modern UI**: Beautiful, responsive React frontend with Tailwind CSS and Lucide icons
- **Clean Architecture**: Well-structured .NET backend following Clean Architecture principles with MediatR
- **Database Persistence**: SQLite database with Entity Framework Core
- **Docker Support**: Full containerization with multi-stage builds and development profiles
- **Real-time Analytics**: Track IP addresses, user agents, and referrers for each click

## 🏗️ Architecture

This project follows Clean Architecture principles with the following layers:

- **Domain**: Core entities (`ShortUrl`, `Click`), interfaces, and business logic
- **Application**: CQRS pattern with MediatR for commands and queries, DTOs, and validators
- **Infrastructure**: Data access with EF Core, repositories, and external services (QR code generation)
- **API**: Web API controllers, middleware, and application configuration
- **Frontend**: React TypeScript application with modern UI components

### Technology Stack

**Backend:**
- .NET 8 Web API
- Entity Framework Core with SQLite
- MediatR for CQRS pattern
- QRCoder for QR code generation

**Frontend:**
- React 18 with TypeScript
- Tailwind CSS for styling
- Lucide React for icons
- Axios for API communication
- QRCode.react for client-side QR generation

## 📋 Prerequisites

Before running this project, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v16 or higher)
- [Docker](https://www.docker.com/) 
- [Git](https://git-scm.com/)

## 🛠️ Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd AnchorzUp
```

### 2. Quick Setup (Windows)

Run the automated setup script:

```powershell
.\setup.ps1
```

This script will:
- Check for required dependencies (.NET 8, Node.js)
- Restore .NET dependencies
- Install frontend dependencies
- Create environment configuration files

### 3. Development Setup

#### Option 1: Local Development (Recommended)

1. **Run the backend locally:**
   ```bash
   cd server/AnchorzUp.API
   dotnet run
   ```

2. **Run the frontend locally:**
   ```bash
   cd frontend
   npm start
   ```

The application uses SQLite database which is created automatically.

#### Option 2: Docker Deployment

Run the complete application with Docker:

```bash
docker-compose up -d
```

This will start:
- .NET API backend
- React frontend (served by the API)
- SQLite database (embedded)

### 4. Backend Setup

1. Navigate to the API project:
```bash
cd server/AnchorzUp.API
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Database migrations are automatically applied on startup, but you can run manually:
```bash
dotnet ef database update
```

4. Start the API:
```bash
dotnet run
```

**Alternative**: Open `AnchorzUp.sln` in Visual Studio and press F5 to run.

The API will be available at:
- HTTPS: `https://localhost:52053`
- HTTP: `http://localhost:52054`

### 5. Frontend Setup

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Environment file is created automatically by `setup.ps1`, or create manually:
```bash
# Create .env file in the frontend directory
echo "REACT_APP_API_URL=https://localhost:52053/api" > .env
```

4. Start the development server:
```bash
npm start
```

The React app will be available at `http://localhost:3000`.

## 🎯 Usage

### Creating Short URLs

1. Open the application in your browser
2. Enter a long URL in the input field
3. Optionally set an expiration date (1 minute to 5 hours, or default 30 days)
4. Click "Shorten URL" to generate a short link

### Features

- **Copy Links**: Click the copy button to copy short URLs to clipboard
- **QR Codes**: Each short URL includes a QR code for easy sharing and downloading
- **Click Tracking**: View how many times each link has been clicked with detailed analytics
- **Expiration**: Links automatically expire based on the set date
- **Delete Links**: Remove unwanted short URLs
- **Real-time Updates**: See click counts update in real-time
- **Responsive Design**: Works seamlessly on desktop and mobile devices

### API Endpoints

**Short URL Management:**
- `POST /api/shorturl/create` - Create a new short URL
- `GET /api/shorturl` - Get all short URLs
- `DELETE /api/shorturl/{id}` - Delete a short URL
- `GET /api/shorturl/qr/{shortCode}` - Get QR code image

**Redirect:**
- `GET /{shortCode}` - Redirect to original URL (tracks clicks automatically)

## 🗄️ Database Schema

### ShortUrls Table
- `Id` (Guid) - Primary key
- `OriginalUrl` (string, max 2048) - The original long URL
- `ShortCode` (string, max 10) - The unique 8-character short code
- `CreatedAt` (DateTime) - Creation timestamp (UTC)
- `ExpiresAt` (DateTime?) - Expiration date (optional, UTC)
- `IsActive` (bool) - Whether the URL is active
- `ClickCount` (int) - Number of times clicked
- `LastAccessedAt` (DateTime?) - Last access timestamp (UTC)

### Clicks Table
- `Id` (Guid) - Primary key
- `ShortUrlId` (Guid) - Foreign key to ShortUrls
- `ClickedAt` (DateTime) - Click timestamp (UTC)
- `IpAddress` (string?, max 45) - IP address of the clicker
- `UserAgent` (string?, max 500) - User agent string
- `Referer` (string?, max 100) - Referer URL

**Relationships:**
- One-to-Many: ShortUrl → Clicks (Cascade delete)
- Unique Index: ShortCode (ensures uniqueness)

## 🔧 Configuration

### Environment Variables

#### Backend (.NET)
- `ConnectionStrings:DefaultConnection` - SQLite database connection string
- `AppSettings:BaseUrl` - Base URL for generating short URLs

#### Frontend (React)
- `REACT_APP_API_URL` - Backend API URL (default: `https://localhost:52053/api`)

### Database Configuration

**SQLite (All Environments):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=anchorzup.db"
  }
}
```

### Customization

- **Short Code Length**: Modify `ShortCodeLength` constant in `ShortUrlService.cs` (default: 8)
- **Default Expiration**: Change the default expiration period in `CreateShortUrlAsync` (default: 30 days)
- **Base URL**: Update the `BaseUrl` in `appsettings.json`
- **QR Code Size**: Modify QR code size in `QrCodeService.cs` (default: 200px)

## 🧪 Testing

### Backend Testing
```bash
cd server/AnchorzUp.API
dotnet test
```

### Frontend Testing
```bash
cd frontend
npm test
```

### API Testing
Use tools like Postman, curl, or any HTTP client to test the API endpoints.

## 🚀 Deployment

### Docker Deployment (Recommended)

The project includes a complete Docker setup with multi-stage builds:

```bash
# Build and run the full application stack
docker-compose --profile prod up -d
```

This will:
- Build the .NET API backend
- Build the React frontend
- Serve the frontend from the API
- Use embedded SQLite database
- Expose the application on port 8080

### Manual Deployment

#### Backend Deployment

1. Build the application:
```bash
cd server/AnchorzUp.API
dotnet publish -c Release -o ./publish
```

2. Deploy to your preferred hosting platform (Azure, AWS, etc.)

#### Frontend Deployment

1. Build the React app:
```bash
cd frontend
npm run build
```

2. Deploy the `build` folder to your hosting platform

### Production Considerations

- **Database**: SQLite is suitable for small to medium applications
- **Environment Variables**: Set production connection strings and API URLs
- **HTTPS**: Ensure SSL certificates are properly configured
- **CORS**: Update CORS policies for production domains
- **Logging**: Configure appropriate logging levels for production

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Troubleshooting

### Common Issues

1. **Database Connection Issues**
   - Check if `anchorzup.db` file exists in the API directory
   - Verify connection string in `appsettings.json`
   - Ensure database migrations are applied (automatic on startup)

2. **CORS Issues**
   - Verify CORS policy in `Program.cs`
   - Check frontend API URL configuration in `.env`
   - Ensure frontend and backend are running on correct ports

3. **Port Conflicts**
   - Change ports in `launchSettings.json` for backend
   - Update `REACT_APP_API_URL` in frontend `.env`
   - Check if ports 3000, 52053, 52054, or 8080 are in use

4. **Build Issues**
   - Run `dotnet clean` and `dotnet restore`
   - Delete `node_modules` and run `npm install` for frontend
   - Check .NET and Node.js versions match requirements

5. **Docker Issues**
   - Ensure Docker is running
   - Check Docker logs: `docker-compose logs`
   - Rebuild containers: `docker-compose down && docker-compose up --build`

### Getting Help

If you encounter any issues:

1. Check the console logs for error messages
2. Verify all prerequisites are installed
3. Ensure all services are running
4. Check the troubleshooting section above
5. Use tools like Postman or curl for API testing

## 🎉 Acknowledgments

- Built with [.NET 8](https://dotnet.microsoft.com/)
- Frontend powered by [React](https://reactjs.org/) and [Tailwind CSS](https://tailwindcss.com/)
- Database: [SQLite](https://www.sqlite.org/)
- QR Code generation: [QRCoder](https://github.com/codebude/QRCoder)
- Icons: [Lucide React](https://lucide.dev/)
- CQRS Pattern: [MediatR](https://github.com/jbogard/MediatR)
- ORM: [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

---

**Happy URL Shortening! 🚀**