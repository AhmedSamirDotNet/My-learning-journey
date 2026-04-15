# Salting & Slow Hashing: Defending Against Real Attacks

Hashing alone is not enough to protect passwords in real-world systems. Attackers don't try to "reverse" hashes; they guess passwords, hash them, and compare the results. This lesson explains how to defend against these attacks.

---

## 1. The Real Attack: Offline Password Cracking
If an attacker steals your database, they can perform an **Offline Attack**.
* **How it works:** They guess millions of passwords per second, hash each guess, and compare it to the stolen hashes.
* **The Danger:** There are no rate limits, no account lockouts, and no alerts because the attack is happening on the attacker's own machine.

---

## 2. Why Fast Hashing is Dangerous
Algorithms like **SHA-256**, **SHA-1**, or **MD5** are designed for speed and data integrity, not password storage.
* **Fast Hashing = Fast Cracking:** If an algorithm can generate millions of hashes per second, an attacker can test millions of passwords per second.

---

## 3. What is Salting?
**Salting** is the process of adding a unique, random string of data to a password **before** it is hashed.

### Why Salting Exists
1. **Prevents Rainbow Tables:** Without salts, attackers use "Rainbow Tables" (precomputed lists of millions of hashes for common passwords).
2. **Uniqueness:** If two users have the same password (`Password123`), salting ensures they have **different hashes** in the database.

### The Mental Model: Unique Recipe
Think of Salt as a **Unique Recipe** per user. Even if an attacker knows the recipe (the salt), they must "cook" each password guess separately for every single user. They cannot reuse their work.

> **Note:** Salts are NOT secret. They are stored in the database next to the hash. Their purpose is uniqueness, not secrecy.



---

## 4. What is Slow Hashing?
Slow hashing algorithms are intentionally designed to consume CPU and memory, making them "expensive" to run.

### Why Slowness Matters
* **Normal Hash:** 1 millisecond per guess = 1,000 guesses per second.
* **Slow Hash:** 500 milliseconds per guess = 2 guesses per second.
* **Result:** It turns a brute-force attack that would take days into one that would take centuries.

---

## 5. Recommended Algorithms
For password storage, always use algorithms that support **internal salting** and **adjustable slowness (Work Factor)**:
* **Argon2:** (Modern & Recommended)
* **bcrypt:** (Standard and very secure)
* **PBKDF2:** (Older but still reliable)

---

## 6. The Complete Secure Password Flow
1. **User Sends Password:** The raw password reaches the API.
2. **Generate Salt:** The system creates a unique, random salt.
3. **Slow Hash:** The system combines `Password + Salt` and runs it through a slow algorithm (like BCrypt).
4. **Store:** Both the **Hash** and the **Salt** are stored in the database.
5. **Discard:** The original password is deleted from memory immediately.



---

## 7. Common Questions
* **Doesn't storing the salt make it weak?** No. The salt's job is to stop "bulk" attacks (Rainbow Tables). Even if the salt is known, the attacker still has to brute-force the password one-by-one, which is stopped by **Slow Hashing**.
* **Why not use Encryption?** Encryption is reversible. If the encryption key is leaked, **all** passwords are exposed at once. Hashing is never reversible.

---

## 8. Summary of Defense
* **Hashing:** Prevents original password recovery.
* **Salting:** Prevents precomputed (Rainbow Table) attacks.
* **Slow Hashing:** Prevents large-scale brute-force cracking.

### Final Conclusion
**Passwords are not protected by secrecy — they are protected by time.** If a system can show you your password in plain text, it is fundamentally insecure.