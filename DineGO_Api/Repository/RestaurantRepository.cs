using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Data;
using DineGO_Api.Model;

namespace DineGO_Api.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantDAO _restaurantDAO;
        public RestaurantRepository(RestaurantDAO restaurantDAO)
        {
            _restaurantDAO = restaurantDAO;
        }
        public List<Restaurant> GetRestaurants() => _restaurantDAO.GetRestaurants();
        public Restaurant FindRestaurantById(int Id) => _restaurantDAO.FindRestaurantById(Id);
        public void SaveRestaurant(Restaurant p) => _restaurantDAO.SaveRestaurant(p);
        public void UpdateRestaurant(Restaurant p) => _restaurantDAO.UpdateRestaurant(p);
        public void DeleteRestaurant(int Id) => _restaurantDAO.DeleteRestaurant(Id);

        public List<Restaurant> SearchRestaurants(string name, string address)
        {
            var restaurants = _restaurantDAO.GetRestaurants();

            if (!string.IsNullOrEmpty(name))
            {
                restaurants = restaurants.Where(r => r.res_name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(address))
            {
                restaurants = restaurants.Where(r => r.res_address.Contains(address, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return restaurants;
        }


    }
}