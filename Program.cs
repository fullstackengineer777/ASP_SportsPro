using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SportsPro;
using SportsPro.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options => {
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<SportsProContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SportsPro")));
//connect mysql
//builder.Services.AddTransient<MySqlConnection>(_ =>
//    new MySqlConnection(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddTransient<MySqlDatabase>(_ => new MySqlDatabase("server=localhost; database=asp; uid=root; pwd=;"));

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


//Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "productall",
    pattern: "{controller=Product}/{action=Index}");
app.MapControllerRoute(
    name: "productedit",
    pattern: "{controller=Product}/{action=Edit}/{id}");
app.MapControllerRoute(
    name: "productcreate",
    pattern: "{controller=Product}/{action=Create}");
app.MapControllerRoute(
    name: "productcreate",
    pattern: "{controller=Product}/{action=Create}/{id}");
app.MapControllerRoute(
    name: "productdelete",
    pattern: "{controller=Product}/{action=Delete}/{id}");

app.MapControllerRoute(
    name: "technicianall",
    pattern: "{controller=Technician}/{action=Index}");
app.MapControllerRoute(
    name: "technicianedit",
    pattern: "{controller=Technician}/{action=Edit}/{id}");
app.MapControllerRoute(
    name: "techniciancreate",
    pattern: "{controller=Technician}/{action=Create}");
app.MapControllerRoute(
    name: "techniciancreate",
    pattern: "{controller=Technician}/{action=Create}/{id}");
app.MapControllerRoute(
    name: "techniciandelete",
    pattern: "{controller=Technician}/{action=Delete}/{id}");

app.MapControllerRoute(
    name: "customerall",
    pattern: "{controller=Customer}/{action=Index}");
app.MapControllerRoute(
    name: "customercreate",
    pattern: "{controller=Customer}/{action=Create}");
app.MapControllerRoute(
    name: "customerdelete",
    pattern: "{controller=Customer}/{action=Delete}/{id}");

app.MapControllerRoute(
    name: "incidentall",
    pattern: "{controller=Incident}/{action=Index}");
app.MapControllerRoute(
    name: "incidentcreate",
    pattern: "{controller=Incident}/{action=Create}");


app.Run();
