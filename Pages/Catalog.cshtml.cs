using KysKys.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KysKys.Pages;
public class CatalogModel : PageModel
{
    [BindProperty(SupportsGet = true)] public string? Category { get; set; }
    [BindProperty(SupportsGet = true)] public bool IsNew { get; set; }
    public IEnumerable<Product> Items => Store.Products.Where(p => (Category == null || p.Category == Category) && (!IsNew || p.IsNew));
}
