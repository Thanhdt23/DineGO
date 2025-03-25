using System;
using System.Collections.Generic;
using System.Linq;
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

        // Get all restaurant owners
        public List<RestaurantOwner> GetRestaurantOwners()
        {
            try
            {
                return _context.restaurantOwners.ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Error fetching restaurant owners: {e.Message}");
            }
        }

        // Get restaurant owner by ID
        public RestaurantOwner FindRestaurantOwnerById(int id)
        {
            try
            {
                return _context.restaurantOwners.SingleOrDefault(x => x.resOwner_id == id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error finding restaurant owner: {e.Message}");
            }
        }

        // Save a new restaurant owner
        public void SaveRestaurantOwner(RestaurantOwner owner)
        {
            try
            {
                _context.restaurantOwners.Add(owner);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error saving restaurant owner: {e.Message}");
            }
        }

        // Update restaurant owner details
        public void UpdateRestaurantOwner(RestaurantOwner owner)
        {
            try
            {
                _context.Entry(owner).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error updating restaurant owner: {e.Message}");
            }
        }

        // Delete restaurant owner by ID
        public void DeleteRestaurantOwner(int id)
        {
            try
            {
                var owner = _context.restaurantOwners.SingleOrDefault(x => x.resOwner_id == id);
                if (owner != null)
                {
                    _context.restaurantOwners.Remove(owner);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting restaurant owner: {e.Message}");
            }
        }
    }
}
