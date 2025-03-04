using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;
using Microsoft.EntityFrameworkCore;

namespace DineGO_Api.Data
{
    public class CategoryDAO
    {
        private readonly ApplicationDbContext _context;

        public CategoryDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all categorys
        public List<Category> GetCategories()
        {
            try
            {
                return _context.categories.ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Error fetching categorys: {e.Message}");
            }
        }

        // Get category by ID
        public Category FindCategoryById(int id)
        {
            try
            {
                return _context.categories.SingleOrDefault(x => x.cate_id == id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error finding category: {e.Message}");
            }
        }

        // Save a new category
        public void SaveCategory(Category category)
        {
            try
            {
                _context.categories.Add(category);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error saving category: {e.Message}");
            }
        }

        // Update category details
        public void UpdateCategory(Category category)
        {
            try
            {
                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error updating category: {e.Message}");
            }
        }

        // Delete category by ID
        public void DeleteCategory(int id)
        {
            try
            {
                var category = _context.categories.SingleOrDefault(x => x.cate_id == id);
                if (category != null)
                {
                    _context.categories.Remove(category);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting category: {e.Message}");
            }
        }
    }
}