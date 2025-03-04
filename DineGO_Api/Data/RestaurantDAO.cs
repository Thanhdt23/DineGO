using System;
using System.Collections.Generic;
using System.Linq;
using DineGO_Api.Model;
using Microsoft.EntityFrameworkCore;

namespace DineGO_Api.Data
{
    public class RestaurantDAO
    {
        private readonly ApplicationDbContext _context;

        public RestaurantDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all restaurants
        public List<Restaurant> GetRestaurants()
        {
            try
            {
                return _context.restaurants.ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Error fetching restaurants: {e.Message}");
            }
        }

        // Get restaurant by ID
        public Restaurant FindRestaurantById(int id)
        {
            try
            {
                return _context.restaurants.SingleOrDefault(x => x.res_id == id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error finding restaurant: {e.Message}");
            }
        }

        // Save a new restaurant
        public void SaveRestaurant(Restaurant restaurant)
        {
            try
            {
                _context.restaurants.Add(restaurant);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error saving restaurant: {e.Message}");
            }
        }

        // Update restaurant details
        public void UpdateRestaurant(Restaurant restaurant)
        {
            try
            {
                _context.Entry(restaurant).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error updating restaurant: {e.Message}");
            }
        }

        // Delete restaurant by ID
        public void DeleteRestaurant(int id)
        {
            try
            {
                var restaurant = _context.restaurants.SingleOrDefault(x => x.res_id == id);
                if (restaurant != null)
                {
                    _context.restaurants.Remove(restaurant);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting restaurant: {e.Message}");
            }
        }
    }
}
