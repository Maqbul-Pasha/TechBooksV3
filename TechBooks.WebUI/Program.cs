using Microsoft.EntityFrameworkCore;
using TechBooks.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string cnString = builder.Configuration.GetConnectionString("TechBooksCString")!;

builder.Services.AddDbContext<TechBooksContext>(options =>
{
    options.UseSqlServer(cnString);
});

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{ //*** Just for learning purposes (WEAKENING YOUR SECURITY IS NOT RECOMMENDED) ***
    options.Password.RequireDigit = false; //default: true
    options.Password.RequireLowercase = false; //default: true
    options.Password.RequireUppercase = false; //default: true
    options.Password.RequireNonAlphanumeric = false; //default: true
}).AddEntityFrameworkStores<TechBooksContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
