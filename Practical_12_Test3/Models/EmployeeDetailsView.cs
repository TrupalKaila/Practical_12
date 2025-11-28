using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practical_12_Test3.Models
{
    //  Model include Id, First Name, Middle Name, Last Name, Designation, DOB, Mobile Number, Address & Salary
    public class EmployeeDetailsView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DesignationName { get; set; }
        public DateTime DOB { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
    }
}