
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "User",
    areaName: "User",
    pattern: "User/{controller=Home}/{action=Index}/{id?}"
);

app.Run();
