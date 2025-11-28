using Practical_12_Test3.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practical_12_Test3.Controllers
{
    public class EmployeeController : Controller
    {
        string conString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;
        public ActionResult Index()
        {
            List<EmployeeDetailsView> empDetailsList = new List<EmployeeDetailsView>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                string command = "select e.Id, e.FirstName, e.MiddleName, e.LastName, d.DesignationName, e.DOB, e.MobileNumber, e.Address, e.Salary from EmployeeTest3 e Join Designation d on e.DesignationId = d.Id";
                SqlCommand cmd = new SqlCommand(command, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empDetailsList.Add(new EmployeeDetailsView()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DesignationName = reader["DesignationName"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
            }
            return View(empDetailsList);
        }
        //Write a query to count the number of records by designation name
        public ActionResult CountRecordsByDesignation()
        {
            List<DesignationEmployeeCount> designationEmployeeCountList = new List<DesignationEmployeeCount>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                string command = "select d.DesignationName,count(DesignationName)as EmployeeCount from EmployeeTest3 e Join Designation d on e.DesignationId = d.Id group by d.DesignationName";
                SqlCommand cmd = new SqlCommand(command, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    designationEmployeeCountList.Add(new DesignationEmployeeCount()
                    {
                        DesignationName = reader["DesignationName"].ToString(),
                        EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
                    });
                }
            }
            return View(designationEmployeeCountList);
        }
        //Write a query to display First Name, Middle Name, Last Name & Designation name
        public ActionResult EmployeeDetails()
        {
            List<EmployeeDetail> empDetailList = new List<EmployeeDetail>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                string command = "select e.FirstName, e.MiddleName, e.LastName, d.DesignationName from EmployeeTest3 e Join Designation d on e.DesignationId = d.Id";
                SqlCommand cmd = new SqlCommand(command, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empDetailList.Add(new EmployeeDetail()
                    {
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DesignationName = reader["DesignationName"].ToString()
                    });
                }
                return View(empDetailList);
            }
        }
        //Create a database view that outputs Employee Id, First Name, Middle Name, Last Name, Designation, DOB, Mobile Number, Address & Salary
        public ActionResult EmployeeDetailsView()
        {
            List<EmployeeDetailsView> empDetailsViewList = new List<EmployeeDetailsView>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                string command = "select * from EmployeeView";
                SqlCommand cmd = new SqlCommand(command, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empDetailsViewList.Add(new EmployeeDetailsView()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DesignationName = reader["DesignationName"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
            }
            return View(empDetailsViewList);
        }

        // Create a stored procedure to insert data into the Designation table with required parameters
        public ActionResult AddDesignation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDesignation(string designationName)
        {
            if (string.IsNullOrWhiteSpace(designationName))
            {
                ViewBag.Error = "Designation name cannot be empty.";
                return View();
            }

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertDesignation", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Designation", designationName.Trim());
                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        //Create a stored procedure to insert data into the Employee table with required parameters 
        public ActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertEmployee", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@DOB", employee.DOB);
                cmd.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
                cmd.Parameters.AddWithValue("@Address", employee.Address);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                cmd.Parameters.AddWithValue("@DesignationId", employee.DesignationId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
        //  Write a query that displays only those designation names that have more than 1 employee
        public ActionResult CountEmployeeByDesignationGreaterThanOne()
        {
            List<DesignationEmployeeCount> designationEmployeeCountList = new List<DesignationEmployeeCount>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                string command = "select d.DesignationName,count(DesignationName)as EmployeeCount from EmployeeTest3 e Join Designation d on e.DesignationId = d.Id group by d.DesignationName having count(DesignationName) > 1";
                SqlCommand cmd = new SqlCommand(command, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    designationEmployeeCountList.Add(new DesignationEmployeeCount()
                    {
                        DesignationName = reader["DesignationName"].ToString(),
                        EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
                    });
                }
            }
            return View(designationEmployeeCountList);
        }
        //Create a stored procedure that returns a list of employees with columns Employee Id, First Name, Middle Name, Last Name, Designation, DOB, Mobile Number, Address & Salary (records should be ordered by DOB)
        public ActionResult GetEmployeeDetailsOrderByDOB()
        {
            List<EmployeeDetailsView> empDetailsViewList = new List<EmployeeDetailsView>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetEmployeeDetails", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empDetailsViewList.Add(new EmployeeDetailsView()
                    {
                        Id = Convert.ToInt32(reader["EmployeeId"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DesignationName = reader["DesignationName"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
            }
            return View(empDetailsViewList);
        }
        // Create a stored procedure that return a list of employees by designation id (Input) with columns Employee Id, First Name, Middle Name, Last Name, DOB, Mobile Number, Address & Salary (records should be ordered by First Name) 
        public ActionResult GetEmployeeDetailsOrderByFirstName()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetEmployeeDetailsOrderByFirstName(int designationId)
        {
            List<EmployeeDetailsView> empDetailsViewList = new List<EmployeeDetailsView>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("spGetEmployeesByDesignation", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DesignationId", designationId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empDetailsViewList.Add(new EmployeeDetailsView()
                    {
                        Id = Convert.ToInt32(reader["EmployeeId"]),
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
            return View(empDetailsViewList);
        }
        //     Write a query to find the employee having maximum salary - there can be multiple same salary so it should return list
        public ActionResult GetEmployeeWithMaximumSalary()
        {
            List<EmployeeDetailsView> empDetailsList = new List<EmployeeDetailsView>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                string command = "select e.Id, e.FirstName, e.MiddleName, e.LastName, d.DesignationName, e.DOB, e.MobileNumber, e.Address, e.Salary from EmployeeTest3 e Join Designation d on e.DesignationId = d.Id where e.Salary = (select max(Salary) from EmployeeTest3)";
                SqlCommand cmd = new SqlCommand(command, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empDetailsList.Add(new EmployeeDetailsView()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DesignationName = reader["DesignationName"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
            }
            return View(empDetailsList);
        }
    }
}