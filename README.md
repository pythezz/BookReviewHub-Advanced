# BookReviewHub

A community-driven book review platform built with **ASP.NET Core 8** and **Entity Framework Core**. Users can browse books, write reviews, manage a personal reading list, and explore authors and genres. Administrators have a dedicated panel for full content and user management.

**Live Demo:** https://bookreviewhub-stanislavstoychev-fjf7bqeyh0d7epc2.germanywestcentral-01.azurewebsites.net/

**GitHub:** https://github.com/pythezz/BookReviewHub-Advanced

---

## Features

- **Browse & Search** — filter books by title or author, narrow by genre, sort by title / year / top rated
- **Pagination** — books listed 6 per page with full pagination controls
- **Book Details** — full info page with average rating calculated from real user reviews
- **Reviews** — authenticated users can write one review per book, edit or delete their own; admins can delete any review
- **Reading List** — add books with Want to Read / Currently Reading / Read status, grouped list view
- **Authors** — dedicated author pages with biography, nationality, and book count
- **Genres** — genre browsing with book counts
- **Authentication** — register, login, logout via ASP.NET Core Identity
- **Two Roles** — User and Administrator with different access levels
- **Admin Panel** — dedicated area to manage books, genres, users; promote/demote roles, delete accounts
- **Custom Error Pages** — 404 Not Found and 500 Server Error pages

---

## Architecture

```
BookReviewHub/
├── Areas/
│   └── Admin/
│       ├── Controllers/        # Admin-only: BooksController, GenresController, UsersController
│       └── Views/              # Admin views with separate _AdminLayout
├── Controllers/                # BooksController, AuthorsController, GenresController,
│                               #   ReviewsController, ReadingListController,
│                               #   AccountController, HomeController
├── Data/
│   └── ApplicationDbContext.cs # EF Core + Identity context with full seeding
├── Helpers/
│   └── PaginatedList.cs        # Generic pagination helper
├── Models/                     # Entity models: Book, Author, Genre, Review,
│                               #   ApplicationUser, ReadingListItem
├── Services/
│   ├── Interfaces/             # IBookService, IAuthorService, IGenreService,
│   │                           #   IReviewService, IReadingListService
│   └── *.cs                    # Service implementations
├── ViewModels/                 # DTOs — entity models are never exposed to views directly
└── Views/                      # Razor views per controller + Shared layout, partials

BookReviewHub.Tests/
└── Services/                   # NUnit + EF In-Memory tests for all services
    ├── BookServiceTests.cs
    ├── ReviewServiceTests.cs
    ├── GenreServiceTests.cs
    └── AuthorServiceTests.cs
```

### Design Decisions

- **Service layer** — all business logic lives in services; controllers only handle HTTP concerns
- **ViewModel / DTO pattern** — entity models are never passed directly to views
- **Interface-driven services** — every service is registered against an interface, enabling easy mocking in tests
- **SOLID principles** — Single Responsibility per service, Dependency Inversion via constructor injection throughout
- **`PaginatedList<T>`** — generic helper wraps `IQueryable<T>`, keeping pagination logic out of controllers
- **Admin Area** — fully separated MVC area with its own layout, controllers, and views; all actions protected by `[Authorize(Roles = "Administrator")]`

---

## Entity Models (6)

| Model | Key Fields |
|---|---|
| `ApplicationUser` | Extends `IdentityUser` — `DisplayName`, `RegisteredAt` |
| `Book` | `Title`, `Description`, `PublicationYear`, `AuthorId` FK, `GenreId` FK |
| `Author` | `Name`, `Biography`, `Nationality`, `BirthYear` |
| `Genre` | `Name` |
| `Review` | `Content`, `Rating` (1–5), `CreatedAt`, `BookId` FK, `UserId` FK |
| `ReadingListItem` | `UserId` FK, `BookId` FK, `Status` (enum), `AddedAt` |

---

## Controllers (7 public + 3 admin)

