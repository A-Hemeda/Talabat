# Talabat API Integration

Welcome to the **Talabat API** documentation! This API allows developers to interact with Talabat’s services, including retrieving menu items, placing orders, tracking orders, and managing restaurant data.

---

## Table of Contents
1. [Introduction](#introduction)
2. [Getting Started](#getting-started)
3. [Authentication](#authentication)
4. [Endpoints](#endpoints)
   - [Menu Retrieval](#menu-retrieval)
   - [Place Order](#place-order)
   - [Order Tracking](#order-tracking)
   - [Restaurant Management](#restaurant-management)
5. [Error Codes](#error-codes)
6. [Rate Limits](#rate-limits)
7. [Support](#support)

---

## Introduction

The Talabat API enables seamless integration with the Talabat platform, allowing third-party applications to manage food ordering services efficiently. 

**Key Features:**
- Access restaurant menus and details.
- Place and manage customer orders.
- Track live order statuses.
- Update and manage restaurant information.

---

## Getting Started

To use the Talabat API:
1. **Sign Up:** Register for an API key at the [Talabat Developer Portal](https://developer.talabat.com).
2. **Integrate SDKs:** Download SDKs or use direct API endpoints.
3. **Test Environment:** Use the sandbox environment to test your integration.
4. **Production Access:** Once testing is complete, request production access.

---

## Authentication

The API uses **Bearer Token Authentication**. You need to include your API key in the `Authorization` header of each request.

**Example:**
```http
Authorization: Bearer YOUR_API_KEY
```

---

## Endpoints

### Menu Retrieval
**GET** `/restaurants/{restaurant_id}/menu`

**Description:** Retrieve the menu of a specific restaurant.

**Parameters:**
- `restaurant_id` (path): The ID of the restaurant.

**Response:**
```json
{
  "restaurant_id": "12345",
  "menu": [
    {
      "item_id": "1",
      "name": "Pizza Margherita",
      "price": 8.99
    }
  ]
}
```

---

### Place Order
**POST** `/orders`

**Description:** Place a new order.

**Request Body:**
```json
{
  "restaurant_id": "12345",
  "customer_id": "67890",
  "items": [
    {
      "item_id": "1",
      "quantity": 2
    }
  ],
  "delivery_address": "123 Main Street"
}
```

**Response:**
```json
{
  "order_id": "abcdef123456",
  "status": "confirmed"
}
```

---

### Order Tracking
**GET** `/orders/{order_id}/status`

**Description:** Get the status of a specific order.

**Response:**
```json
{
  "order_id": "abcdef123456",
  "status": "delivered"
}
```

---

### Restaurant Management
**POST** `/restaurants/{restaurant_id}/update`

**Description:** Update restaurant information.

**Request Body:**
```json
{
  "name": "New Restaurant Name",
  "address": "456 Another Street",
  "contact": "9876543210"
}
```

**Response:**
```json
{
  "restaurant_id": "12345",
  "status": "updated"
}
```

---

## Error Codes

| Code | Message                | Description                          |
|------|------------------------|--------------------------------------|
| 401  | Unauthorized           | Invalid or missing API key.         |
| 404  | Not Found              | Resource does not exist.            |
| 429  | Too Many Requests      | Exceeded rate limit.                |
| 500  | Internal Server Error  | An error occurred on the server.    |

---
