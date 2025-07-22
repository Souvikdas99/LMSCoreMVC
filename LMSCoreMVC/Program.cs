using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using LMSCoreMVC.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddDbContext<LMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<JwtService>();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600 ; 
});


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
app.UseSession();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LMSDbContext>();

    if (!context.Subjects.Any())
    {
        context.Subjects.AddRange(
            new Subjects { Name = "Data Structures", CreditPoints = 4 },
            new Subjects { Name = "Operating Systems", CreditPoints = 3 },
            new Subjects { Name = "Database Management Systems", CreditPoints = 4 },
            new Subjects { Name = "Artificial Intelligence", CreditPoints = 3 },
            new Subjects { Name = "Software Engineering", CreditPoints = 2 }
        );

        context.SaveChanges();
    }
}
