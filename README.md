# To-Do List Application (Angular & .NET API)

A full-stack task management application that allows users to create, view, toggle completion status, and delete tasks. This project demonstrates a decoupled architecture with a modern frontend and a robust RESTful API.

## 🚀 Technologies Used

### Frontend
* **Angular 18+**: Framework for building the client-side application.
* **Tailwind CSS**: Utility-first CSS framework for styling.
* **ngx-toastr**: For real-time toast notifications.
* **Lucide Angular**: Modern and clean iconography.

### Backend
* **.NET 8 Web API**: For building the server-side logic and RESTful endpoints.
* **Entity Framework Core**: ORM for database access and management.
* **Repository Pattern**: Used for clean separation of business logic and data access.

## 🛠️ Installation & Setup

Since the repository excludes build files and dependencies (via `.gitignore`), follow these steps to set up your local environment:

### Clone the Repository
git clone 
cd your-repo

### Configure the Backend (.NET)
Navigate to the backend folder: cd backend.
Restore dependencies and build the project: dotnet build
Run the API:  dotnet run

### Configure the Frontend (Angular)
Navigate to the frontend folder: cd frontend.
Install the required npm packages: npm install
Start the development server: ng serve

📋 Features
Task Visualization: Dynamic list that consumes data from the C# API.

Task Creation: Form with validation and success/error notifications using ngx-toastr.

Completion Toggle: Update task status (Completed/Incomplete) with immediate visual feedback.

Task Deletion: Remove records from the database with a confirmation dialog.

📂 Project Structure
/frontend: Angular project files (Components, Services, Models).

/backend: C# API project (Controllers, Repositories, Data Context).