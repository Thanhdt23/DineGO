using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DineGO_Client.Model
{
    public class Reservation
    {
        [Key]
        public int reser_id { get; set; }

        [Required]
        public int cus_id { get; set; }

        [Required]
        public int res_id { get; set; }

        public DateTime reser_date { get; set; }

        [Range(1, 100)]
        public int reser_quantity { get; set; }

        [Required, MaxLength(50)]
        public string reser_status { get; set; }

        public string reser_note { get; set; }

        public string restaurantName { get; set; } 

        [ForeignKey("cus_id")]
        public Customer customer { get; set; }

        [ForeignKey("res_id")]
        public Restaurant restaurant { get; set; }

        public List<Payment> payments { get; set; }
    }
}