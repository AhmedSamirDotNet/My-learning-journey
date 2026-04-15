# 🛡️ Enforcing HTTPS: All-in-One Summary

Securing the transport layer is mandatory before adding JWT. Without HTTPS, credentials and tokens are sent in plain text and can be stolen.

---

### 1️⃣ Middleware (Program.cs)

Ensure this line is present before mapping controllers:
`app.UseHttpsRedirection();`

> **Rule:** This redirects traffic but doesn't "enable" HTTPS. HTTPS must be running for this to work.

### 2️⃣ Profile Config (launchSettings.json)

Verify the **https** profile exists in `Properties/launchSettings.json`:

`"https": { "commandName": "Project", "launchBrowser": true, "launchUrl": "swagger", "applicationUrl": "https://localhost:7217;http://localhost:5215" }`

- **Critical:** Select the **https** profile from the Visual Studio run toolbar (not http or IIS Express).

### 3️⃣ Trust Certificates (Terminal)

Run these commands once per machine to enable SSL locally:

`dotnet dev-certs https --clean`
`dotnet dev-certs https --trust`

### 4️⃣ Dev vs Production Logic

- **Development:** Visual Studio uses separate profiles. Redirection won't "jump" between profiles; you must start the HTTPS one.
- **Production:** Single environment. HTTP -> HTTPS redirection is automatic and always enforced.

---

### 🧬 The Security Chain

- **HTTPS:** Encrypts the tunnel.
- **JWT:** The key inside the tunnel.
- **Result:** Security is added transparently without changing a single endpoint or controller.

# 🔒 مرجع: إعادة التوجيه الآمن (HTTPS Redirection) في .NET

في تطوير الويب الحديث، يعتبر بروتوكول **HTTPS** هو المعيار الأساسي والوحيد المقبول لتأمين انتقال البيانات بين المستخدم والسيرفر.

### 1. ما هي وظيفة `app.UseHttpsRedirection()`؟

هذا الـ Middleware يعمل كـ "بوابة إجبارية". وظيفته هي التحقق من أي طلب (Request) قادم للسيرفر:

- إذا كان الطلب قادم عبر **HTTP** (غير مشفر - منفذ 80)، يقوم السيرفر بإرسال رد للمتصفح يخبره بأن ينتقل فوراً إلى نفس العنوان ولكن باستخدام **HTTPS** (مشفر - منفذ 443).
- المتصفح يقوم بتنفيذ هذا التوجيه تلقائياً دون تدخل المستخدم.

---

### 2. ماذا يحدث إذا لم تستخدمها؟ (المخاطر)

إذا قمت بإلغاء هذه الميزة وكان موقعك متاحاً عبر HTTP العادي:

1. **التنصت (Sniffing):** أي بيانات يرسلها المستخدم (كلمات مرور، أرقام فيزا، API Keys) تظهر كنص واضح (Plain Text) لأي شخص على الشبكة.
2. **فقدان الثقة:** المتصفحات مثل Chrome ستظهر رسالة **"Not Secure"** بجانب رابط موقعك، مما ينفر المستخدمين.
3. **تحذيرات أمنية:** قد تظهر "صفحة حمراء" تمنع المستخدم من الدخول للموقع خوفاً على بياناته.
4. **تراجع الـ SEO:** محركات البحث (Google) تعطي أولوية أقل للمواقع التي لا تفرض التشفير.

---

### 3. الفرق بين HTTPS والشهادة (SSL/TLS)

- **HTTPS:** هو البروتوكول (طريقة التواصل).
- **SSL/TLS Certificate:** هي "الوثيقة الرقمية" التي تسمح بالتشفير.
- **بدون شهادة:** لن يعمل الـ HTTPS حتى لو وضعت الكود، لأن السيرفر لن يجد "المفتاح" لتشفير البيانات.

---

### 4. إضافات هامة يجب معرفتها (Pro-Level)

#### أ. الـ HSTS (HTTP Strict Transport Security)

بالإضافة للـ Redirection، نستخدم `app.UseHsts()`.

- **فائدتها:** تخبر المتصفح ألا يحاول حتى الاتصال بـ HTTP العادي مستقبلاً، بل يتصل دائماً بـ HTTPS مباشرة من تلقاء نفسه لزيادة الأمان.

#### ب. الشهادات المجانية (Let's Encrypt)

لا تحتاج لشراء شهادة غالية. يمكنك استخدام **Let's Encrypt** للحصول على شهادة مجانية تماماً وتتجدد تلقائياً، وهي مدعومة في أغلب لوحات تحكم الاستضافة.

#### ج. بيئة التطوير (Local Development)

في .NET، عند تشغيل المشروع محلياً، يقوم الـ SDK بإنشاء شهادة مؤقتة (Self-signed) لتجربة الـ HTTPS. يمكنك الوثوق بها عن طريق الأمر:
`dotnet dev-certs https --trust`

---

### 5. ملخص الإعداد في `Program.cs`

```csharp
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // تفعل في الإنتاج فقط لإجبار المتصفح على الأمان الدائم
    app.UseHsts();
}

// إجبار تحويل أي طلب HTTP إلى HTTPS
app.UseHttpsRedirection();

app.MapControllers();
app.Run();
```
