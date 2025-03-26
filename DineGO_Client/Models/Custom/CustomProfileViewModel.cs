using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Client.Model;

namespace DineGO_Client.Models.Custom
{
    public class CustomProfileViewModel
    {
        public Customer Customer { get; set; }
        public List<RestaurantOwner> RestaurantOwners { get; set; }
        public List<Reservation> Reservations{ get; set; }
        public List<Payment> Payments{ get; set; }
    }
}