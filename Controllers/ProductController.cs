using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private MySqlDatabase MySqlDatabase { get; set; }

        public ProductController(MySqlDatabase mySqlDatabase)
        {
            this.MySqlDatabase = mySqlDatabase;
        }

        // GET: ProductController
        public  ActionResult Index()
        {
            return View( GetProducts());
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO Products ( ProductCode, Name, YearlyPrice, ReleaseDate) " +
                           "VALUES ( @ProductCode, @Name, @YearlyPrice, @ReleaseDate)";

                //cmd.Parameters.AddWithValue("@ProductID", 133);
                cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@YearlyPrice", product.YearlyPrice);
                cmd.Parameters.AddWithValue("@ReleaseDate", product.ReleaseDate);
               // cmd.Parameters.AddWithValue("@ReleaseDate", DateTime.Now.ToString("yyyy/MM/dd"));
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT * FROM Products WHERE ProductID = @ProductID";
                cmd.Parameters.AddWithValue("@ProductID", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Product product = new Product()
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductCode = reader["ProductCode"].ToString(),
                            Name = reader["Name"].ToString(),
                            YearlyPrice = Convert.ToInt32(reader["YearlyPrice"]),
                            ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"])
                        };

                        return View(product);
                    }

                }

            }
            catch
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Product product)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"UPDATE Products SET ProductCode = @ProductCode , Name=@Name, YearlyPrice=@YearlyPrice, ReleaseDate=@ReleaseDate WHERE ProductID = @ProductID";

                cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@YearlyPrice", product.YearlyPrice);
                cmd.Parameters.AddWithValue("@ReleaseDate", product.ReleaseDate);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"Delete from  Products WHERE ProductID = @ProductID";
                cmd.Parameters.AddWithValue("@ProductID", id);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

       

        private List<Product> GetProducts()
        {
            var products = new List<Product>();
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM Products";

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var t = new Product()
                    {
                        ProductID = reader.GetFieldValue<int>(0),
                        ProductCode = reader.GetFieldValue<string>(1),
                        Name = reader.GetFieldValue<string>(2),
                        YearlyPrice = reader.GetFieldValue<uint>(3),
                        ReleaseDate = reader.GetFieldValue<DateTime>(4)
                    };

                    products.Add(t);
                }

            }
            return products;
        }


    }

}
