# HTTPS: The Foundation of Web Security

HTTPS is the absolute starting point for any secure system. Without it, all other security layers (like JWT, Passwords, or Roles) are useless because data can be stolen or modified while traveling.

---

## 1. What is HTTPS?
**HTTPS = HTTP + Security (TLS/SSL)**
It is simply the standard HTTP protocol running inside an encrypted tunnel called **TLS (Transport Layer Security)**.

### The 3 Pillars of HTTPS
1.  **Confidentiality:** Data is encrypted; only the sender and receiver can read it.
2.  **Integrity:** Data cannot be modified during transit without being detected.
3.  **Authentication:** Proves that the server is who it claims to be, preventing "Fake Server" attacks.

---

## 2. The Problem: HTTP is Plain Text
When using standard **HTTP**, data travels like a postcard. Anyone on the network (Public Wi-Fi, ISPs, Routers) can:
* **Read** your password.
* **Modify** the amount of money you are transferring.
* **Steal** your session token.

---

## 3. Certificates & Trust
A **TLS/SSL Certificate** is like a digital ID card for your server.

### What’s Inside a Certificate?
* The Domain Name (e.g., techtober.com).
* The **Public Key** (used to start the encryption).
* The **Issuer (CA)** (Who verified this server).
* Expiration Date & Digital Signature.

### Certificate Authorities (CAs)
Certificates are issued by trusted organizations called CAs (like **Let’s Encrypt**, **DigiCert**, or **Sectigo**). Browsers and Operating Systems come with a pre-installed list of these trusted CAs.

---

## 4. HTTPS in Development vs. Production

| Environment | Certificate Type | How to get it |
| :--- | :--- | :--- |
| **Development** | Self-signed / Local | Created by .NET (`dotnet dev-certs`) |
| **Production** | CA-Signed (Global) | **Let's Encrypt** (Free) or paid providers |

### Important for .NET Developers:
In development, you use `localhost`. Public CAs don't issue certificates for localhost. You must run this command to trust your local dev certificate:
`dotnet dev-certs https --trust`

---

## 5. How the HTTPS Handshake Works
1.  **The Greeting:** Client connects and the Server sends its **Certificate**.
2.  **The Verification:** Client checks if the certificate is valid and issued by a trusted CA.
3.  **The Negotiation:** Both sides agree on a **Shared Secret Key** without actually sending the key over the wire.
4.  **Secure Tunnel:** All communication (headers, body, URLs) is now encrypted using that secret key.



---

## 6. Comparison: HTTP vs. HTTPS

| Feature | HTTP | HTTPS |
| :--- | :--- | :--- |
| **Data Format** | Plain Text | Encrypted Ciphertext |
| **Port** | 80 | 443 |
| **Security** | None | High |
| **Search Engine Rank** | Neutral | Improved (SEO Boost) |

---

## 7. Critical Truth: HTTPS is Not Enough
HTTPS only protects **Data in Transit** (while it is moving). It does **NOT**:
* Authenticate users (You still need JWT/Cookies).
* Protect your Database (Data at rest).
* Stop logic attacks or bad code.

### Summary of Interconnections
* **HTTPS** secures the "Pipe" (Transport).
* **Authentication** proves "Who" is using the pipe.
* **Authorization** controls "What" they can do inside.

---

**File Name:** `https-fundamentals.md`