using System.Threading.Tasks;
using recipe.Models;
using recipe.context;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly appDbContext _dbContext;

    public UserService(appDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserModel> GetUserByIdAsync(int userId)
    {
        return await _dbContext.Users.FindAsync(userId);
    }
}
