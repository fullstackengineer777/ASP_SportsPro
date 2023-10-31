using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private MySqlDatabase MySqlDatabase { get; set; }
        public TechnicianController(MySqlDatabase mySqlDatabase)
        {
            this.MySqlDatabase = mySqlDatabase;
        }
        public ActionResult Index()
        {
            return View(GetTechnicians());
        }

        // GET: Technician/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Technician technician)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO Technicians ( Name, Email, Phone) " +
                           "VALUES ( @Name, @Email, @Phone)";
                cmd.Parameters.AddWithValue("@Name", technician.Name);
                cmd.Parameters.AddWithValue("@Email", technician.Email);
                cmd.Parameters.AddWithValue("@Phone", technician.Phone);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Technician/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT * FROM Technicians WHERE TechnicianID = @TechnicianID";
                cmd.Parameters.AddWithValue("@TechnicianID", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Technician technician = new Technician()
                        {
                            TechnicianID = Convert.ToInt32(reader["TechnicianID"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString()
                        };

                        return View(technician);
                    }

                }

            }
            catch
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        // POST: Technician/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Technician technician)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"UPDATE Technicians SET Name=@Name, Email=@Email, Phone=@Phone WHERE TechnicianID = @TechnicianID";
                cmd.Parameters.AddWithValue("@TechnicianID", technician.TechnicianID);
                cmd.Parameters.AddWithValue("@Name", technician.Name);
                cmd.Parameters.AddWithValue("@Phone", technician.Phone);
                cmd.Parameters.AddWithValue("@Email", technician.Email);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Technician/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"Delete from  Technicians WHERE TechnicianID = @TechnicianID";
                cmd.Parameters.AddWithValue("@TechnicianID", id);
                var recs = cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
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
