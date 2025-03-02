using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;

namespace DineGO_Api.Repository
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();
        Category FindCategoryById(int ID);
        void SaveCategory(Category p);
        void UpdateCategory(Category p);
        void DeleteCategory(int p);
    }
}