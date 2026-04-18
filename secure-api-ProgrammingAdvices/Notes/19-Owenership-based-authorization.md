# 🧭 Ownership-Based Authorization: The "Data Boundary" Layer

### 1️⃣ Introduction: Why Roles Are Not Enough?
Up to this point, our API is secure at the "Gate" (Authentication) and "Rank" (Roles) levels. However, a major security hole remains:
* **Role-Based (RBAC):** Answers "What can this user do in general?" (e.g., A Student can view profiles).
* **Ownership-Based:** Answers "Is this specific data owned by this user?" (e.g., Can Student A view Student B's profile?).

**The Problem:** Without ownership checks, any authenticated Student could change the ID in the URL (`/api/Students/1` to `/api/Students/999`) and steal data. This is called **Horizontal Privilege Escalation**.

---

### 2️⃣ The Vulnerability: Horizontal Privilege Escalation
This is one of the most common API security failures in the real world (OWASP Top 10 - Broken Object Level Authorization / BOLA).
* **Attacker Profile:** Stays in the same role (e.g., Student).
* **Attack Method:** Modifies the Resource ID to access data belonging to others.
* **Analogy:** Having a key to the apartment building (Role) doesn't mean you have the right to open every neighbor's door (Ownership).

---

### 3️⃣ What is Ownership-Based Authorization?
It is a **Contextual** security check that compares two entities at runtime:
1.  **The Subject:** The authenticated user identity (Extracted from JWT Claims).
2.  **The Resource:** The owner of the data being requested (Extracted from the Database or Route).

**The Rule:**
Access is granted ONLY if: `JWT.UserId == Resource.OwnerId` OR `User.Role == Admin`.

---

### 4️⃣ Ownership vs. Role: Comparison Table

| Feature | Role-Based (RBAC) | Ownership-Based |
| :--- | :--- | :--- |
| **Question** | What is your rank? | Is this yours? |
| **Check Type** | Static (Attributes) | Dynamic (Runtime Logic) |
| **Complexity** | Simple (`[Authorize(Roles="Admin")]`) | Higher (Requires Policy & Handlers) |
| **Scope** | Global (All Students) | Specific (This Student Record) |
| **Prevents** | Vertical Escalation (Student -> Admin) | Horizontal Escalation (Student -> Student) |

---

### 5️⃣ Where It Fits in the Security Roadmap
1. **HTTPS + CORS:** Secure Communication.
2. **Authentication (JWT):** Identity Establishment.
3. **Role-Based Authorization:** Permission Groups.
4. **Ownership-Based Authorization:** 🎯 **Data Boundaries (Current Layer).**
5. **Policies & Hardening:** Advanced Logic & Production Readiness.

---

### 🔗 Interconnection (The Logic Flow)
1. **JWT:** Provides the User Identity (e.g., `sub: 5`).
2. **Route/Body:** Identifies the requested resource (e.g., `id: 10`).
3. **Ownership Logic:** Compares both IDs.
4. **Authorization Middleware:** Issues `200 OK` if they match, or `403 Forbidden` if they don't.

---

### 🛠️ Final Summary
* **Roles protect Actions:** (e.g., Who can delete?).
* **Ownership protects Data:** (e.g., Whose data is being deleted?).
* **Authentication without Ownership is incomplete:** Most real-world breaches happen due to missing ownership checks.
* **403 Forbidden:** Is the standard response when ownership is violated.

✅ **Status:** Theoretical understanding complete. Ready for Module 6 (Implementation of Ownership Policies).