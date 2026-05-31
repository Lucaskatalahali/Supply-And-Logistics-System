using Microsoft.EntityFrameworkCore;
using Supply_And_Logistics_System.Data;
using Supply_And_Logistics_System.Models.Cart;
using Supply_And_Logistics_System.Models.Identity;
using Supply_And_Logistics_System.Models.Products;

var builder = WebApplication.CreateBuilder(args);

// =========================
// VERİTABANI YAPILANDIRMASI
// =========================
// PostgreSQL bağlantısı AppDbContext üzerinden yapılandırılır.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =========================
// SERVİS KAYITLARI
// =========================
// Kullanıcı girişleri ve rollerini takip etmek için Session (Oturum) desteği eklenir.
builder.Services.AddSession();

// MVC yapısı için gerekli servisler eklenir.
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
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// Uygulama başladığında Home/Index sayfasına yönlendirir.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// =========================
// SEED
// =========================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    // =========================
    // USERS
    // =========================
    if (!context.Users.Any())
    {
        // Admin
        var admin = new User
        {
            Name = "Admin",
            Email = "admin@system.com",
            PasswordHash = "admin123",
            Role = Role.Admin
        };

        // Warehouse Staff
        var warehouse = new User
        {
            Name = "Warehouse Staff",
            Email = "warehouse@system.com",
            PasswordHash = "warehouse123",
            Role = Role.WarehouseStaff
        };

        // Couriers — um por transportadora
        var courierAras = new User
        {
            Name = "Courier Aras",
            Email = "courier.aras@system.com",
            PasswordHash = "courier123",
            Role = Role.Courier,
            Company = "aras"
        };

        var courierYurtici = new User
        {
            Name = "Courier Yurtici",
            Email = "courier.yurtici@system.com",
            PasswordHash = "courier123",
            Role = Role.Courier,
            Company = "yurtici"
        };

        var courierGlobal = new User
        {
            Name = "Courier GlobalExpress",
            Email = "courier.global@system.com",
            PasswordHash = "courier123",
            Role = Role.Courier,
            Company = "globalexpress"
        };

        // Customer
        var customer = new User
        {
            Name = "Lucas Silva",
            Email = "lucas@system.com",
            PasswordHash = "lucas123",
            Role = Role.Customer,
            Address = "Rua das Flores 123, Lisboa"
        };

        context.Users.AddRange(admin, warehouse, courierAras, courierYurtici, courierGlobal, customer);
        context.SaveChanges();

        // Cart para o customer
        context.Carts.Add(new Cart { UserId = customer.Id });
        context.SaveChanges();
    }

    // =========================
    // PRODUCTS
    // =========================
    if (!context.Products.Any())
    {
        // Simple Products
        var mouse = new SimpleProduct
        {
            Name = "Mouse",
            Description = "Wireless optical mouse",
            Price = 15.99m,
            Stock = 50,
            Threshold = 5
        };

        var keyboard = new SimpleProduct
        {
            Name = "Keyboard",
            Description = "Mechanical keyboard RGB",
            Price = 45.00m,
            Stock = 30,
            Threshold = 5
        };

        var monitor = new SimpleProduct
        {
            Name = "Monitor",
            Description = "24 inch Full HD monitor",
            Price = 199.99m,
            Stock = 10,
            Threshold = 2
        };

        var ram = new SimpleProduct
        {
            Name = "RAM 16GB",
            Description = "DDR4 3200MHz RAM",
            Price = 55.00m,
            Stock = 20,
            Threshold = 3
        };

        var cpu = new SimpleProduct
        {
            Name = "CPU Intel i5",
            Description = "Intel Core i5 12th Gen",
            Price = 220.00m,
            Stock = 15,
            Threshold = 3
        };

        var headphones = new SimpleProduct
        {
            Name = "Headphones",
            Description = "Noise cancelling headphones",
            Price = 89.99m,
            Stock = 25,
            Threshold = 4
        };

        context.Products.AddRange(mouse, keyboard, monitor, ram, cpu, headphones);
        context.SaveChanges();

        // Composite Product — Gaming PC
        var gamingPc = new CompositeProduct
        {
            Name = "Gaming PC Bundle",
            Threshold = 2
        };
        gamingPc.AddComponent(ram);
        gamingPc.AddComponent(cpu);
        gamingPc.AddComponent(monitor);

        // Composite Product — Office Pack
        var officePack = new CompositeProduct
        {
            Name = "Office Pack",
            Threshold = 3
        };
        officePack.AddComponent(mouse);
        officePack.AddComponent(keyboard);

        context.Products.AddRange(gamingPc, officePack);
        context.SaveChanges();
    }
}

app.Run();
