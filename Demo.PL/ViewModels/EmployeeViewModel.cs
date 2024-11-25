using Deno.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "Max Length is 50 Char")]
        [MinLength(5, ErrorMessage = "Min Length is 5 Char ")]
        public string Name { get; set; }
        public IFormFile Image { get; set; }

        public string ImageName { get; set; }
        [Range(20, 35, ErrorMessage = "Age must be in 20 to 25")]
        public int Age { get; set; }
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
            , ErrorMessage = "Addreess Must Be Like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        //optinal =>  ondelete: restrict 
        //required => ondelete: CaseCade 
        [InverseProperty("empolyees")]
        public Department Department { get; set; }

    }
}
