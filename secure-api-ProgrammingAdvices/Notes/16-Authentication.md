# 🪪 Authentication Fundamentals: Who Is Calling Your API?

After securing the "tunnel" (HTTPS) and the "entrance" (CORS), we now tackle the most critical question: Identity.

---

### 🔹 What Is Authentication? (The Identity Check)

Authentication is the process of answering one specific question: **Who are you?**

- It is NOT about permissions (that is Authorization).
- It is purely about **Identity Verification**.

---

### 🔹 Why Your Student API Needs It

Currently, your `POST`, `PUT`, and `DELETE` endpoints are wide open.

- ❌ Anyone can modify student data.
- ❌ The API trusts every incoming request blindly.
- ✅ Authentication allows the API to identify the caller before making any decisions.

---

### 🔹 The Approach: Stateless Authentication

Since REST APIs are stateless, the server does not "remember" you.

- **The Concept:** Every single request must prove its identity again.
- **The Tool:** We use JWT (JSON Web Tokens).
- **The Flow:** 1. Login (Username + Password) ➡️ 2. API issues a JWT ➡️ 3. Client sends JWT in every subsequent request.

[Image of authentication flow diagram]

---

### 🔹 Authentication vs. Authorization

It is vital to distinguish between these two layers:

- **Authentication (Who):** Like a security guard checking your ID card at the gate.
- **Authorization (What):** Like a manager deciding which specific offices you are allowed to enter once you are inside.

---

### 🧬 Key Characteristics

- **Stateless by Design:** The server doesn't store session data; the token carries the identity.
- **Foundational:** You cannot have Authorization (permissions) without first having Authentication (identity).
- **New Endpoints:** Implementation usually requires new paths like `/api/Auth/Register` and `/api/Auth/Login`.

---

### 🛠️ The Security Interconnection

Security is a layered cake, not a single feature:

1. **HTTPS:** Protects credentials during the login process.
2. **CORS:** Ensures only your trusted frontend can prompt the login.
3. **Authentication:** Proves the identity of the person logged in.
4. **Authorization:** Enforces rules based on that proven identity.

---

✅ **Conclusion:**
The Mental Model is ready. We know who we are looking for.
👉 **Next Lesson: JWT Deep Dive — What is inside the token?**
