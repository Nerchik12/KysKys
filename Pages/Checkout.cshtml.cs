using KysKys.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KysKys.Pages;
[Authorize]
public class CheckoutModel(AppDb db) : PageModel
{
    public List<Product> Items { get; set; } = new();
    [BindProperty] public string Address { get; set; } = "";
    [BindProperty] public string Phone { get; set; } = "";
    List<Product> Load() => CartStore.Get(HttpContext.Session).Select(id => Store.Products.First(p => p.Id == id)).ToList();
    public IActionResult OnGet()
    {
        Items = Load(); if (!Items.Any()) return RedirectToPage("/Cart");
        Phone = db.Users.Find(User.Id())?.Phone ?? ""; return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        Items = Load(); if (!Items.Any()) return RedirectToPage("/Cart");
        db.Orders.Add(new Order { UserId = User.Id(), Address = Address.Trim(), Phone = Phone.Trim(), Total = Items.Sum(i => i.Price),
            Items = string.Join(", ", Items.GroupBy(i => i.Name).Select(g => $"{g.Key} ×{g.Count()}")) });
        await db.SaveChangesAsync();
        CartStore.Set(HttpContext.Session, new());
        return RedirectToPage("/Account");
    }
}
