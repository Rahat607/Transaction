var builder = WebApplication.CreateBuilder(args);

// ১. সেশন সার্ভিস যোগ করুন (builder.Build() এর আগে)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ৩০ মিনিট পর অটো লগআউট হবে
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ২. সেশন মিডলওয়্যার ব্যবহার করুন (UseRouting এবং UseAuthorization এর মাঝে রাখা ভালো)
app.UseSession();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}"); // ডিফল্ট পেজ Login করে দিন

app.Run();