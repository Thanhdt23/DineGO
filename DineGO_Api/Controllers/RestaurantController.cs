using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;
using DineGO_Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantsRepository;

        public RestaurantController(IRestaurantRepository restaurantsRepository)
        {
            _restaurantsRepository = restaurantsRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_restaurantsRepository.GetRestaurants());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var restaurant = _restaurantsRepository.FindRestaurantById(id);
            if (restaurant == null)
            {
                return NotFound($"Restaurant with ID {id} not found.");
            }
            return Ok(restaurant);
        }


        [HttpPost]
        public IActionResult AddRestaurants(Restaurant p)
        {
            _restaurantsRepository.SaveRestaurant(p);
            // return Ok(_restaurantsRepository.GetRestaurants());
            return Ok(new { res_id = p.res_id});
        }
        [HttpPut]
        public IActionResult UpdateRestaurants(Restaurant p)
        {
            _restaurantsRepository.UpdateRestaurant(p);
            return Ok(_restaurantsRepository.GetRestaurants());
        }
        [HttpDelete]
        public IActionResult DeleteRestaurants(int Id)
        {
            _restaurantsRepository.DeleteRestaurant(Id);
            return Ok(_restaurantsRepository.GetRestaurants());
        }

        [HttpGet("search")]
        public IActionResult SearchRestaurants(string name, string address)
        {
            var result = _restaurantsRepository.SearchRestaurants(name, address);
            return Ok(result);
        }

    }
}