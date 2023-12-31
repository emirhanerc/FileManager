using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project2.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=EMIRHAN\\SQLEXPRESS;Database=filemanager;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"));

//bu kodu çalıştırıyoruz yoksa Ajax 400 hatası verir çalışmaz
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
