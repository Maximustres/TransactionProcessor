# API de Procesamiento de Transacciones

Este proyecto es una API RESTful desarrollada en **.NET Core 8** para procesar transacciones simuladas. Implementa un flujo completo de creación, validación, autorización (mock), almacenamiento y consulta de detalles de transacciones.

## 🧩 Características

- Arquitectura limpia con separación por capas (Controllers, Services, Repositories)
- Middleware personalizado para manejo de excepciones
- Validación de entradas con **FluentValidation**
- Comunicación con adquirente simulado (Mock)
- Persistencia en base de datos **PostgreSQL**
- Reintentos automáticos usando **Polly**
- Documentación de endpoints con **Swagger**
- Mapeo entre entidades y DTOs con **AutoMapper**
- Migraciones con CLI de `.NET` para EF Core

---

## 🚀 Tecnologías y Paquetes Usados

- [.NET Core 8](https://dotnet.microsoft.com/en-us/)
- Entity Framework Core
- PostgreSQL
- AutoMapper
- FluentValidation
- Polly
- Swagger (Swashbuckle)
- ILogger
- Middleware personalizado

---

## ⚙️ Configuración del Proyecto

### 1. Clona el repositorio

```bash
git clone https://github.com/Maximustres/api-transaction.git
cd api-transaction
```

### 2. Configura la base de datos

Asegúrate de tener PostgreSQL instalado y ejecutando. Crea una base de datos y actualiza el `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=transaction_db;Username=postgres;Password=tu_password"
}
```

### 3. Ejecuta las migraciones de EF Core

```bash
dotnet ef database update
```

> Si no tienes las herramientas de EF instaladas:
>
```bash
dotnet tool install --global dotnet-ef
```

---

## ▶️ Ejecución

```bash
dotnet run
```

La API estará disponible en:

```
https://localhost:5001
```

Swagger UI:

```
https://localhost:5001/swagger
```

---

## 📌 Endpoints Principales

| Método | Ruta                              | Descripción                              |
|--------|-----------------------------------|------------------------------------------|
| POST   | `/api/transactions`               | Procesa una nueva transacción            |
| GET    | `/api/transactions/{id}/details` | Consulta el historial de una transacción |

---

## 🧪 Ejemplo de Request (POST)

```http
POST /api/transactions
Content-Type: application/json

{
  "pan": "4111111111111111",
  "expiry": "12/25",
  "amount": 15000,
  "currency": "CLP",
  "cvv": "123",
  "merchantId": "M123"
}
```

### Respuesta esperada:

```json
{
  "status": "Approved",
  "code": "00"
}
```

---

## 📁 Estructura del Proyecto

```
api_transaction/
│
├── Controllers/
│   └── TransactionsController.cs
├── DTOs/
├── Entities/
├── Interfaces/
├── Repositories/
├── Service/
├── Middleware/
├── Persistence/
│   └── ApplicationDbContext.cs
├── Validators/
│   └── TransactionRequestValidator.cs
├── Program.cs
├── MappingProfile.cs
```

---

## 🔄 Comportamiento Interno

- Al recibir una transacción, se valida y se guarda con estado `Pending`.
- Se intenta autorizar la transacción contra un adquirente simulado (`MockIso8583Acquirer`).
- Se aplican **hasta 3 reintentos** en caso de timeout.
- Se registran todos los cambios de estado en `TransactionDetails`.
- Se puede consultar el historial de cada transacción por su `ID`.

---

## ✍️ Autor

Luis Bustos  
Desarrollador .NET Core

---

## 📝 Notas adicionales

- Este proyecto puede servir como base para integraciones reales con Transbank, Webpay u otros adquirentes.
- Incluye una arquitectura escalable y buenas prácticas para microservicios .NET.
