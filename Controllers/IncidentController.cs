using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SportsPro.Models;
using System.Reflection.PortableExecutable;
using Ubiety.Dns.Core;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private MySqlDatabase MySqlDatabase { get; set; }

        public IncidentController(MySqlDatabase mySqlDatabase)
        {
            this.MySqlDatabase = mySqlDatabase;
        
        }
        // GET: IncidentController
        public ActionResult Index()
        {
            return View(GetIncidents());
        }

        // GET: IncidentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IncidentController/Create
        public ActionResult Create()
        {
            ViewBag.products = GetProducts();
            ViewBag.customers = GetCustomers();
            ViewBag.technicians = GetTechnicians();
            return View();
        }

        // POST: IncidentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Incident incident)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO Incidents ( Title, Description, DateOpened, DateClosed, CustomerID,ProductID,TechnicianID) " +
                           "VALUES ( @Title, @Description, @DateOpened, @DateClosed,  @CustomerID, @ProductID, @TechnicianID)";

                //cmd.Parameters.AddWithValue("@ProductID", 133);
                cmd.Parameters.AddWithValue("@Title", incident.Title);
                cmd.Parameters.AddWithValue("@Description", incident.Description);
                cmd.Parameters.AddWithValue("@DateOpened", incident.DateOpened);
                cmd.Parameters.AddWithValue("@DateClosed", incident.DateClosed);
                cmd.Parameters.AddWithValue("@CustomerID", incident.CustomerID);
                cmd.Parameters.AddWithValue("@ProductID", incident.ProductID);
                cmd.Parameters.AddWithValue("@TechnicianID", incident.TechnicianID);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: IncidentController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT * FROM Incidents WHERE IncidentID = @IncidentID";
                cmd.Parameters.AddWithValue("@IncidentID", id);

                ViewBag.products = GetProducts();
                ViewBag.customers = GetCustomers();
                ViewBag.technicians = GetTechnicians();
               using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Incident incident = new Incident()
                        {
                            IncidentID = Convert.ToInt32(reader["IncidentID"]),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            DateOpened = Convert.ToDateTime(reader["DateOpened"]),
                            DateClosed = Convert.ToDateTime(reader["DateClosed"]),
                            CustomerID = Convert.ToInt32(reader["CustomerID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            TechnicianID = Convert.ToInt32(reader["TechnicianID"])
                        };

                    /*    IncidentParam inc_param = new IncidentParam
                        {
                            products = GetProducts();
                            technicians = GetTechnicians();
                            customers = GetCustomers();
                            incident = incident;
                        }*/

                        ViewBag.incident = incident;                        

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

        // POST: IncidentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Incident incident)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"UPDATE Incidents SET Title = @Title, Description=@Description, DateOpened=@DateOpened, DateClosed=@DateClosed, "  +
                    " CustomerID=@CustomerID, ProductID=@ProductID, TechnicianID=@TechnicianID WHERE IncidentID = @IncidentID";

                cmd.Parameters.AddWithValue("@IncidentID", incident.IncidentID);
                cmd.Parameters.AddWithValue("@Title", incident.Title);
                cmd.Parameters.AddWithValue("@Description", incident.Description);
                cmd.Parameters.AddWithValue("@DateOpened", incident.DateOpened);
                cmd.Parameters.AddWithValue("@DateClosed", incident.DateClosed);
                cmd.Parameters.AddWithValue("@CustomerID", incident.CustomerID);
                cmd.Parameters.AddWithValue("@ProductID", incident.ProductID);
                cmd.Parameters.AddWithValue("@TechnicianID", incident.TechnicianID);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: IncidentController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"Delete from Incidents WHERE IncidentID = @IncidentID";
                cmd.Parameters.AddWithValue("@IncidentID", id);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // POST: IncidentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private List<Incident> GetIncidents()
        {
            var incidents = new List<Incident>();
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM Incidents";

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var t = new Incident()
                    {
                        IncidentID = reader.GetFieldValue<int>(0),
                        Title = reader.GetFieldValue<string>(1),
                        Description = reader.GetFieldValue<string>(2),
                        DateOpened = reader.GetFieldValue<DateTime>(3),
                        DateClosed = reader.GetFieldValue<DateTime>(4),
                        CustomerID = reader.GetFieldValue<int>(5),
                        ProductID = reader.GetFieldValue<int>(6),
                        TechnicianID = reader.GetFieldValue<int>(7),
                    };                 
                       /* */
                    incidents.Add(t);
                   
                }
                reader.Close();
            }
            
            int index = 0;
            while (index < incidents.Count)
            {                
                int customerid = incidents[index].CustomerID;
                cmd.CommandText = @"SELECT * FROM Customers WHERE CustomerID = " + customerid.ToString();
               
                using (MySqlDataReader reader1 = cmd.ExecuteReader())
                {
                    if (reader1.Read())
                    {
                        incidents[index].Customer = new Customer()
                        {
                            FirstName = reader1["FirstName"].ToString(),
                            LastName = reader1["LastName"].ToString()
                        };
                    }
                }
                int productid = incidents[index].ProductID;
                cmd.CommandText = @"SELECT * FROM Products WHERE ProductID = " + productid.ToString();
                using (MySqlDataReader reader2 = cmd.ExecuteReader())
                {
                    if (reader2.Read())
                    {
                        incidents[index].Product = new Product()
                        {
                            Name = reader2["Name"].ToString()
                        };
                    }
                }
                int technicianid = incidents[index].TechnicianID;
                cmd.CommandText = @"SELECT * FROM Technicians WHERE TechnicianID = " + technicianid.ToString();
                using (MySqlDataReader reader3 = cmd.ExecuteReader())
                {
                    if (reader3.Read())
                    {
                        incidents[index].Technician = new Technician()
                        {
                            Name = reader3["Name"].ToString()
                        };
                    }
                }

                index++;
            }

            return incidents;
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
                       // Address = reader.GetFieldValue<string>(3),
                       // City = reader.GetFieldValue<string>(4),
                       // State = reader.GetFieldValue<string>(5),
                       // PostalCode = reader.GetFieldValue<string>(6),
                       // Phone = reader.GetFieldValue<string>(7),
                       // Email = reader.GetFieldValue<string>(8),
                      //  CountryID = reader.GetFieldValue<string>(9)
                    };

                    customers.Add(t);
                }

            }
            return customers;
        }

        private List<Technician> GetTechnicians()
        {
            var technicians = new List<Technician>();
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM Technicians";


            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var t = new Technician()
                    {
                        TechnicianID = reader.GetFieldValue<int>(0),
                        Name = reader.GetFieldValue<string>(1),
                        Email = reader.GetFieldValue<string>(2),
                        Phone = reader.GetFieldValue<string>(3)
                    };

                    technicians.Add(t);
                }

            }
            return technicians;
        }
    }
}
