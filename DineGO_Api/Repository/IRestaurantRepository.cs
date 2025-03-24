using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;

namespace DineGO_Api.Repository
{
    public interface IRestaurantRepository 
    {
        List<Restaurant> GetRestaurants();
        Restaurant FindRestaurantById(int ID);
        void SaveRestaurant(Restaurant p);
        void UpdateRestaurant(Restaurant p);
        void DeleteRestaurant(int p);

        List<Restaurant> SearchRestaurants(string name, string address);

    }
}