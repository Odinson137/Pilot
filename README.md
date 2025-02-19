# Pilot Project

## Overview
The **Pilot** project is a robust microservice-based solution for project management, aimed at organizations involved in software development, service provision, and other team-oriented activities. The platform enables efficient collaboration through features like task management, team communication, and progress tracking.

### Key Features
- **Project Management**: Manage projects, tasks, teams, and timelines.
- **Real-Time Communication**: Integrated chat functionality powered by SignalR.
- **Task Notifications**: Automated notifications for task updates.
- **Role and Skill Management**: Assign roles and manage skills for team members.
- **Data Storage**: Secure file storage integrated with Google Cloud Storage.
- **Background Jobs**: Delayed and scheduled tasks using Hangfire.

## Architecture
The Pilot project follows a **microservice architecture** and consists of the following services:

### 1. **ApiGateway**
- **Purpose**: Acts as the entry point for the web application.
- **Responsibilities**: Routing and load balancing between services.

### 2. **IdentityServer**
- **Purpose**: Manages user authentication and authorization.
- **Database**: Stores user credentials and roles in MongoDB.

### 3. **WorkerServer**
- **Purpose**: Handles employee, company, project, and task management.
- **Database**: MySQL for structured data storage.

### 4. **MessengerServer**
- **Purpose**: Manages chats and messages, providing real-time communication using SignalR.
- **Database**: Redis for caching and MySQL for persistence.

### 5. **CapabilityServer**
- **Purpose**: Manages user skills, positions, and related data.
- **Database**: MySQL.

### 6. **StorageServer**
- **Purpose**: Provides file storage services integrated with Google Cloud Storage.
- **Database**: MySQL for metadata storage.

### 7. **Background Jobs**
- **Purpose**: Manages scheduled and delayed actions using Hangfire.
- **Database**: SQL Server for Hangfire's task data.

## Technology Stack
- **Backend**: ASP.NET Core, MediatR, SignalR, MassTransit.
- **Frontend**: Blazor WebAssembly.
- **Databases**: MySQL, Redis, MongoDB.
- **Queue Management**: RabbitMQ.
- **Caching**: Redis.
- **File Storage**: Google Cloud Storage.
- **Task Scheduling**: Hangfire.
- **Containerization**: Docker and Docker Compose.

## Deployment
### Prerequisites
- Docker and Docker Compose installed.

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/Odinson137/Pilot.git
   cd Pilot
   ```
2. Build and start the services:
   ```bash
   docker-compose up --build
   ```
3. Access the application at `http://localhost:8080`.

### Configuration
Configurations for each microservice are stored in `appsettings.json`. Ensure that environment-specific settings (e.g., connection strings, JWT keys) are correctly set.

## Features by Service
### **IdentityServer**
- User registration and login with JWT-based authentication.
- Password hashing with salt for secure storage.

### **MessengerServer**
- Real-time chat functionality with SignalR.
- Group management and message history.

### **WorkerServer**
- Manage employees, projects, tasks, and teams.
- Supports role-based access control.

### **CapabilityServer**
- Manage and assign user skills and roles.
- Track skill progression.

### **StorageServer**
- File upload and retrieval via Google Cloud Storage.
- Metadata management in MySQL.

### **Background Jobs**
- Delayed notifications and reminders using Hangfire.
- Scheduled tasks with SQL Server as the backing store.

## Development
### Running Locally
1. Install prerequisites: .NET 6.0 SDK, Docker.
2. Run the infrastructure services:
   ```bash
   docker-compose up -d
   ```
3. Start individual services in development mode:
   ```bash
   dotnet run --project Pilot.Api
   ```

### Testing
- Unit tests are implemented using xUnit.
- Run tests with the following command:
   ```bash
   dotnet test
   ```

## Future Improvements
- Centralized configuration management with Consul or Vault.
- Enhanced logging with distributed tracing.
- Improved test coverage with integration tests.

## License
This project is licensed under the MIT License. See the LICENSE file for details.

## Contact
For questions or support, please reach out to the project maintainer at [GitHub](https://github.com/Odinson137/Pilot).

