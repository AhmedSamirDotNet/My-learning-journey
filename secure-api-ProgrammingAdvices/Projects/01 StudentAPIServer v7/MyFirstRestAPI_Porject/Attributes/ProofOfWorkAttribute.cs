using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentApi.Attributes
{
    public class ProofOfWorkAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // بنسحب الـ Hash والـ Nonce (الرقم العشوائي) من الـ Headers
            var powHash = context.HttpContext.Request.Headers["X-POW-Hash"].ToString();
            var nonce = context.HttpContext.Request.Headers["X-POW-Nonce"].ToString();

            if (string.IsNullOrEmpty(powHash) || !IsValidPoW(powHash, nonce))
            {
                context.Result = new BadRequestObjectResult("Proof of Work failed. Are you a bot? 🤖");
            }
        }

        private bool IsValidPoW(string hash, string nonce)
        {
            // مثال بسيط: لازم الـ Hash يبدأ بـ "0000"
            // في الحقيقة بنعمل Hash للـ (Email + Nonce) ونتأكد من النتيجة
            return hash.StartsWith("0000");
        }
    }
}
