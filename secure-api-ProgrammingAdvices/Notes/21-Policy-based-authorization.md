# 🛡️ Policy-Based Authorization: The Mental Model & Deep Dive

### 💡 التشبيه الواقعي (Mental Model): نادي الـ VIP

تخيل أنك تدير نادياً رياضياً VIP، وهناك طريقتان لإدارة الدخول:

1. **الطريقة القديمة (Roles):** البواب يقف على الباب ويسأل كل داخل: "هل أنت عضو ذهبي؟". إذا قلت نعم، تدخل.
   - **المشكلة:** إذا أردنا إضافة شرط جديد (مثل: يجب أن يكون العمر فوق 18 ودفع اشتراك الشهر)، سيضطر البواب لحفظ قائمة طويلة من الأسئلة المعقدة لكل شخص، مما يؤدي للفوضى والخطأ.

2. **طريقة الـ Policy (اللائحة):** نحن نضع "قاعدة" مكتوبة في لائحة النادي تسمى **(قاعدة دخول صالة الألعاب)**.
   - البواب لا يهمه التفاصيل المعقدة؛ هو فقط يمرر بياناتك للنظام ويسأل: "هل هذا الشخص يستوفي (قاعدة دخول صالة الألعاب)؟".
   - النظام في الخلفية يفحص العمر، الاشتراك، والنوع، ثم يعطي البواب كلمة واحدة: **"نعم"** أو **"لا"**.

---

### 🧠 شرح الـ Core (حل المشكلات والتعميم)

في ASP.NET Core، الـ Policy هي "اسم مستعار" لمجموعة شروط معقدة.

- **المشكلة (Hard-coding):** قديماً كان الكود يمتلئ بـ `[Authorize(Roles = "Admin, Manager")]`. إذا تغيرت المسميات، يجب تعديل كل Controller في المشروع.
- **الحل بالـ Policy:** أنت تكتب `[Authorize(Policy = "AtLeast18")]`. إذا قررت غداً تغيير السن لـ 21، ستعدل في مكان واحد فقط (Registration) دون لمس الـ Controllers.

**المكونات الأساسية:**

- **الـ Requirement:** هو الكائن الذي يحمل "البيانات" أو "الهدف" (مثل: الحد الأدنى للسن هو 18).
- **الـ Handler:** هو "الموظف" الذي يحتوي على الكود البرمجي (Logic) الذي يقرر: هل المستخدم يحقق الـ Requirement أم لا؟

---

### ⚠️ الأخطاء الشائعة والتحذيرات (Critical Truths)

- **ليست حكراً على ASP.NET:** مفهوم **(Policy-based access control)** عالمي وموجود في أنظمة كبرى مثل **AWS IAM**، لكن تطبيق (Requirement/Handler) هو نمط تصميمي قوي جداً في بيئة .NET.
- **فصل المهام (Separation of Concerns):** الـ Handler وُجد ليكون قابلاً للاختبار (**Unit Testable**) ومنفصلاً عن الـ Web API. كتابة المنطق داخل الـ Controller هي "خطيئة" برمجية في المشاريع الكبيرة.

---

### 🔗 الربط بالطبقات السابقة (Interconnection)

تذكر عندما تحدثنا عن الـ **Bitwise Operations**؟
يمكنك الآن إنشاء Policy تسمى `"CanDelete"`، والـ Handler الخاص بها يقوم بعملية الـ **AND (&)** التي تعلمناها سابقاً ليتأكد من أن الـ Permissions المخزنة داخل الـ Token تسمح بهذا الإجراء!

---

### 🧬 ملخص المفاهيم

- **الـ Policy:** هي اللائحة (الدستور).
- **الـ Requirement:** هو نص القانون (الهدف).
- **الـ Handler:** هو القاضي الذي ينفذ القانون (المنطق).

✅ **النتيجة:** نظام أمان مرن، سهل التعديل، ومنظم برمجياً.

# Summary: Part 1 - The Anatomy of Authorization Policies

## 1. Definition

- Policy: A logical name for a set of requirements (The 'What').
- Requirement: A data structure holding the parameters (e.g., MinAge = 18).
- Handler: The actual logic that evaluates the requirement (The 'How').

## 2. Why Policies?

- Decoupling: Controllers don't need to know 'why' a user is authorized.
- Maintenance: Change logic in one place, updates everywhere.
- Flexibility: Supports complex logic (Bitwise checks, Database lookups, Time-based access).

## 3. Framework Context

- This specific implementation (Policy + Handler) is a hallmark of ASP.NET Core's "Claims-Based" identity system.
- Other frameworks (Node/Spring) use similar patterns but with different naming conventions.

## 4. Implementation Mindset

- You don't copy-paste from Docs every time.
- You understand the "Pattern": Define Requirement -> Create Handler -> Register in Program.cs -> Use Attribute.

# 🛡️ Policy-Based Authorization: The Triple Threat (Requirement, Handler, & Registration)

