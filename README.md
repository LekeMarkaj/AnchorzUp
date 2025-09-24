# AnchorzUp - URL Shortener

A modern, full-stack URL shortening service built with .NET 8, React, and SQLite. Transform long URLs into short, memorable links with comprehensive tracking, flexible expiration dates, and QR code generation.

## 📋 Prerequisites

Before running this project, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v16 or higher)
- [Git](https://git-scm.com/)

### Install Entity Framework Core Tools

After cloning the repository, install the EF Core tools globally:

```bash
dotnet tool install --global dotnet-ef
```

## 🛠️ Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd AnchorzUp
```

### 2. Development Setup

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

### 3. Backend Setup

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

### 4. Frontend Setup

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Create environment file manually:
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
- **Click Tracking**: View how many times each link has been clicked
- **Expiration**: Links automatically expire based on the set date
- **Delete Links**: Remove unwanted short URLs
- **Real-time Updates**: See click counts update in real-time
- **Responsive Design**: Works seamlessly on desktop and mobile devices
