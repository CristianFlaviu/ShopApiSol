namespace ShopApi.Authentication
{
    public class LoginTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ErrorMessage { get; set; }
    }
}
