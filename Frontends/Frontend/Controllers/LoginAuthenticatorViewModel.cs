namespace Frontend.Controllers
{
    public class LoginAuthenticatorViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string AuthenticatorSecrete { get; set; }
    }
}