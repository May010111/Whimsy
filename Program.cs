using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Whimsy.Components;
using Whimsy.Data;
using Whimsy.Models;
using Whimsy.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=journal.db"));

builder.Services.AddScoped<EntryService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<SearchService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    await SeedMoodsAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static async Task SeedMoodsAsync(AppDbContext db)
{
    if (await db.Moods.AnyAsync()) return;
    db.Moods.AddRange(
        new Mood { Name = "Happy",    Emoji = "😊" },
        new Mood { Name = "Sad",      Emoji = "😢" },
        new Mood { Name = "Neutral",  Emoji = "😐" },
        new Mood { Name = "Excited",  Emoji = "🎉" },
        new Mood { Name = "Anxious",  Emoji = "😰" },
        new Mood { Name = "Angry",    Emoji = "😠" },
        new Mood { Name = "Grateful", Emoji = "🙏" },
        new Mood { Name = "Tired",    Emoji = "😴" }
    );
    await db.SaveChangesAsync();
}
