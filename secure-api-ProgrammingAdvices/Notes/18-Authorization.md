# 🛂 ASP.NET Core Authorization & Role-Based Access Control (RBAC)

### 1️⃣ Introduction: Authentication vs. Authorization

- **Authentication (Who are you?):** Proof of identity via JWT.
- **Authorization (What can you do?):** Enforcement of permissions based on roles or policies.
  > **Note:** Authentication always happens before Authorization.

---

### 2️⃣ The Security Boundary Table (Single Source of Truth)

This table defines the security contract for the Student API:

| Endpoint                    | Action       | Access Level      | Reason                               |
| :-------------------------- | :----------- | :---------------- | :----------------------------------- |
| `GET /api/Students/Passed`  | View Passed  | **Public**        | Aggregated data (Non-sensitive)      |
| `GET /api/Students/Average` | View Average | **Public**        | Aggregated data (Non-sensitive)      |
| `GET /api/Students/{id}`    | View Profile | **Owner / Admin** | Personal data (Needs Ownership Rule) |
| `GET /api/Students/All`     | View List    | **Admin Only**    | Sensitive list of all users          |
| `POST /api/Students`        | Create       | **Admin Only**    | System integrity                     |
| `PUT /api/Students/{id}`    | Update       | **Admin Only**    | System integrity                     |
| `DELETE /api/Students/{id}` | Delete       | **Admin Only**    | Destructive operation                |

---

### 3️⃣ Implementation in ASP.NET Core

#### A. Global Controller Protection

Secure the entire controller to block anonymous users by default:
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase { ... }

#### B. Overriding for Public Access

Use `[AllowAnonymous]` for endpoints that don't need a JWT:
[AllowAnonymous]
[HttpGet("Passed")]
public ActionResult GetPassedStudents() { ... }

#### C. Role-Based Restriction (RBAC)

Use `[Authorize(Roles = "Admin")]` to restrict dangerous operations:
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public ActionResult DeleteStudent(int id) { ... }

---

### 4️⃣ Understanding Status Codes (Critical for Debugging)

- **❌ 401 Unauthorized:** \* **Meaning:** "I don't know who you are."
  - **Cause:** Missing token, expired token, or invalid signature.
- **❌ 403 Forbidden:** \* **Meaning:** "I know who you are, but you don't have permission."
  - **Cause:** Token is valid, but the user doesn't have the "Admin" role.
  - **Note:** 403 is a security SUCCESS (the system blocked an unauthorized action).

---

### 5️⃣ The "Ownership" Problem (The Next Level)

Role-based authorization (Admin/Student) is not enough for endpoints like `GET /api/Students/{id}`.

- **The Risk:** A student with a valid "Student" role could theoretically change the ID in the URL to see another student's data (**Horizontal Privilege Escalation**).
- **The Solution:** Ownership-based authorization (Policies), where we check if `Logged-In User ID == Requested Resource ID`.

---

### 🔗 Interconnection (The Full Cycle)

🔗 Interconnection
Login → issues JWT
JWT → carries role claim
Middleware → validates identity
Authorization → checks role
Controller → allows or blocks action

---

### 🧬 Security Roadmap Recap

1. **HTTPS + CORS:** Secure transport and browser access.
2. **JWT Authentication:** Establishing identity.
3. **Role-Based Authorization:** Distinguishing between user types (Admin vs. Student).
4. **Ownership & Policies :** Ensuring users only access their own data.

✅ **Status:** API is now Professionally Authorized. Ready for Ownership Rules (Module 6).
