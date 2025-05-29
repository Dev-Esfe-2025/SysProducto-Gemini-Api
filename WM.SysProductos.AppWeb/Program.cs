using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WM.SysProductos.AppWeb.Service;
using WM.SysProductos.BL;
using WM.SysProductos.DAL;
using WM.SysProveedors.DAL;

var builder = WebApplication.CreateBuilder(args);
ExcelPackage.LicenseContext = LicenseContext.Commercial;

// Add services to the container.
builder.Services.AddDbContext<SysProductosDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Conn");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


builder.Services.AddScoped<ProductoDAL>();
builder.Services.AddScoped<ProductoBL>();

builder.Services.AddScoped<ProveedorDAL>();
builder.Services.AddScoped<ProveedorBL>();

builder.Services.AddScoped<CompraBL>();
builder.Services.AddScoped<CompraDAL>();

builder.Services.AddScoped<ClienteDAL>();
builder.Services.AddScoped<ClienteBL>();
builder.Services.AddScoped<VentaDAL>();
builder.Services.AddScoped<VentaBL>();

builder.Services.AddHttpClient<IGeminiService, GeminiService>();

builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//Configuraciòn para uso de rotativa, Path
IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.
    Setup(env.WebRootPath, "../wwwroot/Rotativa");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
