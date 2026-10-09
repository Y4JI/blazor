# CorePOS - Project Context & AI Agent Guide

Welcome to the **CorePOS Product Dashboard** codebase. This document serves as a comprehensive architectural and navigation guide designed for AI agents and developers to quickly understand, navigate, and contribute to this repository.

---

## 1. Project Overview

- **Application**: CorePOS Product Dashboard
- **Framework**: .NET 8 (ASP.NET Core Blazor Web App)
- **Render Mode**: `InteractiveServer` (full client-side interactivity without full-page reloads)
- **Styling**: Tailwind CSS (loaded via CDN with custom config) supplemented by `wwwroot/app.css`
- **Data Layer**: In-memory singleton service (`ProductService`) simulating a POS back-office catalog

---

## 2. Directory & File Map

```text
blazor/
├── .gitignore                          # Standard .NET build output exclusions
├── context.md                          # AI agent & developer project documentation
├── README.md                           # Repository readme
└── POSProductDashboard/
    ├── POSProductDashboard.csproj       # .NET 8 Web SDK project file
    ├── Program.cs                      # Dependency injection, middleware & routing setup
    ├── appsettings.json                # Runtime configuration
    │
    ├── Models/
    │   └── Products.cs                 # Domain model (Product, Margin calculation, Clone helper)
    │
    ├── Services/
    │   └── ProductService.cs           # Thread-safe in-memory CRUD & metrics store
    │
    ├── Components/
    │   ├── App.razor                   # HTML shell, Tailwind script, font links & InteractiveServer Routes
    │   ├── Routes.razor                # Blazor Router mapping & default layout
    │   ├── _Imports.razor              # Global namespace imports for Razor components
    │   │
    │   ├── Layout/
    │   │   ├── MainLayout.razor        # Two-column layout (sidebar + main content)
    │   │   ├── MainLayout.razor.css    # Scoped layout CSS
    │   │   └── NavMenu.razor           # CorePOS sidebar with brand logo, links & user profile
    │   │
    │   ├── Modals/
    │   │   ├── AddProductModal.razor   # Modal dialog for creating new catalog products
    │   │   ├── EditProductModal.razor  # Modal dialog for modifying existing products
    │   │   └── DeleteProductModal.razor # Danger confirmation modal for deleting products
    │   │
    │   └── Pages/
    │       ├── Home.razor              # Dashboard overview (metrics cards & recent products)
    │       ├── Products.razor          # Product management (live search, filter & modal triggers)
    │       ├── AddProductPage.razor    # Dedicated route showcasing Add Product modal
    │       ├── EditProductPage.razor   # Dedicated route showcasing Edit Product modal
    │       ├── DeleteProductPage.razor # Dedicated route showcasing Delete Product modal
    │       ├── Counter.razor           # Default template counter (optional reference)
    │       ├── Weather.razor           # Default template weather (optional reference)
    │       └── Error.razor             # Global error view
    │
    ├── Properties/
    │   └── launchSettings.json         # Development server profiles (HTTP port 5262)
    │
    └── wwwroot/
        └── app.css                     # Base styling, Inter font declaration, NavLink active overrides
```

---

## 3. Core Architecture & Patterns

### 3.1. State Management & Service Architecture
- **In-Memory Singleton**: `ProductService` is registered as a singleton in [`Program.cs`](file:///c:/Users/MyPC/3RDYR-CS/blazor/POSProductDashboard/Program.cs). All components inject this service to read and update products.
- **Thread Safety**: Read and write operations use an internal `_lock` object.
- **Cloning & Immutability**: `GetAll()` and `GetByUpc()` return cloned instances to prevent accidental mutations before confirmation.
- **Pre-Seeded POS Data**: Seeded with exact items and counts matching design specifications (e.g. Total Companies: `8`, Total Products: `1,284`, Needs Review: `23`).

### 3.2. User Interface & Styling
- **Tailwind CSS**: Injected in [`App.razor`](file:///c:/Users/MyPC/3RDYR-CS/blazor/POSProductDashboard/Components/App.razor) with the `'Inter'` font family extension.
- **Active Navigation**: Handled by Blazor's `NavLink` combined with `.nav-item-link.active` styles in [`wwwroot/app.css`](file:///c:/Users/MyPC/3RDYR-CS/blazor/POSProductDashboard/wwwroot/app.css).
- **Sticky Sidebar**: The sidebar remains sticky at `h-screen` while the main workspace scrolls independently.

### 3.3. Modal Design & Interaction Pattern
- Modals reside in [`Components/Modals/`](file:///c:/Users/MyPC/3RDYR-CS/blazor/POSProductDashboard/Components/Modals/).
- Each modal accepts:
  - `IsOpen` (`bool`): controls visibility and rendering of the blurred backdrop overlay.
  - Event callbacks: `OnCancel`, `OnSave` / `OnUpdate` / `OnDeleteConfirmed`.
- Modals use `@onclick:stopPropagation="true"` on the modal card to ensure clicking outside the card dismisses the modal gracefully.

---

## 4. Route Map

| URL Route | Component | Purpose |
| :--- | :--- | :--- |
| `/` or `/dashboard` | [`Home.razor`](file:///c:/Users/MyPC/3RDYR-CS/blazor/POSProductDashboard/Components/Pages/Home.razor) | Dashboard Overview with 3 KPI metric cards and Recent Products table. |
| `/products` or `/products-page` | [`Products.razor`](file:///c:/Users/MyPC/3RDYR-CS/blazor/POSProductDashboard/Components/Pages/Products.razor) | Complete Product Management catalog with search, filter, and CRUD modals. |
| `/add-product-modal` | [`AddProductPage.razor`](file:///c:/Users/MyPC/3RDYR-CS/blazor/POSProductDashboard/Components/Pages/AddProductPage.razor) | Dedicated preview page with Add Product modal open. |
| `/edit-product-modal` | [`EditProductPage.razor`](file:///c:/Users/MyPC/3RDYR-CS/blazor/POSProductDashboard/Components/Pages/EditProductPage.razor) | Dedicated preview page with Edit Product modal open. |
| `/delete-product-modal` | [`DeleteProductPage.razor`](file:///c:/Users/MyPC/3RDYR-CS/blazor/POSProductDashboard/Components/Pages/DeleteProductPage.razor) | Dedicated preview page with Delete Product modal open. |

---

## 5. Development & Execution Commands

Run commands from the `POSProductDashboard` directory:

```powershell
# Build solution
dotnet build

# Launch local server using HTTP profile (port 5262)
dotnet run --launch-profile http
```

To run without launching a browser window automatically:
```powershell
dotnet run --no-launch-profile --urls "http://localhost:5262"
```

---

## 6. Contribution & Commit Guidelines

- **Commit Message Standard**: All git commits must strictly follow [Conventional Commits](https://www.conventionalcommits.org/):
  - `feat(<scope>): <description>` for new features or pages.
  - `fix(<scope>): <description>` for bug fixes.
  - `style(<scope>): <description>` for CSS and visual adjustments.
  - `docs(<scope>): <description>` for documentation changes.
  - `chore(<scope>): <description>` for maintenance or configuration.
- **C# & Blazor Conventions**:
  - Keep components modular and single-responsibility.
  - Prefer strong typing and null-safety (`Nullable` enabled).
  - Use `async Task` for asynchronous UI event handlers.
