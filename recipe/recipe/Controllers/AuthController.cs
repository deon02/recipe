using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using recipe.Models;
using recipe.context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;



namespace recipe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly appDbContext _appDbContext;

        public AuthController(appDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            try
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user != null && VerifyPassword(model.Password, user.Password))
                {
                    var token = CreateJwt(user);
                    HttpContext.Session.SetString("IsLoggedIn", "true");

                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestModel model)
        {
            try
            {
                if (await UserExists(model.Username))
                {
                    return BadRequest(new { message = "Username already exists. Choose another username." });
                }

                string hashedPassword = HashPassword(model.Password);

                var newUser = new UserModel
                {
                    Username = model.Username,
                    Password = hashedPassword
                };

                _appDbContext.Users.Add(newUser);
                await _appDbContext.SaveChangesAsync();

                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username == model.Username); // Retrieve the newly created user

                var token = CreateJwt(user); // Use the retrieved user to generate the token

                return Ok(new { token, message = "Signup successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
        {
            try
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found!" });
                }

                if (VerifyPassword(model.ExistingPassword, user.Password))
                {
                    user.Password = HashPassword(model.NewPassword);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(new { message = "Password changed successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Existing password is incorrect" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("changeusername")]
        public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsernameRequestModel model)
        {
            try
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username == model.ExistingUsername);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found!" });
                }

                user.Username = model.NewUsername;
                await _appDbContext.SaveChangesAsync();
                return Ok(new { message = "Username changed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            // Clear the user session
            HttpContext.Session.Remove("user");
            HttpContext.Session.Remove("user_id");
            HttpContext.Session.Clear(); // Clear all session data
            HttpContext.Session.SetString("IsLoggedIn", "false");

            // Create a response with cache-control headers to prevent caching
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return Redirect(Url.Action("LogoutPage"));
        }

        // Define your logout page action method
        [HttpGet("logoutpage")]
        public IActionResult LogoutPage()
        {
            // Implement your logout page logic here
            return Content("You have been logged out successfully.");
        }

        [HttpGet("isloggedin")]
        public IActionResult IsLoggedIn()
        {
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            var jwtToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(jwtToken))
            {
                var principal = DecodeJwtToken(jwtToken);
                if (principal != null)
                {
                    var userIdClaim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        var usernameClaim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                        return Ok(new { isLoggedIn, userId, username = usernameClaim });
                    }
                }
            }

            return Ok(new { isLoggedIn });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound(new { message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }





        private async Task<bool> UserExists(string username)
        {
            return await _appDbContext.Users.AnyAsync(u => u.Username == username);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInputPassword = HashPassword(password);
            return hashedInputPassword.Equals(hashedPassword);
        }

        private string GenerateJwtSecret()
        {
            // Generate a random key of sufficient length for HS256 algorithm (256 bits)
            const int keySize = 256 / 8; // 256 bits
            var keyBytes = new byte[keySize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }
            return Convert.ToBase64String(keyBytes);
        }


        private string CreateJwt(UserModel user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = GenerateJwtSecret(); // Use the generated key
            var keyBytes = Encoding.ASCII.GetBytes(key);
            var identity = new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.Name, $"{user.Id}"),
        new Claim("Name", $"{user.Username}")
            });


            var credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal DecodeJwtToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(GenerateJwtSecret()); // Use the same key used for token generation
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // Adjust as needed
            };

            try
            {
                var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                // Token validation failed
                // You may log the exception or handle it according to your application's requirements
                return null;
            }
        }


        
    }
}

        






    

