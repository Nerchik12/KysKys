using KysKys.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KysKys.Pages;
public class ContactsModel(AppDb db) : PageModel
{
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync(string name, string phone)
    {
        db.Messages.Add(new ContactMessage { Name = name.Trim(), Phone = phone.Trim() });
        await db.SaveChangesAsync(); TempData["ok"] = 1; return RedirectToPage();
    }
}
