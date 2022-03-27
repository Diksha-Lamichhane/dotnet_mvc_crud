using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_crud_mvc.Models
{
    public class Customers
    {
        [Key]
        public int Customer_ID { get; set; }

        [Required(ErrorMessage = "Name is required..!!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Address is required..!!")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone is required..!!")]
        public string? Phone { get; set; }
    }
}
