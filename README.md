# FilmStudion

FilmStudion is a website with a backend built using an API and a frontend where users can log in as either a film studio or an admin to see available movies.

The API can be tested using tools like Postman.

## Project Features:
- **API** for managing films, admin and filmstudios
- **C#** for backend development
- **HTML5 & JavaScript** for frontend

---

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/en-us/)
- Visual Studio or any code editor with .NET CLI support
- A modern web browser

---

## Getting Started: Backend
1. Clone the repository:
   ```bash
   git clone https://github.com/gytu24nn/FilmStudion
    ```
2. Install .NET SDK if it’s not already installed. 
3. Start the backend server (terminal): dotnet run or dotnet watch
4. Test the API using Postman or a similar tool

## Getting started frontend
1. Complete all the steps to start the backend.
2. Start the frontend using a tool like Live Server.
3. To log in, you ned to register a film studio using Postman or the API
4. Create a movie using Postman or the API to ensure there are movies available to view in the frontend.


## **API Usage:**

Below are some common API endpoints for interacting with the FilmStudion system. These examples demonstrate how to register filmstudio.

---

### **1. Register a Film Studio**  
**Endpoint:** `POST /api/filmstudio/register`  
**Description:** Creates a new film studio account.

**Request Body Example:**
```json
{

    "FilmstudioName": "tuva",
    "email": "tuva@mail.com",
    "password": "test",
    "city": "Värnamo"

}