### 💡 التشبيه الواقعي (Mental Model): نظام أمن المطار ✈️

تخيل أنك تصمم نظام فحص أمني للمطار، العملية تنقسم لثلاثة أدوار:

1. **Requirement (الاستمارة):** ورقة مكتوب فيها "يجب أن يكون المسافر حاملاً لجواز سفر ساري". هي مجرد "بيانات" وشروط مسجلة.
2. **Handler (ضابط الجوازات):** هو الشخص الذي يمسك الورقة (Requirement)، ويفحص جواز سفرك الحقيقي، ويقارنه بالشرط. هو من يملك "المنطق" (Logic) ليقول: "تم الفحص بنجاح".
3. **Registration (تعيين الضابط):** هي عملية وضع هذا الضابط في مكانه الصحيح عند البوابة وتدريبه على فهم هذه الاستمارة ليعمل ضمن منظومة المطار.

---

### 🧠 شرح الـ Core (التنفيذ البرمجي)

إليك كيف نطبق هذا النمط برمجياً داخل ASP.NET Core:

#### 1️⃣ الـ Requirement (البيانات)

كلاس بسيط يرث من `IAuthorizationRequirement` ووظيفته فقط "حمل البيانات" التي سنختبر بناءً عليها.

    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinAge { get; }
        public MinimumAgeRequirement(int age) => MinAge = age;
    }

#### 2️⃣ الـ Handler (المنطق)

كلاس يرث من `AuthorizationHandler<T>` حيث `T` هو الـ Requirement. هنا نكتب الـ **Logic** الفعلي.

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        // 1. نجلب البيانات من الـ Claims (التي تعلمناها في الـ JWT)
        var dobClaim = context.User.FindFirst("DateOfBirth")?.Value;

        if (dobClaim == null) return Task.CompletedTask;

        // 2. منطق حساب العمر (مثال مبسط)
        int age = DateTime.Today.Year - DateTime.Parse(dobClaim).Year;

        // 3. التحقق والموافقة
        if (age >= requirement.MinAge)
        {
            context.Succeed(requirement); // الإشارة للنظام بالنجاح
        }

        return Task.CompletedTask;
    }

#### 3️⃣ الـ Registration (الربط في Program.cs)

نقوم بربط اسم الـ Policy بالـ Requirement، ثم نعرف الـ Handler في نظام الـ Dependency Injection.

    // أ. تعريف السياسة والشرط
    builder.Services.AddAuthorization(options => {
        options.AddPolicy("AtLeast18", policy =>
            policy.Requirements.Add(new MinimumAgeRequirement(18)));
    });

    // ب. تسجيل الموظف (Handler) في النظام
    builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

---

### ⚠️ الأخطاء الشائعة والتحذيرات (Critical Truths)

- **نسيان الـ Registration:** أكبر سبب لفشل الـ Policies هو نسيان تسجيل الـ Handler. بدون هذه الخطوة، الـ API لن يعرف من هو "الضابط" المسؤول عن تنفيذ المنطق، ولن يتم استدعاء الكود أبداً.
- **الـ Context.Succeed:** النظام في ASP.NET Core يعمل بمبدأ **Fail-safe**؛ أي أنه يعتبر المستخدم "مرفوضاً" افتراضياً ما لم يتم استدعاء ميثود `Succeed`. إذا نسيت كتابتها، سيحصل المستخدم دائماً على **403 Forbidden**.

---

### 🔗 الربط بالطبقات السابقة (Interconnection)

هل تذكر الـ **Bitwise Permissions**؟
بدلاً من `MinimumAgeRequirement` يمكنك عمل `PermissionRequirement` يحمل "رقم الصلاحية المطلوبة". والـ Handler الخاص به يقوم بالعملية الحسابية:
`(userPerms & requiredPerm) == requiredPerm`
هذا هو "السر" الذي يربط مفاهيم الأساسيات (كلام أبو هدهود) بالهيكلية المتقدمة لـ **ASP.NET Core Architecture**!

---

✅ **Status:** Security layers interconnected. Logic centralized. Ready for Resource-Based Authorization.

# Summary: Part 2 - Technical Implementation Pattern

## 1. The Implementation Workflow

1. Create a Requirement Class (Implements IAuthorizationRequirement).
2. Create a Handler Class (Inherits AuthorizationHandler<TRequirement>).
3. Register the Policy in Program.cs using AddAuthorization.
4. Register the Handler in Services (AddSingleton/Scoped).

## 2. Key Code Snippets (Pattern)

- Requirement: Just a data holder.
- Handler: Uses `context.Succeed(requirement)` to grant access.
- Policy: `options.AddPolicy("Name", p => p.Requirements.Add(...))`.

## 3. Why it's not "Copy-Paste"?

- Once you understand that Requirements = Data and Handlers = Logic, you can build any complex rule (Time-based, Location-based, Bitwise-based) without needing a tutorial.

