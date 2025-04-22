using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DineGO_Admin.Model
{
    public class RestaurantOwner
    {
        [Key]
        public int resOwner_id { get; set; }
        public int cus_id { get; set; }
        public string resOwner_name { get; set; }
        public DateTime resOwner_createdDate { get; set; }
        public bool resOwner_isAuthorize { get; set; }

        [ForeignKey("cus_id")]
        public Customer customer { get; set; }

        public List<Restaurant> restaurants { get; set; }
        public List<Blog> blogs { get; set; }
    }
}