using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DineGO_Api.Model
{
    public class Customer
    {
        [Key]
        public int cus_id { get; set; }

        [Required, MaxLength(50)]
        public string cus_username { get; set; }

        [Required, MaxLength(100)]
        public string cus_password { get; set; }

        [Required, MaxLength(100)]
        public string cus_name { get; set; }

        [Required, EmailAddress]
        public string cus_email { get; set; }

        [Phone]
        public string cus_phone { get; set; }

        [MaxLength(200)]
        public string cus_address { get; set; }

        public DateTime cus_birthday { get; set; }

        [Required]
        public string cus_gender { get; set; }

        public string cus_image { get; set; }
        public bool cus_isKYI { get; set; }

        public List<Reservation> reservations { get; set; }
        public List<Payment> payments { get; set; }
        public List<Notification> notifications { get; set; }
        public List<RestaurantOwner> restaurantOwners { get; set; }
    }
}