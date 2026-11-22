using KysKys.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var b = WebApplication.CreateBuilder(args);

// Данные (SQLite и ключи cookie) лежат в App_Data — эта папка не перезаписывается при деплое.
var dataDir = Path.Combine(b.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(dataDir);
var conn = b.Configuration.GetConnectionString("Default") ?? $"Data Source={Path.Combine(dataDir, "kyskys.db")}";

b.Services.AddRazorPages();
b.Services.AddDbContext<AppDb>(o => o.UseSqlite(conn));
b.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(dataDir, "keys"))).SetApplicationName("KysKys");
b.Services.AddDistributedMemoryCache();
b.Services.AddSession(o => { o.Cookie.HttpOnly = true; o.Cookie.IsEssential = true; });
b.Services.AddAuthentication("Cookies").AddCookie("Cookies", o =>
{
    o.LoginPath = "/Login"; o.ExpireTimeSpan = TimeSpan.FromDays(14); o.SlidingExpiration = true;
    o.Cookie.HttpOnly = true; o.Cookie.SameSite = SameSiteMode.Lax;
});
b.Services.AddAuthorization();

var app = b.Build();
using (var s = app.Services.CreateScope())
{
    var db = s.ServiceProvider.GetRequiredService<AppDb>();
    db.Database.EnsureCreated();
    if (!db.Reviews.Any())
    {
        db.Reviews.AddRange(
            new Review { Name = "Алиса", Stars = 5, Text = "Быстрая доставка и отличный выбор кормов. Кошка в восторге!" },
            new Review { Name = "Кирилл", Stars = 5, Text = "Удобный сайт, всё нашёл за пару минут. Скидки приятные." },
            new Review { Name = "Софи", Stars = 4, Text = "Заказывала одежду для собаки — размер подошёл идеально." },
            new Review { Name = "Артём", Stars = 5, Text = "Обмен прошёл без проблем, спасибо команде." });
        db.SaveChanges();
    }
}
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
