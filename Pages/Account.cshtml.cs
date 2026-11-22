using Microsoft.AspNetCore.Authentication;
using KysKys.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KysKys.Pages;
[Authorize]
public class AccountModel(AppDb db) : PageModel
{
    public User Me { get; set; } = null!;
    public List<Order> Orders { get; set; } = new();
    public async Task<IActionResult> OnGetAsync()
    {
        var u = await db.Users.FindAsync(User.Id());
        if (u == null) { await HttpContext.SignOutAsync("Cookies"); return RedirectToPage("/Login"); }
        Me = u; Orders = db.Orders.Where(o => o.UserId == u.Id).OrderByDescending(o => o.Id).ToList();
        return Page();
    }
    public async Task<IActionResult> OnPostLogoutAsync() { await HttpContext.SignOutAsync("Cookies"); return Redirect("/"); }
}