| Controller | Area | Key Actions |
|---|---|---|
| `HomeController` | — | `Index`, `NotFound404`, `ServerError` |
| `BooksController` | — | `Index` (search/filter/page), `Details`, `Create`, `Edit`, `Delete` |
| `AuthorsController` | — | `Index`, `Details`, `Create`*, `Edit`*, `Delete`* |
| `GenresController` | — | `Index`, `Details`, `Create`*, `Edit`*, `Delete`* |
| `ReviewsController` | — | `Create`, `Edit`, `Delete`, `MyReviews` |
| `ReadingListController` | — | `Index`, `Add`, `Remove`, `UpdateStatus` |
| `AccountController` | — | `Register`, `Login`, `Logout` |
| `Admin/BooksController` | Admin | `Index`, `Edit`, `Delete` |
| `Admin/GenresController` | Admin | `Index`, `Create`, `Edit`, `Delete` |
| `Admin/UsersController` | Admin | `Index`, `Promote`, `Demote`, `Delete` |

*Admin only (`[Authorize(Roles = "Administrator")]`)

---

## Views (20+)

Books (5), Authors (5), Genres (5), Reviews (2), Account (2), ReadingList (1), Home (1), Shared (5), Admin Books (2), Admin Genres (3), Admin Users (1) = **32 views**

---

## Security

| Concern | Implementation |
|---|---|
| CSRF | `[ValidateAntiForgeryToken]` on every POST action |
| XSS | Razor HTML-encodes all output by default |
| SQL Injection | EF Core parameterised queries only — no raw SQL |
| Role-based auth | `[Authorize(Roles = "Administrator")]` on all admin and destructive actions |
| Ownership checks | Users can only edit/delete their own reviews; admins bypass this |
| Input validation | `DataAnnotations` on all ViewModels + jQuery Unobtrusive client-side validation |

---

## Validation

All inputs are validated at two levels:

**Server-side** — `[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`, `[DataType]`, `[Compare]` attributes on every ViewModel. `ModelState.IsValid` checked on every POST action before any data is saved.

**Client-side** — jQuery Unobtrusive Validation wired up automatically via `_ValidationScriptsPartial` on every form page.

---

## Seeding

`ApplicationDbContext.OnModelCreating` seeds on first migration:

- **10 genres** — Fiction, Non-Fiction, Science Fiction, Fantasy, Mystery, History, Biography, Self-Help, Romance, Thriller
- **5 authors** — George Orwell, J.K. Rowling, Frank Herbert, Agatha Christie, J.R.R. Tolkien
- **6 books** — with correct author/genre relationships
- **2 roles** — `Administrator` and `User`
- **Admin account** — `admin@bookreviewhub.com` / `Admin@123456`

---

## Unit Tests

Tests use **NUnit** with the **EF Core In-Memory** provider. Each test method gets its own isolated database to prevent test bleed.

| Service | Tests |
|---|---|
| `BookService` | 14 |
| `ReviewService` | 10 |
| `AuthorService` | 8 |
| `GenreService` | 9 |
| **Total** | **41** |

**Run in Visual Studio:**
Test → Run All Tests

**Run in terminal:**
dotnet test

**Run with detailed output:**
dotnet test --logger "console;verbosity=normal"

---

## Setup

### Prerequisites
- .NET 8 SDK
- SQL Server or LocalDB (included with Visual Studio)
- Visual Studio 2022

### Steps

# 1. Clone the repo
git clone https://github.com/pythezz/BookReviewHub-Advanced.git
cd BookReviewHub-Advanced

# 2. Restore packages
dotnet restore

# 3. Update connection string in appsettings.json if needed
# Default: Server=(localdb)\mssqllocaldb;Database=BookReviewHubDb;...

# 4. Apply migrations (creates DB and seeds all data)
cd BookReviewHub
dotnet ef database update

# 5. Run
dotnet run
```

Navigate to `https://localhost:7176`

**Admin login:** `admin@bookreviewhub.com` / `Admin@123456`

---

## Tech Stack

| Technology | Usage |
|---|---|
| ASP.NET Core 8 | Web framework |
| Entity Framework Core 8 | ORM and migrations |
| ASP.NET Core Identity | Authentication and role management |
| SQL Server / LocalDB | Database |
| Bootstrap 5 | Responsive UI |
| jQuery Validation | Client-side form validation |
| NUnit | Unit testing framework |
| EF Core InMemory | In-memory database for tests |
