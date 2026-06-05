# Kontrakty zdarzen integracyjnych dla `sklep`

## Wspolna koperta zdarzenia

```json
{
  "eventId": "uuid",
  "eventType": "string",
  "eventVersion": 1,
  "occurredAt": "2026-06-05T19:00:00Z",
  "aggregateId": "string",
  "correlationId": "uuid",
  "payload": {}
}
```

## `ProductCreated`

Publikuje: Catalog Service

Odbiera: Sales Service

```json
{
  "eventType": "ProductCreated",
  "aggregateId": "product-1",
  "payload": {
    "productId": 1,
    "name": "Monstera deliciosa",
    "price": 89.99,
    "imageUrl": "http://catalog-service/productimg/monstera.jpg",
    "stockQuantity": 10,
    "categoryId": 1
  }
}
```

## `ProductUpdated`

Publikuje: Catalog Service

Odbiera: Sales Service

```json
{
  "eventType": "ProductUpdated",
  "aggregateId": "product-1",
  "payload": {
    "productId": 1,
    "name": "Monstera deliciosa XL",
    "price": 109.99,
    "imageUrl": "http://catalog-service/productimg/monstera.jpg",
    "stockQuantity": 8,
    "categoryId": 1
  }
}
```

## `OrderPlaced`

Publikuje: Sales Service

Odbiera: Catalog Service

```json
{
  "eventType": "OrderPlaced",
  "aggregateId": "order-1001",
  "payload": {
    "orderId": 1001,
    "userId": 7,
    "items": [
      {
        "productId": 1,
        "quantity": 2,
        "unitPriceSnapshot": 109.99
      }
    ],
    "totalPrice": 219.98,
    "status": "Pending"
  }
}
```

## `StockReserved`

Publikuje: Catalog Service

Odbiera: Sales Service

```json
{
  "eventType": "StockReserved",
  "aggregateId": "order-1001",
  "payload": {
    "orderId": 1001,
    "reservationId": "res-550e8400-e29b-41d4-a716-446655440000",
    "items": [
      {
        "productId": 1,
        "quantity": 2
      }
    ]
  }
}
```

## `StockReservationRejected`

Publikuje: Catalog Service

Odbiera: Sales Service

```json
{
  "eventType": "StockReservationRejected",
  "aggregateId": "order-1001",
  "payload": {
    "orderId": 1001,
    "reason": "Insufficient stock",
    "items": [
      {
        "productId": 1,
        "requestedQuantity": 2,
        "availableQuantity": 0
      }
    ]
  }
}
```

## `OrderConfirmed`

Publikuje: Sales Service

Odbiera: inne uslugi, np. wysylka lub powiadomienia

```json
{
  "eventType": "OrderConfirmed",
  "aggregateId": "order-1001",
  "payload": {
    "orderId": 1001,
    "userId": 7,
    "status": "Confirmed",
    "totalPrice": 219.98
  }
}
```

