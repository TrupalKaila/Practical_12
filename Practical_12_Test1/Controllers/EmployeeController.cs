using Microsoft.Data.SqlClient;
using Practical_12_Test1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practical_12_Test1.Controllers
{
    public class EmployeeController : Controller
    {
        string conString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;

        public ActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "Select * from Employee";
                SqlCommand GetAllEmployeeCommand = new SqlCommand(sqlCommand, con);
                con.Open();
                SqlDataReader reader = GetAllEmployeeCommand.ExecuteReader();
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
                        Address = reader["Address"].ToString()
                    });
                }

            }
            return View(employees);
        }
        public ActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "insert into Employee (FirstName, MiddleName, LastName, DOB, MobileNumber, Address) values (@FirstName, @MiddleName, @LastName, @DOB, @MobileNumber, @Address)";
                SqlCommand InsertEmployeeCommand = new SqlCommand(sqlCommand, con);
                InsertEmployeeCommand.Parameters.AddWithValue("@FirstName", employee.FirstName);
                InsertEmployeeCommand.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                InsertEmployeeCommand.Parameters.AddWithValue("@LastName", employee.LastName);
                InsertEmployeeCommand.Parameters.AddWithValue("@DOB", employee.DOB);
                InsertEmployeeCommand.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
                InsertEmployeeCommand.Parameters.AddWithValue("@Address", employee.Address);
                con.Open();
                InsertEmployeeCommand.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
        }
        public ActionResult AddTestingRecords()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "INSERT INTO Employee (FirstName, MiddleName, LastName, DOB, MobileNumber, Address) VALUES ('Peter', NULL, 'Parker', '2000-01-01', '9999999999', 'New York, USA'),('Tony', 'Edward', 'Stark', '1975-05-29', '8888888888', 'Los Angeles, USA'),('Bruce', NULL, 'Wayne', '1980-02-19', '7777777777', 'Gotham City'),('Clark', 'Joseph', 'Kent', '1985-06-18', '6666666666', 'Metropolis'),('Natasha', NULL, 'Romanoff', '1990-11-22', '5555555555', 'Moscow, Russia');";
                SqlCommand InsertTestingRecordsCommand = new SqlCommand(sqlCommand, con);
                con.Open();
                InsertTestingRecordsCommand.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateFirstNameForSingleRecord()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "update Employee set FirstName = 'SQLPerson' where Id=1";
                SqlCommand UpdateFirstNameForSingleRecordCommand = new SqlCommand(sqlCommand, con);
                con.Open();
                UpdateFirstNameForSingleRecordCommand.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateMiddleNameForAll()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "Update Employee set MiddleName = 'I'";
                SqlCommand UpdateMiddleNameCommand= new SqlCommand(sqlCommand, con);
                con.Open();
                UpdateMiddleNameCommand.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteRecord()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "Delete from Employee where Id<2";
                SqlCommand DeleteRecordCommand = new SqlCommand(sqlCommand, con);
                con.Open();
                DeleteRecordCommand.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteAllRecords()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlCommand = "Truncate table Employee";
                SqlCommand DeleteAllRecordCommand = new SqlCommand(sqlCommand, con);
                con.Open();
                DeleteAllRecordCommand.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
        }
    }
}