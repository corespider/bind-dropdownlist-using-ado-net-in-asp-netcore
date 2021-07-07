using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MVCCore.BindDropdownList.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCore.BindDropdownList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration Configuration)
        {
            _logger = logger;
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            List<BookModel> books = GetBookList();
            return View(new SelectList(books, "BookId", "BookName"));
        }
        [HttpPost]
        public IActionResult Index(string BookId, string BookName)
        {
            ViewBag.Message = "Book Name: " + BookName;
            ViewBag.Message += "\\nBook Id: " + BookId;

            List<BookModel> books = GetBookList();
            return View(new SelectList(books, "BookId", "BookName"));
        }

        private List<BookModel> GetBookList()
        {
            string constr = _configuration.GetConnectionString("DefaultConnection");
            List<BookModel> books = new List<BookModel>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT BookId,BookName FROM Books";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            books.Add(new BookModel
                            {
                                BookName = sdr["BookName"].ToString(),
                                BookId = Convert.ToInt32(sdr["BookId"])
                            });
                        }
                    }
                    con.Close();
                }
            }
            return books;
        }
    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
