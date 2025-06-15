PERSONAL ERP is developed using following tech stack

- .NET 
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (SSMS)
- Visual Studio
- Postman
- Swagger

<!-- .NET -->
![.NET](https://img.shields.io/badge/.NET-6.0-blueviolet?logo=dotnet&logoColor=white)

<!-- Visual Studio -->
![Visual Studio](https://img.shields.io/badge/IDE-Visual%20Studio-5C2D91?logo=visual-studio&logoColor=white)

<!-- Postman -->
![Postman](https://img.shields.io/badge/API-Tested%20with-Postman-FF6C37?logo=postman&logoColor=white)

<!-- Swagger -->
![Swagger](https://img.shields.io/badge/API%20Docs-Swagger-brightgreen?logo=swagger&logoColor=white)

<!-- SSMS (Custom badge as it's not available on shields.io) -->
![SSMS](https://img.shields.io/badge/Database-SSMS-4479A1?logo=microsoftsqlserver&logoColor=white)

_🎨 ArtPiece API_

📍 **GET /api/ArtPiece**
Fetches a list of all available Art Pieces.
Returns: Id, Name, Description, and Price.

📍 **GET /api/ArtPiece/{id}**
Fetches a specific Art Piece by its id.

📍 **POST /api/ArtPiece**
Creates a new Art Piece.
Required: Name, Description, and Price.

**📍 PUT /api/ArtPiece/{id}**
Updates an existing Art Piece.

**📍 DELETE /api/ArtPiece/{id}**
Deletes the Art Piece with the given ID.

_🛒 CraftsOrder API_
**📍 GET /api/CraftsOrder**
Returns all orders, including:
Order Id, ArtName, OrderRef, Price, ArtId, CustomerId, CustomerName

**📍 GET /api/CraftsOrder/{id}**
Returns a specific order by id.

**📍 POST /api/CraftsOrder**
Creates a new Crafts Order.

_Customer logic:_
If the phone number does not exist, a new customer is created.
Required: Customer Info + Initial Credit Limit.
If the phone number already exists, only the phone number is required.

_Art-related:_
Requires: ArtId, InitialPayment (can be 0).

_System also:_
Creates a record in BillPaymentCredit.
Updates ArtPiece with new CraftsOrderId.
Initializes credit tracking via Customer and PayingOffCredit.

_👤 Customer_
Each customer has the following tracked:
  InitialCreditLimit
  TotalBillAmount
  TotalBillPaid
  TotalBillPayable
  CurrentCreditLimit

Example Impact of Orders:
  New Order increases TotalBillAmount, decreases CurrentCreditLimit.
  Payment increases TotalBillPaid, adjusts CurrentCreditLimit and TotalBillPayable.

_💳 BillPaymentCredit_
Created when a new order is placed.
Records:
  InitialPayment, TotalBillAmount, and the status of payment (partial, full).
  If InitialPayment = 0, customer must pay later based on their credit limit.
  Optional field CreditLimitUsed is kept nullable.
Business logic ensures:
  When customer pays more than one bill, the older pending bills get cleared first.

_🧾 PayingOffCredit_
  This tracks every customer payment activity.
  
**📍 POST /api/PayingOffCredit**
Required:
  BPId (BillPayment Id),
  CustomerId,
  PaymentMethod (e.g., Cash = 1),
  BankId

Business Rules:
  Validates that the given BPId actually belongs to the provided CustomerId.
  Reflects the payment in:
  BillPaymentCredit → adjusts paid and remaining amounts.
  Customer → updates TotalPaid, Remaining, and Current Credit Limit.
  Sets ModifiedBy accordingly.

🧠 Smart Handling:
  If the paid amount exceeds one bill, it automatically applies to multiple BillPaymentCredits starting from the oldest.
  If nothing is left to pay, a message is shown.
  If some excess remains after all dues are cleared, the response shows the remaining amount.
  PayingOffCredit has been successfully integrated with initial and later payments, but BankId and PaymentMethod handling will be refined.




