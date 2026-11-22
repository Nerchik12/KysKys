using KysKys.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KysKys.Pages;
public class RegisterModel(AppDb db) : PageModel
{
    [BindProperty] public string Name { get; set; } = "";
    [BindProperty] public string Email { get; set; } = "";
    [BindProperty] public string Phone { get; set; } = "";
    [BindProperty] public string Password { get; set; } = "";
    public string? Error { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
        var email = string.IsNullOrWhiteSpace(Email) ? Phone.Trim() + "@kyskys.ru" : Email.Trim().ToLower();
        if (Password.Length < 6) { Error = "Пароль должен быть не короче 6 символов"; return Page(); }
        if (db.Users.Any(x => x.Email == email)) { Error = "Эта почта уже зарегистрирована"; return Page(); }
        var u = new User { Name = Name.Trim(), Email = email, Phone = Phone.Trim() };
        u.PasswordHash = new PasswordHasher<User>().HashPassword(u, Password);
        db.Users.Add(u); await db.SaveChangesAsync();
        await Auth.SignIn(HttpContext, u);
        return LocalRedirect("/Account");
    }
}
