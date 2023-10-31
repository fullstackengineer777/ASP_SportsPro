using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private MySqlDatabase MySqlDatabase { get; set; }

        public CustomerController(MySqlDatabase mySqlDatabase)
        {
            this.MySqlDatabase = mySqlDatabase;
        }
        // GET: CustomerController
        public ActionResult Index()
        {
            var customers = GetCustomers();
            return View(customers);
           // var countrys = GetCountries();
            //return View("Create", new { customers = customers, countrys = countrys });
        }

        
        // GET: CustomerController/Create
        public ActionResult Create()
        {
            ViewBag.countrys = GetCountries();
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO Customers ( FirstName, LastName, Address, City, State, PostalCode, Phone, Email, CountryID) " +
                           "VALUES ( @FirstName, @LastName, @Address, @City, @State, @PostalCode, @Phone, @Email, @CountryID )";

                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@City", customer.City);
                cmd.Parameters.AddWithValue("@State", customer.State);
                cmd.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@CountryID", customer.CountryID);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                cmd.Parameters.AddWithValue("@CustomerID", id);
                ViewBag.countrys = GetCountries();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Customer customer = new Customer()
                        {
                            CustomerID = Convert.ToInt32(reader["CustomerID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Address = reader["Address"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            PostalCode = reader["PostalCode"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString(),
                            CountryID = reader["CountryID"].ToString()
                        };
                        ViewBag.customer = customer;
                        return View();
                    }

                }

            }
            catch
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Customer customer)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"UPDATE Customers SET FirstName = @FirstName , LastName=@LastName, Address=@Address, City=@City," +
                    " State=@State, PostalCode=@PostalCode,Phone=@Phone,Email=@Email,CountryID=@CountryID WHERE CustomerID = @CustomerID";
                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@City", customer.City);
                cmd.Parameters.AddWithValue("@State", customer.State);
                cmd.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@CountryID", customer.CountryID);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"Delete from  Customers WHERE CustomerID = @CustomerID";
                cmd.Parameters.AddWithValue("@CustomerID", id);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        private List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM Customers";

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var t = new Customer()
                    {
                        CustomerID = reader.GetFieldValue<int>(0),
                        FirstName = reader.GetFieldValue<string>(1),
                        LastName = reader.GetFieldValue<string>(2),
                        Address = reader.GetFieldValue<string>(3),
                        City = reader.GetFieldValue<string>(4),
                        State = reader.GetFieldValue<string>(5),
                        PostalCode = reader.GetFieldValue<string>(6),
                        Phone = reader.GetFieldValue<string>(7),
                        Email = reader.GetFieldValue<string>(8),
                        CountryID = reader.GetFieldValue<string>(9)
                    };

                    customers.Add(t);
                }

            }
            return customers;
        }

        private List<Country> GetCountries()
        {
            var countrys = new List<Country>();
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM Countrys";

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var t = new Country()
                    {
                        CountryID = reader.GetFieldValue<string>(0),
                        Name = reader.GetFieldValue<string>(1)
                    };

                    countrys.Add(t);
                }

            }
            return countrys;
        }
    }
}
