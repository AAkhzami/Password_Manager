# Password_Manager
# 🛡️ Password Manager System

A desktop application built using **C#** and **Windows Forms** (WinForms). This project is designed to help users securely store and manage their digital credentials in one centralized local database.

---

## 🎯 Project Purpose 
This project was developed as a practical application of my previous programming knowledge. specifically focusing on:
* **Architectural Design:** Implementing a clean 3-Tier architecture.
* **Security Exploration:** This was my first experimental journey into  **Encryption and Decryption**, exploring how to protect sensitive user data before storing it in a database.

---

## 🔑 Default Credentials (Quick Access)
If you want to test the application immediately without going through the "Create New User" process, you can use the following default account:

* **Username:** `Admin`
* **Password:** `1234`

---

## ✨ Key Features
* **Secure Authentication:** A dedicated login system to ensure only authorized users can access the stored data.
* **Centralized Dashboard:** View all saved accounts, websites, and passwords in a clean, filterable list.
* **Credential Management:** Easily add, update, or delete "Pass Keys" for various services.
* **3-Tier Architecture:** Structured into Data, Business, and Presentation layers for high maintainability.

---

## 📸 User Interface

### 1️⃣ Login Screen
![Login](./screenshots/Login.png)
*Figure 1: Login*

### 2️⃣ Main Dashboard
![Main Page](./screenshots/Main_Page.png)
*Figure 2: Main Page*

### 3️⃣ Add New Pass Key
![Add New Pass Key](./screenshots/AddNewKey.png)
*Figure 3: Add New Pass Key*

---

## 🛠️ Tech Stack
* **Language:** C#
* **Framework:** .NET Framework (WinForms)
* **Database:** SQL Server
* **Security:** Experimental Encryption/Decryption logic.

---

## 🚀 Installation & Setup

1.  **Clone/Download:** Download the project files and extract the archive.
2.  **Database Setup:**
    > **⚠️ IMPORTANT:** The database file is included within the project folder. Please ensure you download and attach it to your **SQL Server Management Studio (SSMS)**
3.  **Connection String:** Update the `ConnectionString` in the `clsDataAccessSettings.cs` file (Data Layer) to match your local SQL Server instance name.
```csharp
// Example:
public static string ConnectionString = "Server=.;Database=DVLD;User Id=YourUser;Password=YourPassword;";
```
4.  **Run:** Open the `.sln` file in **Visual Studio** and press `F5` to build and run.
* **Executable Path:** You can find the ready-to-run application at the following path:  
      `\Password Manager\bin\Debug\Password Manager.exe`
---
**This project represents a significant step in my learning journey, focusing on security and software structure.** 🫡
