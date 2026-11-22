namespace KysKys.Data;
public record Product(int Id, string Name, string Category, string Brand, decimal Price, decimal? Old, double Rating, bool IsNew, string Icon);

public static class Store
{
    public static readonly string[] Categories = { "Корма", "Одежда", "Игрушки", "Лежанки", "Лакомства", "Гигиена" };
    public static readonly string[] Brands = { "Pro Plan", "Royal Canin", "Purina", "Brit", "Zoetis", "Light Bites", "Grandorf", "Barkals", "Stelline", "Trial" };

    public static readonly List<Product> Products = Enumerable.Range(1, 36).Select(i =>
    {
        var cat = Categories[i % Categories.Length];
        var icon = cat switch { "Корма" => "🥫", "Одежда" => "🧥", "Игрушки" => "🧸", "Лежанки" => "🛏️", "Лакомства" => "🦴", _ => "🧴" };
        decimal price = 250 + i * 37 % 1900;
        return new Product(i, $"{cat}: товар №{i}", cat, Brands[i % Brands.Length], price,
            i % 3 == 0 ? Math.Round(price * 1.2m) : null, 4 + (i % 10) / 10.0, i % 4 == 0, icon);
    }).ToList();

}

public static class CartStore
{
    public static List<int> Get(ISession s) =>
        (s.GetString("cart") ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    public static void Set(ISession s, List<int> l) => s.SetString("cart", string.Join(',', l));
}
