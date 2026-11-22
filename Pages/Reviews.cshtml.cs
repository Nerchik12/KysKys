using KysKys.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KysKys.Pages;
public class ReviewsModel(AppDb db) : PageModel
{
    public List<Review> Items { get; set; } = new();
    public void OnGet() => Items = db.Reviews.OrderByDescending(r => r.Id).ToList();
    public async Task<IActionResult> OnPostAsync(string name, int stars, string text)
    {
        db.Reviews.Add(new Review { Name = name.Trim(), Stars = Math.Clamp(stars, 1, 5), Text = text.Trim() });
        await db.SaveChangesAsync(); return RedirectToPage();
    }
}
