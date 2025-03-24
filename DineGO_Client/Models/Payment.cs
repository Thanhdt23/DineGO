using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DineGO_Client.Model
{
    public class Payment
    {
        [Key]
        public int pay_id { get; set; }
        public int cus_id { get; set; }
        public int reser_id { get; set; }
        public double pay_price { get; set; }
        public string pay_status { get; set; }
        public DateTime pay_createdDate { get; set; }

        [ForeignKey("cus_id")]
        public Customer customer { get; set; }

        [ForeignKey("reser_id")]
        public Reservation reservation { get; set; }
    }
}