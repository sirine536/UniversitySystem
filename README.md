# University Management System

A web-based University Management System developed to organize and manage university-related information through a centralized platform.

## Overview

The University Management System is an academic project designed to support common university management tasks, including managing students, doctors, courses, programs, and announcements.

The project focuses on implementing role-based access and organizing university data using a database-driven web application.

## Features

- **Authentication:** Login system for different user roles.
- **Role-Based Dashboards:** Separate dashboards for administrators, doctors, and students.
- **User Management:** Manage user information based on the application's roles and permissions.
- **Course Management:** Organize courses and their related information.
- **Program Management:** Associate courses and users with academic programs.
- **Announcements:** Store and display university announcements.

## Technologies Used

- **C#** — Application logic and server-side programming.
- **ASP.NET Web Forms** — Web application framework.
- **.NET Framework 4.7.2** — Application runtime and framework.
- **Microsoft SQL Server** — Database management.
- **ADO.NET** — Database connectivity and SQL operations.
- **HTML / CSS** — Page structure and styling.
- **JavaScript** — Client-side scripting, where used.
- **Visual Studio** — Development environment.
- **Git & GitHub** — Version control and source code collaboration.

## Project Structure

The application is organized into pages and components for different user roles, including:

- `Admin/` — Administrator pages and dashboard.
- `Doctor/` — Doctor pages and dashboard.
- `Student/` — Student pages and dashboard.
- `Login.aspx` — Login page.
- `Web.config` — Application configuration.
- `UniversitySystem.csproj` — C# project file.

*Folder and file names may vary depending on the current project structure.*

## Team

This project was developed collaboratively by a team of **three members** as an academic project.

The team worked together on the design and implementation of the university management features.

## Getting Started

### Prerequisites

- Windows
- Visual Studio with ASP.NET and .NET Framework 4.7.2 development support
- Microsoft SQL Server or SQL Server LocalDB
- A compatible database schema and seed data, if required

### Setup

1. Clone or download this repository.
2. Open `UniversitySystem.sln` in Visual Studio, or open the project file `UniversitySystem.csproj`.
3. Configure the database connection string in `Web.config` for your local SQL Server instance.
4. Create or restore the required database and tables.
5. Build the solution in Visual Studio.
6. Run the application using IIS Express.

> **Note:** The application currently requires local database configuration. It is source code hosted on GitHub and is not yet deployed as a live production website.

## Future Improvements

- Secure password storage using password hashing.
- Improve validation and error handling.
- Configure production-ready database hosting.
- Deploy the application to a compatible ASP.NET hosting environment.

## Project Status

Development project — source code available on GitHub. Production deployment is planned for a future stage.
