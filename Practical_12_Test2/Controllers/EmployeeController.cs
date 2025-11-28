using Microsoft.Data.SqlClient;
using Practical_12_Test2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practical_12_Test2.Controllers
{
    public class EmployeeController : Controller
    {
        string conString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;
        private List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "SELECT * FROM EmployeeTest2";
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new Employee()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
            }

            return employees;
        }

        public ActionResult Index()
        {
            List<Employee> employees = GetAllEmployees();
            return View(employees);
        }
        public ActionResult AddTestingRecords()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "INSERT INTO EmployeeTest2 (FirstName, MiddleName, LastName, DOB, MobileNumber, Address, Salary) VALUES ('Peter', NULL, 'Parker', '2001-01-01', '9999999999', 'New York, USA', 50000),('Tony', 'Edward', 'Stark', '1975-05-29', '8888888888', 'Los Angeles, USA', 90000),('Bruce', NULL, 'Wayne', '1980-02-19', '7777777777', 'Gotham City', 75000),('Clark', 'Joseph', 'Kent', '1985-06-18', '6666666666', 'Metropolis', 25000),('Natasha', NULL, 'Romanoff', '1990-11-22', '5555555555', 'Moscow, Russia', 80000);";
                SqlCommand InsertTestingRecordsCommand = new SqlCommand(sqlCommand, con);
                con.Open();
                InsertTestingRecordsCommand.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
        }
        public ActionResult TotalSalary()
        {
            decimal TotalSal = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "select sum(Salary) from EmployeeTest2";
                SqlCommand TotalSalaryCommand = new SqlCommand(sqlCommand, con);
                con.Open();
                TotalSal = Convert.ToDecimal(TotalSalaryCommand.ExecuteScalar());
            }
            TempData["TotalSalary"] = TotalSal;
            return RedirectToAction("IndexWithSalary");
        }
        public ActionResult IndexWithSalary()
        {
            List<Employee> employees = GetAllEmployees();
            ViewBag.TotalSalary = TempData["TotalSalary"];
            return View("Index", employees);
        }
        public ActionResult EmployeesBornBefore2000()
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "select * from EmployeeTest2 where DOB< '2000-01-01'";
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
            }
            return View("Index", employees);
        }
        public ActionResult CountMiddleNameNull()
        {
            int count = 0;
            List<Employee> employees = GetAllEmployees();
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "SELECT COUNT(*) FROM EmployeeTest2 WHERE MiddleName IS NULL";
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                con.Open();
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            ViewBag.EmpCount = count;
            return View("Index", employees);
        }
    }
}