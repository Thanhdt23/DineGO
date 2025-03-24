using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DineGO_Client.Model
{
    public class Restaurant
    {
        [Key]
        public int res_id { get; set; }
        [Required]
        public int cate_id { get; set; } 
        [Required]
        public int resOwner_id { get; set; } 

        [Required, MaxLength(100)]
        public string res_name { get; set; }

        [MaxLength(200)]
        public string res_address { get; set; }

        [Phone]
        public string res_phone { get; set; }

        public string res_information { get; set; }

        public decimal res_rate { get; set; }

        public decimal res_price { get; set; }

        public decimal res_discount { get; set; }

        public List<string> res_images { get; set; } = new List<string>();

        [ForeignKey("cate_id")]
        public virtual Category category { get; set; }
        [ForeignKey("resOwner_id")]
        public virtual RestaurantOwner restaurantOwner { get; set; }

        public List<Reservation> reservations { get; set; }
    }
}