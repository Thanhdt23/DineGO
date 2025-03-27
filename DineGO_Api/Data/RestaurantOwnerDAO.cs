using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;
using Microsoft.EntityFrameworkCore;

namespace DineGO_Api.Data
{
    public class RestaurantOwnerDAO
    {
        private readonly ApplicationDbContext _context;

        public RestaurantOwnerDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all RestaurantOwners
        public List<RestaurantOwner> GetRestaurantOwners()
        {
            try
            {
                return _context.restaurantOwners.ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Error fetching Blogs: {e.Message}");
            }
        }

        // Get RestaurantOwner by ID
        public RestaurantOwner FindRestaurantOwnerById(int id)
        {
            try
            {
                return _context.restaurantOwners.SingleOrDefault(x => x.resOwner_id == id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error finding Blog: {e.Message}");
            }
        }

        // Get RestaurantOwner by customer ID
        public List<RestaurantOwner> FindRestaurantOwnersByCusId(int cusId)
        {
            try
            {
                return _context.restaurantOwners
                       .Where(x => x.cus_id == cusId)
                       .ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Error finding Blog: {e.Message}");
            }
        }

        // Save a new RestaurantOwner
        public void SaveRestaurantOwner(RestaurantOwner restaurantOwner)
        {
            try
            {
                _context.restaurantOwners.Add(restaurantOwner);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error saving Blog: {e.Message}");
            }
        }

        // Update RestaurantOwner details
        public void UpdateRestaurantOwner(RestaurantOwner restaurantOwner)
        {
            try
            {
                _context.Entry(restaurantOwner).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error updating Blog: {e.Message}");
            }
        }

        // Delete RestaurantOwner by ID
        public void DeleteRestaurantOwner(int id)
        {
            try
            {
                var restaurantOwner = _context.restaurantOwners.SingleOrDefault(x => x.resOwner_id == id);
                if (restaurantOwner != null)
                {
                    _context.restaurantOwners.Remove(restaurantOwner);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting restaurantOwner: {e.Message}");
            }
        }
        
    }
}
