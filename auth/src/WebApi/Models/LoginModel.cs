namespace Chiron.Auth.Models {
    public class LoginModel {
        public string Id { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
        public bool RememberLogin { set; get; }
        public string ReturnUrl { set; get; }
    }
}
