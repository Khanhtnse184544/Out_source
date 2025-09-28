# API Documentation - Android Game Backend

## 📋 Project Overview

**Project Name**: Backend Android Game  
**Framework**: .NET 8.0 + Supabase  
**Database**: PostgreSQL (Supabase)  
**Payment Gateway**: PayOS  
**Deployment**: Microsoft Azure  

## 🎯 MVP Requirements

1. **Payment Integration**: Direct payment in-game via PayOS
2. **Google Play Store**: Deploy game to Google Play Store
3. **Data Synchronization**: Save game data to database

---

## 🔐 Authentication APIs

### 1. User Registration
```http
POST /api/user/register
```

**Request Body:**
```json
{
  "email": "string",
  "password": "string",
  "username": "string"
}
```

**Response:**
```json
{
  "status": "success",
  "message": "RegisterSuccess",
  "data": {
    "userId": "string",
    "email": "string",
    "username": "string",
    "level": 0,
    "coin": 10,
    "memberTypeId": "M1",
    "status": "active"
  }
}
```

### 2. User Login
```http
POST /api/user/login
```

**Request Body:**
```json
{
  "email": "string",
  "password": "string"
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Login successful",
  "data": {
    "userId": "string",
    "email": "string",
    "username": "string",
    "level": 0,
    "coin": 100,
    "expPerLevel": 50,
    "memberTypeId": "M1",
    "status": "active"
  }
}
```

---

## 💰 Payment APIs

### 3. Create Payment Order
```http
POST /api/payment/create-order
```

**Request Body:**
```json
{
  "userId": "string",
  "amount": 10000,
  "description": "Purchase 100 coins",
  "items": [
    {
      "name": "Coins",
      "quantity": 1,
      "price": 10000
    }
  ]
}
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "orderId": "string",
    "paymentUrl": "https://payos.vn/pay/...",
    "amount": 10000,
    "status": "pending"
  }
}
```

### 4. Payment Callback
```http
POST /api/payment/callback
```

**Request Body:**
```json
{
  "code": "00",
  "desc": "success",
  "data": {
    "orderCode": "string",
    "amount": 10000,
    "description": "Purchase 100 coins",
    "accountNumber": "string",
    "reference": "string",
    "transactionDateTime": "2024-01-01T00:00:00Z",
    "currency": "VND",
    "paymentLinkId": "string",
    "code": "00",
    "desc": "success",
    "signature": "string"
  },
  "signature": "string"
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Payment processed successfully"
}
```

### 5. Verify Payment
```http
GET /api/payment/verify/{orderId}
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "orderId": "string",
    "status": "completed",
    "amount": 10000,
    "transactionDateTime": "2024-01-01T00:00:00Z"
  }
}
```

---

## 🌱 Game Data APIs

### 6. Save Planted Trees
```http
POST /api/game/save-planted-trees
```

**Request Body:**
```json
{
  "userId": "string",
  "trees": [
    {
      "itemId": "TA1",
      "status": "planted"
    }
  ]
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Trees saved successfully",
  "data": {
    "savedCount": 1
  }
}
```

### 7. Get Planted Trees
```http
GET /api/game/planted-trees/{userId}
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "userId": "string",
    "trees": [
      {
        "itemId": "TA1",
        "status": "planted"
      }
    ]
  }
}
```

### 8. Save Harvest Log
```http
POST /api/game/save-harvest
```

**Request Body:**
```json
{
  "userId": "string",
  "harvests": [
    {
      "itemId": "TA1",
      "status": "harvested"
    }
  ]
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Harvest saved successfully",
  "data": {
    "savedCount": 1
  }
}
```

### 9. Get Harvest Log
```http
GET /api/game/harvest-log/{userId}
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "userId": "string",
    "harvests": [
      {
        "itemId": "TA1",
        "status": "harvested"
      }
    ]
  }
}
```

### 10. Save Inventory
```http
POST /api/game/save-inventory
```

**Request Body:**
```json
{
  "userId": "string",
  "items": [
    {
      "itemId": "TA1",
      "quantity": 5
    }
  ]
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Inventory saved successfully",
  "data": {
    "savedCount": 1
  }
}
```

### 11. Get Inventory
```http
GET /api/game/inventory/{userId}
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "userId": "string",
    "items": [
      {
        "itemId": "TA1",
        "quantity": 5
      }
    ]
  }
}
```

### 12. Update User Stats
```http
PUT /api/game/update-stats
```

**Request Body:**
```json
{
  "userId": "string",
  "coin": 150,
  "expPerLevel": 75,
  "level": 2
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Stats updated successfully",
  "data": {
    "userId": "string",
    "coin": 150,
    "expPerLevel": 75,
    "level": 2
  }
}
```

### 13. Get User Stats
```http
GET /api/game/stats/{userId}
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "userId": "string",
    "coin": 150,
    "expPerLevel": 75,
    "level": 2,
    "memberTypeId": "M1",
    "status": "active"
  }
}
```

---

## 🎮 Scene Management APIs

### 14. Save Scene Data
```http
POST /api/game/save-scene
```

**Request Body:**
```json
{
  "userId": "string",
  "sceneData": {
    "status": "active",
    "dateSave": "2024-01-01T00:00:00Z"
  }
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Scene saved successfully"
}
```

