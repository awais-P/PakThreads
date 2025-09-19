# 🌐 Angular + ASP.NET Core Project — **PakThreads**

A **full-stack web application** built with:  
- **Angular** (frontend)  
- **ASP.NET Core** (backend)  
- **SQL Server Management Studio (SSMS)** (database)  

✨ Includes **JWT-based authentication**, **RESTful APIs**, **responsive Angular UI**, **SSMS integration**, and follows **Clean Architecture** principles.  

---

## 📂 Project Structure
```plaintext
PakThreads/
│── Pakthreads Frontend/
│   └── pakthreads-frontend/   # Angular app
│
│── Pakthreads Backend/
│   └── pakthreads-backend/    # .NET API
│
└── README.md
```

---

## 🚀 Features
- 🔑 User Authentication & Authorization (**JWT**)  
- ⚡ RESTful API built with **ASP.NET Core**  
- 🎨 Responsive UI with **Angular**  
- 🗄️ Database integration with **SSMS (SQL Server)**  
- 🧹 **Clean Architecture** for maintainability  

---

## 🛠️ Tech Stack
- **Frontend:** Angular 18, TypeScript, HTML, SCSS  
- **Backend:** ASP.NET Core 8, C#  
- **Database:** SQL Server (SSMS)  
- **Authentication:** JSON Web Tokens (JWT)  
- **Architecture:** Clean Architecture (Separation of Concerns)  

---

## ⚙️ Setup Instructions

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/awais-P/PakThreads.git
cd PakThreads
```
### 2️⃣ Setup Backend (.NET API)
```
cd "Pakthreads Backend/pakthreads-backend"
dotnet restore
dotnet run
```
### 3️⃣ Setup Frontend (Angular)
```
cd "Pakthreads Frontend/pakthreads-frontend"
npm install
ng serve -o
```
### 4️⃣ Database Setup (SSMS + EF Core)
#### 🔹 1. Configure the Connection String
Open **`appsettings.json`** in the backend project and update it with your SQL Server instance:  

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PakThreads;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
#### 🔹 2. Apply EF Core Migrations
Open Visual Studio → Tools → NuGet Package Manager → Package Manager Console
Run the following commands:
```
# Ensure the backend project is set as the Default Project
Add-Migration InitialCreate
Update-Database
```
