//using recipe.Models; // Import the namespace containing UserModel
//using System.Security.Cryptography;
//using System.Text;
//using recipe.context;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;



//namespace recipe.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly appDbContext dbContext; // Replace AppDbContext with your actual DbContext class name


//        public AuthService(appDbContext dbContext)
//        {
//            this.dbContext = dbContext;
//        }

//        // Change access modifier to public
       





//        public async Task<AuthResult> LoginAsync(LoginRequestModel model)
//        {
//            try
//            {
//                UserModel user = GetUserFromDatabase(model.Username);

//                if (user != null)
//                {
//                    if (VerifyPassword(model.Password, user.Password))
//                    {
//                        string token = GenerateToken(model.Username);

//                        return new AuthResult
//                        {
//                            Success = true,
//                            Token = token,
//                            Message = "Login successful"
//                        };
//                    }
//                }

//                return new AuthResult
//                {
//                    Success = false,
//                    Message = "Invalid username or password"
//                };
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("An error occurred during login: " + ex.Message);
//                return new AuthResult
//                {
//                    Success = false,
//                    Message = "An error occurred during login. Please try again later."
//                };
//            }
//        }


//        public async Task<AuthResult> SignUpAsync(SignUpRequestModel model)
//        {
//            try
//            {
//                if (UserExists(model.Username))
//                {
//                    return new AuthResult
//                    {
//                        Success = false,
//                        Message = "Username already exists. Choose another username."
//                    };
//                }

//                string hashedPassword = HashPassword(model.Password);

//                CreateUser(model.Username, hashedPassword);

//                string token = GenerateToken(model.Username);

//                return new AuthResult
//                {
//                    Success = true,
//                    Token = token,
//                    Message = "Signup successful"
//                };
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("An error occurred during signup: " + ex.Message);
//                return new AuthResult
//                {
//                    Success = false,
//                    Message = "An error occurred during signup. Please try again later."
//                };
//            }
//        }

//        private UserModel GetUserFromDatabase(string username)
//        {
//            return dbContext.Users.FirstOrDefault(u => u.Username == username);
//        }

//        private bool UserExists(string username)
//        {
//            return dbContext.Users.Any(u => u.Username == username);
//        }

//        public string HashPassword(string password)
//        {
//            using (var sha256 = SHA256.Create())
//            {
//                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//                StringBuilder builder = new StringBuilder();
//                for (int i = 0; i < bytes.Length; i++)
//                {
//                    builder.Append(bytes[i].ToString("x2"));
//                }
//                return builder.ToString();
//            }
//        }

//        public bool VerifyPassword(string password, string hashedPassword)
//        {
//            string hashedInputPassword = HashPassword(password);
//            return hashedInputPassword.Equals(hashedPassword);
//        }

//        private string GenerateToken(string username)
//        {
//            return Guid.NewGuid().ToString();
//        }

//        private void CreateUser(string username, string hashedPassword)
//        {
//            var newUser = new UserModel
//            {
//                Username = username,
//                Password = hashedPassword
//            };

//            dbContext.Users.Add(newUser);
//            dbContext.SaveChanges();
//        }

//        public async Task<AuthResult> ChangePasswordAsync(int userId, ChangePasswordRequestModel model)
//        {
//            try
//            {
//                // Find the user with the given userId in the database
//                UserModel user = await dbContext.Users.FindAsync(userId);

//                // Verify if the user exists
//                if (user != null)
//                {
//                    // Verify if the existing password matches
//                    if (VerifyPassword(model.ExistingPassword, user.Password))
//                    {
//                        // Hash the new password
//                        string hashedNewPassword = HashPassword(model.NewPassword);

//                        // Update the user's password
//                        user.Password = hashedNewPassword;

//                        // Save changes to the database
//                        dbContext.SaveChanges();

//                        // Return success result
//                        return new AuthResult { Success = true, Message = "Password changed successfully" };
//                    }
//                    else
//                    {
//                        // Existing password does not match
//                        return new AuthResult { Success = false, Message = "Existing password is incorrect" };
//                    }
//                }
//                else
//                {
//                    // User not found
//                    return new AuthResult { Success = false, Message = "User not found" };
//                }
//            }
//            catch (Exception ex)
//            {
//                // Log error and return failure result
//                Console.WriteLine("An error occurred during password change: " + ex.Message);
//                return new AuthResult { Success = false, Message = "An error occurred. Please try again later." };
//            }
//        }

//        public async Task<AuthResult> ChangeUsernameAsync(int userId, ChangeUsernameRequestModel model)
//        {
//            try
//            {
//                // Find the user with the given userId in the database
//                UserModel user = await dbContext.Users.FindAsync(userId);

//                // Verify if the user exists
//                if (user != null)
//                {
//                    // Update the user's username
//                    user.Username = model.NewUsername;

//                    // Save changes to the database
//                    dbContext.SaveChanges();

//                    // Return success result
//                    return new AuthResult { Success = true, Message = "Username changed successfully" };
//                }
//                else
//                {
//                    // User not found
//                    return new AuthResult { Success = false, Message = "User not found" };
//                }
//            }
//            catch (Exception ex)
//            {
//                // Log error and return failure result
//                Console.WriteLine("An error occurred during username change: " + ex.Message);
//                return new AuthResult { Success = false, Message = "An error occurred. Please try again later." };
//            }
//        }










//    }
//}
