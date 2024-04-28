namespace recipe.Models
{
    public class LoginRequestModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignUpRequestModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ChangePasswordRequestModel
    {
        public int UserId { get; set; }
        public string ExistingPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangeUsernameRequestModel
    {
        public string ExistingUsername { get; set; }
        public string NewUsername { get; set; }
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }

        
    }
    public class TokenApiDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}