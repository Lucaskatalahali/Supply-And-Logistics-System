# 🚚 Supply and Logistics Management System

![Academic Project](https://img.shields.io/badge/Academic_Project-Sakarya_University-blue)

![Course](https://img.shields.io/badge/Course-Object--Oriented_Analysis_and_Design-blue)

> 🎓 Part of my **[Computer Engineering Academic Portfolio](https://github.com/Lucaskatalahali/computer-engineering-projects)**.

This project is an ASP.NET Core MVC web application that simulates a supply and logistics management system, covering product management, inventory control, order processing, shipping, and role-based access control.

The project focuses on applying object-oriented design principles through the implementation of multiple **design patterns**, **SOLID principles**, and **Entity Framework Core** with **PostgreSQL**.

---

## 🛠 Technologies

- C#
- ASP.NET Core MVC
- Entity Framework Core
- PostgreSQL

---

## ✨ Features

- Product management (CRUD)
- Inventory management
- Shopping cart
- Order management
- Order lifecycle management
- Multiple shipping carriers
- Role-based access control
- Automatic low-stock notifications
- Logging system

---

## 🎨 Design Patterns

The project applies several classic object-oriented design patterns:

| Pattern | Purpose |
|---------|---------|
| **State** | Manages the order lifecycle without complex conditional logic |
| **Strategy** | Supports interchangeable payment methods |
| **Adapter** | Integrates different shipping carrier APIs through a common interface |
| **Factory Method** | Creates shipping carriers while reducing coupling |
| **Decorator** | Adds optional shipping services dynamically |
| **Composite** | Represents products composed of multiple products |
| **Observer** | Automatically notifies the system when product stock falls below a predefined threshold |
| **Singleton** | Maintains a single logging instance throughout the application |

---

## 👥 User Roles

| Role | Permissions |
|---|---|
| **Admin** | Manage products, approve orders, manage users |
| **WarehouseStaff** | Prepare orders, manage stock, view notifications |
| **Courier** | Mark orders as delivered or return defective items |
| **Customer** | Browse products, place orders, cancel orders |

---

## 📦 Order Workflow

```
Pending
    ↓
Confirmed
    ↓
In Preparation
    ↓
In Transit
    ↓
Delivered
```

The complete order lifecycle is implemented using the **State Pattern**, allowing each order state to encapsulate its own behavior and transition rules.

---

## 📄 Documentation

The `docs` folder contains:

- Original Project Specification
- Project Reports
- Design Report
- Additional academic documentation

---

## 📸 Screenshots

### Home Page

![Home Page](Supply%20And%20Logistics%20System/assets/home.png)

### Products

![Products](Supply%20And%20Logistics%20System/assets/products.png)

### Orders

![Orders](Supply%20And%20Logistics%20System/assets/orders.png)

### Inventory

![Inventory](Supply%20And%20Logistics%20System/assets/stock.png)

---

## ▶️ Getting Started

### Prerequisites

- .NET 8 SDK
- PostgreSQL
- Visual Studio 2022 (recommended)

### Setup

1. Clone this repository.

2. Create a PostgreSQL database named `SupplyDB`.

3. Update the PostgreSQL connection string in `appsettings.json` with your PostgreSQL username and password.

4. Run the application from Visual Studio or execute:

```bash
dotnet run
```

On the first startup, Entity Framework Core automatically applies all database migrations and seeds the database with sample users and products.

## 🔑 Default Accounts

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@system.com | admin123 |
| Warehouse Staff | warehouse@system.com | warehouse123 |
| Courier (Aras) | courier.aras@system.com | courier123 |
| Courier (Yurtici) | courier.yurtici@system.com | courier123 |
| Courier (GlobalExpress) | courier.global@system.com | courier123 |
| Customer | lucas@system.com | lucas123 |

---

## 🎓 Academic Information

- **University:** Sakarya University
- **Department:** Computer Engineering
- **Course:** Object-Oriented Analysis and Design
- **Academic Year:** 2025–2026
- **Project Grade:** 87/100

---

## 📌 Notes

This repository preserves the original academic project exactly as it was submitted and evaluated.

The primary objective of this project was to apply object-oriented analysis and design principles by developing a maintainable enterprise application using modern architectural practices, design patterns, and clean software design.

For more academic projects, visit my **[Computer Engineering Academic Portfolio](https://github.com/Lucaskatalahali/computer-engineering-projects)**.
