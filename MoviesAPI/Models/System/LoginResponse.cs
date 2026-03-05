namespace MoviesAPI.Models.System
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public User User { get; set; }
        public string Error { get; set; }
    }
}