## 4. Framework Comparison

- While ASP.NET uses this specific 'Handler' pattern, Java Spring Security uses 'Voters' or 'AccessDecisionManager', which follow the exact same logical decoupling.

# 🛡️ Policy-Based Authorization: Part 3 - Professional Usage & Dynamic Policies

### 💡 التشبيه الواقعي (Mental Model): جهاز كشف المعادن 🛡️

بدلاً من أن يقوم موظف الأمن بتفتيش كل حقيبة يدوياً في كل مكتب داخل المبنى (كتابة الكود داخل كل Action):

1. نحن نضع **"جهاز كشف معادن"** عند البوابة الرئيسية أو مدخل كل قسم.
2. أي شخص يريد المرور يجب أن يعبر من خلال الجهاز.
3. الجهاز مبرمج مسبقاً على "قاعدة" معينة (**Policy**).
4. إذا أطلق الجهاز إنذاراً، يُمنع الشخص من الدخول تلقائياً دون تدخل من الموظفين بالداخل.

---

### 🧠 شرح الـ Core (الاستخدام والتحكم)

#### 1️⃣ الاستخدام على مستوى الـ Controller أو الـ Action

نستخدم الـ Attribute الجاهز `[Authorize]` مع تحديد اسم الـ Policy التي قمنا بتسجيلها مسبقاً في الـ `Program.cs`.

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("sensitive-data")]
        [Authorize(Policy = "AtLeast18")] // هنا نطبق "جهاز الفحص"
        public IActionResult GetSensitiveData()
        {
            return Ok("This is secret data for adults only.");
        }
    }

#### 2️⃣ الـ Dynamic Policies (لماذا نحتاجها؟)

في الأنظمة الضخمة، قد يكون لديك 100 صلاحية (مثل `CanDeleteOrder`, `CanEditUser`). بدلاً من إنشاء 100 Policy يدوياً، نستخدم **Dynamic Policy Providers**.

- **الفكرة:** نمرر "باراميتر" للـ Policy يحدد نوع الصلاحية، والـ Handler يقوم بعملية الفحص بشكل ديناميكي بناءً على هذا الباراميتر.

---

### ⚠️ الأخطاء الشائعة والتحذيرات (Critical Truths)

- **الخلط بين 401 و 403 (مهم جداً):**
  - **401 Unauthorized:** تعني أنك لم تقدم "بطاقة هوية" أصلاً (لم تسجل الدخول).
  - **403 Forbidden:** تعني أننا فحصنا بطاقتك وعرفنا من أنت، لكن الـ Policy رفضت دخولك لأنك "لا تملك الصلاحية". الـ Handlers هي المسؤول الأول عن رد الـ **403**.
- **الأداء (Performance):** إياك أن تجعل الـ Handler يذهب لقاعدة البيانات في كل طلب (Request).
  - **الحل:** استغل الـ **JWT Claims**. ضع فيها الأرقام المهمة (مثل رقم الـ Bitwise) عند الـ Login، واجعل الـ Handler يقرأها من التوكن مباشرة لتوفير الوقت والـ Resources.

---

### 🔗 الربط بالطبقات السابقة (The Big Picture)

الآن تكتمل الصورة النهائية للـ **Security Architecture**:

1. **الـ Bitwise (الرقم):** مخزن "مشفراً" داخل الـ JWT Token كـ Claim.
2. **الـ Policy:** تطلب إذن "صلاحية الحذف" (مثلاً رقم 4).
3. **الـ Handler:** يسحب الرقم من التوكن، ويجري عملية الـ **&** (AND) ليرى هل الـ "Bit" الخاص برقم 4 مرفوع (1) أم لا.
4. **الـ Controller:** يظل نظيفاً تماماً `[Authorize(Policy="CanDelete")]` ولا يشغل باله بتعقيدات الرياضيات أو قواعد البيانات.

---

✅ **النتيجة:** كود نظيف، أداء سريع، وحماية احترافية لا تخطئ.

# Summary: Part 3 - Consumption & Professional Patterns

## 1. Application Layer

- Use `[Authorize(Policy = "Name")]` to secure endpoints.
- Can be applied at Controller level (Global for all actions) or Action level.

## 2. Advanced: Requirements with Data

- Requirements can hold parameters:
  `new MinimumAgeRequirement(21)` vs `new MinimumAgeRequirement(18)`.
- This allows reusing the same Handler logic for different Policy thresholds.

## 3. Best Practices

- Keep Handlers "Lean": Avoid heavy DB logic; rely on JWT Claims whenever possible.
- Use Constants: Never hardcode string policy names like "AtLeast18". Use `public const string AtLeast18Policy = "AtLeast18"`.
- Unit Testing: You can test the Handler logic independently of the Web Server.

## 4. The Big Picture

- Request -> Middleware -> Policy Evaluation -> Handler Execution -> Result (Success/Fail) -> Controller Action.
