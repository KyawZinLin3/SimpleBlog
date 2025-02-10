# SimpleBlog

SimpleBlog is a web application for creating, editing, and managing blog posts. It includes user authentication, post management, and a responsive UI.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Setup](#setup)
- [Usage](#usage)


## Features

- User registration and authentication
- Create, edit, delete, and view blog posts
- Tag management for posts
- Responsive design using Bootstrap
- Client-side validation 

## Technologies

- ASP.NET Core
- Entity Framework Core
- Identity for user management
- Bootstrap for responsive design
- jQuery for client-side scripting
- JSON Web Tokens (JWT) for authentication

## Setup

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. Clone the repository:
```   
git clone https://github.com/yourusername/SimpleBlog.git
cd SimpleBlog
```
2. Update the database connection string in `appsettings.json`:
```
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=SimpleBlog;User Id=your_user;Password=your_password;"
}
```
3. Apply database migrations:
```
dotnet ef database update
```
4. Run the application:
```
dotnet run
```
## Usage

1. Register a new user or log in with an existing account.
2. Create, edit, or delete blog posts by user.
3. Manage tags for your posts.
4. View posts by other users.


