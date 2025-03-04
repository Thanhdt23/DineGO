using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;
using DineGO_Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DineGO_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categorysRepository;

        public CategoryController(ICategoryRepository categorysRepository)
        {
            _categorysRepository = categorysRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_categorysRepository.GetCategories());
        }
        [HttpGet("id")]
        public IActionResult GetOne(int ID)
        {
            return Ok(_categorysRepository.FindCategoryById(ID));
        }

        [HttpPost]
        public IActionResult AddCategorys(Category p)
        {
            _categorysRepository.SaveCategory(p);
            return Ok(_categorysRepository.GetCategories());
        }
        [HttpPut]
        public IActionResult UpdateCategorys(Category p)
        {
            _categorysRepository.UpdateCategory(p);
            return Ok(_categorysRepository.GetCategories());
        }
        [HttpDelete]
        public IActionResult DeleteCategorys(int Id)
        {
            _categorysRepository.DeleteCategory(Id);
            return Ok(_categorysRepository.GetCategories());
        }

    }
}