# Cohort 4.2 Capstone: Terraform Minds 

## This project was created in collaboration with:
- Danish Habib
- Irwin Singh

## For Instructors
- Trello Link: https://trello.com/b/x08zyIKF/techcareers-capstone

## Problem Statement
To build a web-based learning management system that provides an online course learning and tutoring service for students ranging from Kindergarten to Grade 12 in different subjects.

## Problem Solution
We have utilized C#, Entity Framework, ASP.NET, and MVC in order to create our learning management system.

## Features
- Sign-in and Register
- Course List
  - Enroll in courses
- Administrator Dashboard
  - Create/Update/Delete Courses
  - View Instructors and Students

- Instructor Dashboard
  - View assigned courses
  - View and mark enrolled student assignments
  - Create Assignments
  
- Student Dashboard
  - View enrolled courses
  - Attempt assignments

# Installation Instruction
Please navigate into the project folder

Once in the project folder please follow these steps:

Step 1 - Creating the database
- dotnet ef update database

Step 2 - Database data population:
- Please locate the .sql file and import it into your databasing program of choice

Step 3 - Please install the following packages:
- dotnet add package Microsoft.EntityFrameworkCore.Design
- dotnet add package Pomelo.EntityFrameworkCore.MySql
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# How To Use
The application has 3 primary roles
- Administrator
- Instrucor
- Student

Depending on your role there are varying instructions on how to use the app.
Please navigate to the role you would like to learn about.

For 'How to Use' Home Page, please navigate to the 'Home Page' section.

### Home Page
On the Home Page

### Role - Administrator
To sign in as an Administrator, please navigate to the Sign-in page
and use the following credentials to login.

Email: Techc@ualberta.ca 
Password: Tech

Once logged in, you will now have access to the Administrator Dashboard where you can do the following:
- View Courses
  - Edit a course
  - Delete a course
- View Instructors
- View Students

### Role - Instructor
To become an instructor, please sign into your instructor account. 
If you are new to the website you can create an instructor account from the Registration page.

Once logged in, you will now have access to the Instructor Dashboard where you can do the following:
- View assigned courses
- View and mark assignments for each enrolled student
- Create an assignment

### Role - Student
To become an Student, please sign into your student account. 
If you are new to the website you can create an student account from the Registration page.

once logged in, you will now have access to the Student Dashboard where you can do the following:
- View enrolled courses
- View assignments
- Attempt an assignment

## Test Cases
Please view the following document for Application Test Cases
Test Case: LINK

# Citations
<details>
<summary></summary>
During the lifetime of this project we consulted many online resources. Their list is as follows.
Password Hash and Salt
https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-5.0
https://crackstation.net/hashing-security.htm
https://auth0.com/blog/adding-salt-to-hashing-a-better-way-to-store-passwords/
For Client Side input fields Validation:
https://www.blinkingcaret.com/2016/03/23/manually-use-mvc-client-side-validation/
Regex Expression:
https://regexr.com/3e48o
Login Authentication :
https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-core-mvc-using-cookie/
https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-5.0
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2#create-an-authentication-cookie-1

Match Email regex :
https://www.dotnetperls.com/regex
Bootstrap: 
https://mdbootstrap.com/docs/jquery/navigation/navbar/
https://getbootstrap.com/docs/5.0/getting-started/introduction/
Button style:
https://bootstrapbay.com/blog/bootstrap-button-styles/
Side bar:
https://www.codeply.com/go/Rgq96HykJ2/sidebar-that-changes-to-navbar
Photo Collage Course Page:
https://pixabay.com/photos/art-chalk-child-childhood-color-3509511/
https://pixabay.com/photos/colour-color-colorful-pencil-316776/
https://unsplash.com/photos/IOzk8YKDhYg
Photo Collage Home page inspired by:
https://tedharrison.ca/
WebAIM contrast checker:
https://webaim.org/resources/contrastchecker/
Images compressed using:
https://tinypng.com/
CSS W3C validator:
https://jigsaw.w3.org/css-validator/
Wireframes done using:
https://www.figma.com/
ERD Drawn using: Drawio
Time Tracking in : Google sheets
</details>


