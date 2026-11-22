using KysKys.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KysKys.Pages;
public class CartModel : PageModel
{
    public List<Product> Items { get; set; } = new();
    public void OnGet() => Items = CartStore.Get(HttpContext.Session).Select(id => Store.Products.First(p => p.Id == id)).ToList();
    public IActionResult OnPostAdd(int id)
    {
        var l = CartStore.Get(HttpContext.Session); l.Add(id); CartStore.Set(HttpContext.Session, l);
        var back = Request.Headers.Referer.ToString();
        return string.IsNullOrEmpty(back) ? RedirectToPage() : Redirect(back);
    }
    public IActionResult OnPostRemove(int id)
    {
        var l = CartStore.Get(HttpContext.Session); l.Remove(id); CartStore.Set(HttpContext.Session, l);
        return RedirectToPage();
    }
}
