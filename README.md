# Course Management System (Simple LMS)

Welcome to the Course Management System, a simple Learning Management System (LMS) designed to facilitate the management and delivery of educational courses. This application caters to three types of users: Admins, Instructors, and Students. Below is a detailed description of the functionalities provided by each user role.

## Features

### Admin
- **Course Management:** Add, edit, delete courses.
- **User Management:** Create accounts for users (Instructors and Students), and manage their roles and permissions.
- **Major Management:** Perform CRUD (Create, Read, Update, Delete) operations for majors associated with courses.
- **Overall Control:** Admins have the highest level of control, managing the structure and organization of the entire application.

### Instructor
- **Course Assignment:** Instructors are assigned courses by Admins.
- **Material Management:** Upload lecture materials (videos, PDFs) and assignments.
- **Grading and Feedback:** Mark student assignments and provide feedback.
- **Course Monitoring:** Track student progress and performance in their assigned courses.

### Student
- **Course Enrollment:** Enroll in courses, ensuring prerequisites are met.
- **Material Access:** Access course materials uploaded by Instructors.
- **Assignment Submission:** Upload assignment solutions.
- **Feedback and Grades:** View feedback and grades provided by Instructors.
- **Progress Tracking:** Keep track of grades for each assignment and overall GPA.
- **Profile Management:** Edit personal information, including the ability to change passwords and recover forgotten passwords.

### User Management
- **Registration and Login:** Users can register and log in to the system.
- **Profile Management:** Each user has a profile page where they can update their information.
- **Password Recovery:** Mechanism for users to recover forgotten passwords.

### Course Prerequisites
- **Course Hierarchy:** Courses are structured with prerequisites, ensuring that students cannot enroll in advanced courses without completing the necessary preliminary courses.

## Technologies Used

This project is built using the following technologies and tools:

- **.NET Core MVC:** For building the web application.
- **Entity Framework (EF) Core:** For database operations and ORM.
- **LINQ:** For querying the database.
- **HTML, CSS, Bootstrap and JavaScript:** For front-end development.



**Note:** This is a practice project and not intended for production use. The main goal is to learn and practice using the mentioned technologies.
