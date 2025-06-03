
# 📚 Mentor — Online Learning Platform

Mentor is a modern web-based learning platform inspired by Udemy, designed to offer online courses, manage trainers, and deliver educational content to users in an interactive and responsive interface.

## 🚀 Features

- 🖥️ **Responsive Course Catalog** — Users can browse and view detailed descriptions of available courses.
- 👨‍🏫 **Trainer Profiles** — Trainers are listed with their areas of expertise and professional bios.
- 🎥 **Course Videos & Resources** — Purchased courses include access to YouTube video tutorials and downloadable documents.
- 🛒 **Buy Courses System** — Registered users can purchase courses and access exclusive content.
- 📋 **User Course Management** — Tracks user-owned courses through a `UserCourses` relationship table.
- 🔒 **ASP.NET Identity Integration** — User authentication, registration, and secure course management.
- 📊 **Admin-Friendly** — Easy to manage course content, trainers, and resources.

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core MVC
- **Frontend:** HTML5, CSS3, Bootstrap 5
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **User Management:** ASP.NET Identity

## 📂 Database Structure

### 📄 Tables:
- `Courses` — Stores course information, descriptions, and video links.
- `Trainers` — Information about course trainers.
- `UserCourses` — Links users to purchased courses and tracks active subscriptions.
- `AspNetUsers` — Default ASP.NET Identity table for user accounts.

## 📝 How It Works

1. **Guests** can view course details and trainer profiles.
2. **Registered Users** can:
   - Buy courses.
   - Access purchased course videos and downloadable materials.
3. **Admin/Trainers** manage course content and trainer bios.

## 📌 Setup Instructions

1. Clone the repository.
2. Restore NuGet packages.
3. Configure `appsettings.json` for your local SQL Server connection string.
4. Import .bacpac file to your database
5. Change connection string in program.cs

## 📸 Screenshots

![Homepage Screenshot](./Mentor\wwwroot\assets\img\screenshots\Screenshot 2025-06-03 182806.png)

## 📧 Contact

For questions or suggestions, reach out at: **ezizliilafer@gmail.com**
