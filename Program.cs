using Takip.Services;
using Takip.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<PersonnelRepository>();
builder.Services.AddScoped<SaleRepository>();
builder.Services.AddScoped<SaleService>();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // <-- unutma!

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
