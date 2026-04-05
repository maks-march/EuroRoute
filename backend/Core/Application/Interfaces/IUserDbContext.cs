using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IUserDbContext
{
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}