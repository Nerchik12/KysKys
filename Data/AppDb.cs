using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
namespace KysKys.Data;

public class User { public int Id { get; set; } public string Name { get; set; } = ""; public string Email { get; set; } = ""; public string Phone { get; set; } = ""; public string PasswordHash { get; set; } = ""; }
public class Order { public int Id { get; set; } public int UserId { get; set; } public DateTime Created { get; set; } = DateTime.UtcNow; public string Address { get; set; } = ""; public string Phone { get; set; } = ""; public decimal Total { get; set; } public string Items { get; set; } = ""; public string Status { get; set; } = "Новый"; }
public class Review { public int Id { get; set; } public string Name { get; set; } = ""; public int Stars { get; set; } = 5; public string Text { get; set; } = ""; public DateTime Created { get; set; } = DateTime.UtcNow; }
public class ContactMessage { public int Id { get; set; } public string Name { get; set; } = ""; public string Phone { get; set; } = ""; public DateTime Created { get; set; } = DateTime.UtcNow; }

public class AppDb(DbContextOptions<AppDb> o) : DbContext(o)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ContactMessage> Messages => Set<ContactMessage>();
    protected override void OnModelCreating(ModelBuilder m) => m.Entity<User>().HasIndex(u => u.Email).IsUnique();
}

public static class Auth
{
    public static Task SignIn(HttpContext c, User u) => c.SignInAsync("Cookies",
        new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()), new Claim(ClaimTypes.Name, u.Name) }, "Cookies")),
        new AuthenticationProperties { IsPersistent = true });
    public static int Id(this ClaimsPrincipal p) => int.Parse(p.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
