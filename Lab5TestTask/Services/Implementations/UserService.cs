using Lab5TestTask.Data;
using Lab5TestTask.Enums;
using Lab5TestTask.Models;
using Lab5TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab5TestTask.Services.Implementations;

/// <summary>
/// UserService implementation.
/// Implement methods here.
/// </summary>
public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> GetUserAsync()
    {
        // There can be more than one user
        var userId = await _dbContext.Sessions
            .GroupBy(x => x.UserId)
            .OrderByDescending(x => x.Count())
            .Select(x => x.Key)
            .FirstOrDefaultAsync();

        return await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _dbContext.Users
        .Where(u => _dbContext.Sessions
            .Where(s => s.DeviceType == DeviceType.Mobile)
            .Select(s => s.UserId)
            .Distinct()
            .Contains(u.Id))
        .ToListAsync();
    }
}
