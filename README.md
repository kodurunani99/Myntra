# Myntra Full-Stack Application

A complete e-commerce application inspired by Myntra, built with modern technologies.

## 🏗️ Architecture

- **Backend**: ASP.NET Core Web API (C#)
- **Frontend**: React with TypeScript
- **Database**: SQL Server
- **Authentication**: JWT Tokens
- **Styling**: Tailwind CSS

## 📁 Project Structure

```
Mynte/
├── backend/                 # C# ASP.NET Core Web API
│   ├── MyntraAPI/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── Data/
│   │   └── DTOs/
├── frontend/               # React TypeScript Application
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── services/
│   │   └── types/
└── database/              # SQL Server scripts
    └── scripts/
```

## 🚀 Features

### Backend Features
- User authentication and authorization
- Product management (CRUD operations)
- Category management
- Shopping cart functionality
- Order management
- User profile management
- Search and filtering
- Image upload handling

### Frontend Features
- Responsive design
- User registration and login
- Product browsing and search
- Shopping cart
- Checkout process
- User profile management
- Admin dashboard
- Product management interface

## 🛠️ Prerequisites

- .NET 8.0 SDK
- Node.js 18+ and npm
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or VS Code

## 📦 Installation

### Backend Setup
1. Navigate to the backend directory:
   ```bash
   cd backend/MyntraAPI
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Update connection string in `appsettings.json`

4. Run database migrations:
   ```bash
   dotnet ef database update
   ```

5. Start the API:
   ```bash
   dotnet run
   ```

### Frontend Setup
1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

## 🌐 API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh token

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create product (Admin)
- `PUT /api/products/{id}` - Update product (Admin)
- `DELETE /api/products/{id}` - Delete product (Admin)

### Categories
- `GET /api/categories` - Get all categories
- `POST /api/categories` - Create category (Admin)

### Cart
- `GET /api/cart` - Get user cart
- `POST /api/cart/add` - Add item to cart
- `PUT /api/cart/update` - Update cart item
- `DELETE /api/cart/remove/{id}` - Remove item from cart

### Orders
- `GET /api/orders` - Get user orders
- `POST /api/orders` - Create order
- `GET /api/orders/{id}` - Get order details

## 🔧 Configuration

### Backend Configuration
Update `appsettings.json` with your database connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyntraDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Frontend Configuration
Update API base URL in `src/services/api.ts`:

```typescript
const API_BASE_URL = 'https://localhost:7001/api';
```

## 🧪 Testing

### Backend Tests
```bash
cd backend/MyntraAPI
dotnet test
```

### Frontend Tests
```bash
cd frontend
npm test
```

## 📝 License

This project is for educational purposes.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request 
6.Copilot PR Test - Added from feature branch


