using System.Collections.Generic;
using DineGO_Api.Data;
using DineGO_Api.Model;

namespace DineGO_Api.Repository
{
    public class RestaurantOwnerRepository : IRestaurantOwnerRepository
    {
        private readonly RestaurantOwnerDAO _restaurantOwnerDAO;

        public RestaurantOwnerRepository(RestaurantOwnerDAO restaurantOwnerDAO)
        {
            _restaurantOwnerDAO = restaurantOwnerDAO;
        }

        public List<RestaurantOwner> GetRestaurantOwners() => _restaurantOwnerDAO.GetRestaurantOwners();

        public RestaurantOwner FindRestaurantOwnerById(int id) => _restaurantOwnerDAO.FindRestaurantOwnerById(id);

        public void SaveRestaurantOwner(RestaurantOwner owner) => _restaurantOwnerDAO.SaveRestaurantOwner(owner);

        public void UpdateRestaurantOwner(RestaurantOwner owner) => _restaurantOwnerDAO.UpdateRestaurantOwner(owner);

        public void DeleteRestaurantOwner(int id) => _restaurantOwnerDAO.DeleteRestaurantOwner(id);
    }
}
