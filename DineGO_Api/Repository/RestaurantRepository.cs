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
    }
}