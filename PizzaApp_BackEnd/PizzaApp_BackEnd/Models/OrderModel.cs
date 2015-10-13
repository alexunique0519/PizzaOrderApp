using ServerSideValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp_BackEnd.Models
{
    public class OrderModel
    {
        [Required]
        public string customerName {get; set;}

        [Required]
        //server side postalcode validation
        [PostalCodeValidation]
        public string postalCode {get; set;}
        public string city {get; set;}

        [Required]
        [ProvinceValidation]
        public string province {get; set;}

        [Required]
        [PhoneNumberValidation]
        public string telephoneNumber{get; set;}
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email {get; set;}

        [Required]
        public string pizzaSize {get; set;}
        
        [Required]
        public string[] toppings {get; set;}
        
        [Required]
        public string crustType {get; set;}
    
    }

    public class OrderItem
    {
        public string itemName;
        public int quantity;
        public string cost;
    }

    public class OrderedResult
    {
        public string message;
        public string subTotal;
        public string totalCost;
        public OrderItem[] orderedItems;
    }

}