### 15. Get Scene Data
```http
GET /api/game/scene/{userId}
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "userId": "string",
    "sceneData": {
      "status": "active",
      "dateSave": "2024-01-01T00:00:00Z"
    }
  }
}
```

### 16. Save Scene Details
```http
POST /api/game/save-scene-details
```

**Request Body:**
```json
{
  "userId": "string",
  "sceneDetails": [
    {
      "itemId": "TA1",
      "name": "Apple Tree"
    }
  ]
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Scene details saved successfully"
}
```

### 17. Get Scene Details
```http
GET /api/game/scene-details/{userId}
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "userId": "string",
    "sceneDetails": [
      {
        "itemId": "TA1",
        "name": "Apple Tree"
      }
    ]
  }
}
```

---

## 📊 Category & Items APIs

### 18. Get Categories
```http
GET /api/categories
```

**Response:**
```json
{
  "status": "success",
  "data": [
    {
      "itemId": "TA1",
      "name": "Apple Tree",
      "type": "Tree",
      "price": 0,
      "status": "Active"
    }
  ]
}
```

### 19. Get Items by Type
```http
GET /api/items/type/{type}
```

**Response:**
```json
{
  "status": "success",
  "data": [
    {
      "itemId": "TA1",
      "name": "Apple Tree",
      "type": "Tree",
      "price": 0,
      "status": "Active"
    }
  ]
}
```

### 20. Save Category Details
```http
POST /api/categories/save-details
```

**Request Body:**
```json
{
  "userId": "string",
  "itemId": "TA1",
  "quantity": 5
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Category details saved successfully"
}
```

---

## 🔄 Data Synchronization APIs

### 21. Full Game Sync
```http
POST /api/game/sync
```

**Request Body:**
```json
{
  "userId": "string",
  "gameData": {
    "userStats": {
      "coin": 150,
      "expPerLevel": 75,
      "level": 2
    },
    "inventory": [
      {
        "itemId": "TA1",
        "quantity": 5
      }
    ],
    "plantedTrees": [
      {
        "itemId": "TA1",
        "status": "planted"
      }
    ],
    "harvestLog": [
      {
        "itemId": "TA1",
        "status": "harvested"
      }
    ],
    "sceneData": {
      "status": "active",
      "dateSave": "2024-01-01T00:00:00Z"
    }
  }
}
```

**Response:**
```json
{
  "status": "success",
  "message": "Game data synchronized successfully",
  "data": {
    "syncTimestamp": "2024-01-01T00:00:00Z",
    "syncedSections": ["userStats", "inventory", "plantedTrees", "harvestLog", "sceneData"]
  }
}
```

### 22. Get Full Game Data
```http
GET /api/game/full-data/{userId}
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "userId": "string",
    "userStats": {
      "coin": 150,
      "expPerLevel": 75,
      "level": 2,
      "memberTypeId": "M1",
      "status": "active"
    },
    "inventory": [
      {
        "itemId": "TA1",
        "quantity": 5
      }
    ],
    "plantedTrees": [
      {
        "itemId": "TA1",
        "status": "planted"
      }
    ],
    "harvestLog": [
      {
        "itemId": "TA1",
        "status": "harvested"
      }
    ],
    "sceneData": {
      "status": "active",
      "dateSave": "2024-01-01T00:00:00Z"
    }
  }
}
```

---

## 🚨 Error Responses

### Standard Error Format
```json
{
  "status": "error",
  "message": "Error description",
  "errorCode": "ERROR_CODE",
  "details": "Additional error details"
}
```

### Common Error Codes
- `USER_NOT_FOUND`: User does not exist
- `INVALID_CREDENTIALS`: Invalid email or password
- `PAYMENT_FAILED`: Payment processing failed
- `INVALID_ORDER`: Order not found or invalid
- `SYNC_FAILED`: Data synchronization failed
- `VALIDATION_ERROR`: Request validation failed

---

## 🔧 Technical Specifications

### Base URL
```
Development: https://localhost:7000/api
Production: https://your-azure-app.azurewebsites.net/api
```

### Authentication
- JWT Token in Authorization header
- Format: `Bearer {token}`

### Content-Type
- Request: `application/json`
- Response: `application/json`

### Rate Limiting
- 100 requests per minute per user
- 1000 requests per hour per IP

### CORS
- Allowed Origins: Unity Game Client
- Allowed Methods: GET, POST, PUT, DELETE
- Allowed Headers: Content-Type, Authorization

---

## 📱 Unity Integration Notes

### HTTP Client Setup
```csharp
// Unity HTTP Client Example
using UnityEngine;
using System.Collections;
using System.Text;

public class GameAPIClient : MonoBehaviour
{
    private string baseURL = "https://your-api-url.com/api";
    
    public IEnumerator SaveGameData(string userId, GameData data)
    {
        string json = JsonUtility.ToJson(data);
        byte[] body = Encoding.UTF8.GetBytes(json);
        
        using (UnityEngine.Networking.UnityWebRequest request = 
               new UnityEngine.Networking.UnityWebRequest(baseURL + "/game/sync", "POST"))
        {
            request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(body);
            request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log("Data saved successfully");
            }
        }
    }
}
```

---

*Last Updated: January 2024*  
*Version: 1.0*  
*Framework: .NET 8.0*
