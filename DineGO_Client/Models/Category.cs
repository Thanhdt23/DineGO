using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DineGO_Client.Model
{
    public class Category
    {
        [Key]
        public int cate_id { get; set; }

        [Required, MaxLength(50)]
        public string cate_type { get; set; }

        public string cate_description { get; set; }

        public List<Restaurant> restaurants { get; set; }
    }
}