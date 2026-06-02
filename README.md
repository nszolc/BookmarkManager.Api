-# Bookmark Manager

Personal bookmark manager with REST API and web frontend. Save, organize, tag and filter your links.

## Tech Stack

- **Backend:** ASP.NET Core 9, Entity Framework Core, SQL Server
- **Validation:** FluentValidation
- **Frontend:** HTML, CSS, vanilla JavaScript (fully AI generated in order to learn how to connect a frontend)
- **Database:** SQL Server (via SSMS)

## Features

- Full CRUD for bookmarks, categories and tags
- Filter bookmarks by category, search text or favorites
- Many-to-many relationship between bookmarks and tags
- Custom exception hierarchy with global error handling middleware
- Request logging middleware (method, path, status code, response time)
- Input validation with FluentValidation
- Responsive dark-themed frontend with live API integration (separate pages for bookmarks, categories and tags)

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or LocalDB)
- [SQL Server Management Studio](https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms) (optional, for inspecting data)

### Setup

1. Clone the repository:
```bash
git clone https://github.com/nszolc/BookmarkManager.git
cd BookmarkManager
```

2. Update the connection string in `appsettings.json` with your SQL Server instance:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BookmarkManagerApi;Trusted_Connection=true;TrustServerCertificate=true"
}
```

3. Apply migrations:
```bash
cd BookmarkManager.Api
dotnet ef database update
```

4. Run the application:
```bash
dotnet run
```

5. Open the frontend at `http://localhost:5127/index.html` and the API docs at `http://localhost:5127/swagger` (adjust port as needed).

## Project Structure

```
BookmarkManager.Api/
├── wwwroot/
│   ├── index.html                  # Bookmarks page (HTML/CSS/JS)
│   ├── categories.html             # Categories management page
│   └── tags.html                   # Tags management page
├── Controllers/
│   ├── BookmarksController.cs      # Bookmark CRUD + filtering
│   ├── CategoriesController.cs     # Category CRUD
│   └── TagsController.cs           # Tag CRUD
├── Data/
│   └── AppDbContext.cs             # EF Core database context
├── DTOs/
│   ├── BookDto/
│   │   ├── BookmarkCreateDto.cs    # Input for creating bookmarks
│   │   ├── BookmarkUpdateDto.cs    # Input for updating bookmarks
│   │   └── BookmarkResponseDto.cs  # API response shape
│   ├── CategoryDto/
│   │   ├── CategoryCreateDto.cs
│   │   └── CategoryResponseDto.cs
│   └── TagDto/
│       ├── TagCreateDto.cs
│       └── TagDto.cs
├── Exceptions/
│   └── AppExceptions.cs            # NotFoundException, BadRequestException, ConflictException
├── Middleware/
│   ├── GlobalExceptionMiddleware.cs  # Catches exceptions → JSON responses
│   └── RequestLoggingMiddleware.cs   # Logs HTTP method, path, status, time
├── Migrations/                     # EF Core migrations
├── Models/
│   ├── Bookmark.cs
│   ├── BookmarkTag.cs              # Join entity (many-to-many)
│   ├── Category.cs
│   └── Tag.cs
├── Validators/
│   ├── BookDto/
│   ├── CategoryDto/
│   └── TagDto/
├── Program.cs                      # App entry point and middleware pipeline
├── appsettings.json
└── BookmarkManager.Api.http        # HTTP test requests
```

## API Endpoints

### Bookmarks

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/bookmarks` | List all bookmarks (supports filtering) |
| GET | `/api/bookmarks/{id}` | Get bookmark by ID |
| POST | `/api/bookmarks` | Create a bookmark |
| PUT | `/api/bookmarks/{id}` | Update a bookmark |
| DELETE | `/api/bookmarks/{id}` | Delete a bookmark |
| PATCH | `/api/bookmarks/{id}/favorite` | Toggle favorite status |

**Query parameters for GET /api/bookmarks:**
- `categoryId` — filter by category
- `search` — search in title, URL and description
- `favorite` — show only favorites (true/false)

### Categories

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/categories` | List all categories |
| POST | `/api/categories` | Create a category |
| DELETE | `/api/categories/{id}` | Delete a category |

### Tags

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tags` | List all tags |
| POST | `/api/tags` | Create a tag |
| DELETE | `/api/tags/{id}` | Delete a tag |

## Architecture

### Middleware Pipeline

```
Request → [GlobalExceptionMiddleware] → [RequestLoggingMiddleware] → [CORS] → [Controller]
Response ← [GlobalExceptionMiddleware] ← [RequestLoggingMiddleware] ← [CORS] ← [Controller]
```

**GlobalExceptionMiddleware** catches `AppException` (returns proper HTTP status) and unknown exceptions (returns 500). **RequestLoggingMiddleware** logs every request with response time in milliseconds.

### Custom Exceptions

Controllers throw typed exceptions instead of returning error responses directly. The middleware converts them to consistent JSON:

- `NotFoundException` → 404
- `BadRequestException` → 400
- `ConflictException` → 409

### Validation

FluentValidation validators run manually in controller actions. Each input DTO has a matching validator class that defines rules for required fields, max lengths, URL format, hex color format, etc.

