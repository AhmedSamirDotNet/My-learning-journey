namespace StudentApi.DTOs.Auth
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; }
        public string Email { get; set; }
    }
}
