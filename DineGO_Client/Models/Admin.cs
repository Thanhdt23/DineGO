using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DineGO_Api.Model
{
    public class Admin
    {
        [Key]
        public int ad_id { get; set; }

        [Required, MaxLength(50)]
        public string ad_username { get; set; }

        [Required, MaxLength(100)]
        public string ad_password { get; set; }

        [Required, MaxLength(100)]
        public string ad_name { get; set; }

        [Required, EmailAddress]
        public string ad_email { get; set; }

        public DateTime ad_birthday { get; set; }

        public string ad_image { get; set; }
    }
}
