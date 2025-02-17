namespace ShopEase.DTOs.Response
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsVerified { get; set; }
    }
}
