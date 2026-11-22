using KysKys.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KysKys.Pages;
public class LoginModel(AppDb db) : PageModel
{
    [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }
    [BindProperty] public string Email { get; set; } = "";
    [BindProperty] public string Password { get; set; } = "";
    public string? Error { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
        var u = db.Users.FirstOrDefault(x => x.Email == Email.Trim().ToLower());
        if (u == null || new PasswordHasher<User>().VerifyHashedPassword(u, u.PasswordHash, Password) == PasswordVerificationResult.Failed)
        { Error = "Неверная почта или пароль"; return Page(); }
        await Auth.SignIn(HttpContext, u);
        return LocalRedirect(Url.IsLocalUrl(ReturnUrl) ? ReturnUrl! : "/Account");
    }
}
