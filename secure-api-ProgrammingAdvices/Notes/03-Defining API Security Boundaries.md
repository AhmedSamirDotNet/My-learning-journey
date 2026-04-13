# Security Design: Defining API Security Boundaries

## 1. Introduction
Defining security boundaries is a critical design phase that precedes coding. It involves making strategic decisions about access levels for every endpoint in the API. Without these boundaries, authentication mechanisms like JWT cannot be effectively implemented.

## 2. Core Questions of Security Boundaries
A security boundary must answer three fundamental questions:
1. Who is authorized to access this specific endpoint?
2. Should this endpoint be classified as Public or Protected?
3. What are the consequences of unauthorized access?

## 3. Risk Categorization of Student Endpoints

### High Risk Endpoints (Write Operations)
* **Endpoints:** `POST /api/Students`, `PUT /api/Students/{id}`, `DELETE /api/Students/{id}`
* **Reasoning:** These operations modify, destroy, or corrupt database records and academic history.

### Medium Risk Endpoints (Data Exposure)
* **Endpoints:** `GET /api/Students/{id}`, `GET /api/Students/All`
* **Reasoning:** These endpoints expose personal identities and sensitive student data.

### Low Risk Endpoints (Aggregated Data)
* **Endpoints:** `GET /api/Students/Passed`, `GET /api/Students/AverageGrade`
* **Reasoning:** These return general statistics without exposing individual personal information.

## 4. Access Control Rules (The Authorization Matrix)

### Admin Role Only
* **Access:** Full control over student records.
* **Endpoints:** POST, PUT, DELETE operations.

### Authenticated Student
* **Access:** Restricted view of personal data.
* **Endpoint:** `GET /api/Students/{id}`
* **Constraint:** A student is strictly limited to accessing only their own resource ID (Ownership Policy).

### Public Access
* **Access:** Anonymous viewing of non-sensitive data.
* **Endpoints:** `GET /api/Students/Passed`, `GET /api/Students/AverageGrade`
* **Constraint:** No authentication required.

## 5. Summary of Security Characteristics
* **Proactive Planning:** Security is designed before a single line of code is written.
* **Endpoint Differentiation:** Not all endpoints require the same level of protection.
* **Read-Level Danger:** Read operations can be just as dangerous as write operations if they leak sensitive data.
* **Structured Implementation:** Clear design rules simplify the implementation of JWT, Roles, and Policies.

## 6. Conclusion
The completion of the design phase ensures that:
1. Dangerous endpoints are identified and isolated.
2. Public data remains accessible without overhead.
3. Access roles (Admin vs. Student) are clearly defined.
This framework serves as the foundation for implementing technical security measures.