using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using dotnet_crud_mvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace dotnet_crud_mvc.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            DataTable da = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sdd = new SqlDataAdapter("Select *from Customers", sqlConnection);
                sdd.Fill(da);
            }
            return View(da);
        }
        [HttpGet]

        public IActionResult AddOrEdit(int? id)
        {
            Customers Customers = new Customers();
            if (id > 0)
            {
                Customers = FetchById(id);
            }
            return View(Customers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(Customers customers, int Customer_ID)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();

                    if (Customer_ID == 0)
                    {
                        string query = "Insert into Customers Values(@Name,@Address,@Phone)";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlConnection);
                        sqlCmd.Parameters.AddWithValue("@Name", customers.Name);
                        sqlCmd.Parameters.AddWithValue("@Address", customers.Address);
                        sqlCmd.Parameters.AddWithValue("@Phone", customers.Phone);
                        sqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string query = "Update Customers SET Name=@Name,Address=@Address,Phone=@Phone where Customer_ID=@Customer_ID";
                        SqlCommand sqlcmd = new SqlCommand(query, sqlConnection);
                        sqlcmd.Parameters.AddWithValue("@Customer_ID", customers.Customer_ID);
                        sqlcmd.Parameters.AddWithValue("@Name", customers.Name);
                        sqlcmd.Parameters.AddWithValue("@Address", customers.Address);
                        sqlcmd.Parameters.AddWithValue("@Phone", customers.Phone);
                        sqlcmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customers);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Customers Customers = FetchById(id);

            return View(Customers);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult DeleteConfirmed(int? id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                string query = "Delete from Customers where Customer_ID = @Customer_ID";
                SqlCommand sdd = new SqlCommand(query, sqlConnection);
                sdd.Parameters.AddWithValue("@Customer_ID", id);
                sdd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public Customers FetchById(int? id)
        {
            Customers customer = new Customers();
            DataTable da = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                string query = "Select *from Customers where Customer_ID=@Customer_ID";
                SqlDataAdapter sdd = new SqlDataAdapter(query, sqlConnection);
                sdd.SelectCommand.Parameters.AddWithValue("@Customer_ID", id);
                sdd.Fill(da);

                if (da.Rows.Count == 1)
                {
                    customer.Customer_ID = Convert.ToInt32(da.Rows[0]["Customer_ID"].ToString());
                    customer.Name = da.Rows[0]["Name"].ToString();
                    customer.Address = da.Rows[0]["Address"].ToString();
                    customer.Phone = da.Rows[0]["Phone"].ToString();
                }

                return customer;
            }

        }

    }
}
