using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Transaction.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http; 

namespace Transaction.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(TR model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO traons (Title, Amount, Type, Category, Date) VALUES (@title, @amount, @type, @category, @date)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", model.Title);
                        cmd.Parameters.AddWithValue("@amount", model.Amount);
                        cmd.Parameters.AddWithValue("@type", model.Type);
                        cmd.Parameters.AddWithValue("@category", model.Category);
                        cmd.Parameters.AddWithValue("@date", model.Date);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (MySqlException ex)
            {
                ModelState.AddModelError(string.Empty, "Database connection failed: " + ex.Message);
                return View("Index", model);
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
                return View("Index", model);
            }
        }

        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var adminUser = _configuration["LoginSettings:AdminUser"];
            var adminPass = _configuration["LoginSettings:AdminPass"];

            if (username == adminUser && password == adminPass)
            {
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("Index");
            }

            ViewBag.Error = "ভুল ইউজারনেম বা পাসওয়ার্ড!";
            return View();
        }
    }
}