using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace DineGO_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantOwnerController : ControllerBase
    {
        private readonly IRestaurantOwnerRepository _restaurantOwnerRepository;

        public RestaurantOwnerController(IRestaurantOwnerRepository restaurantOwnerRepository)
        {
            _restaurantOwnerRepository = restaurantOwnerRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_restaurantOwnerRepository.GetRestaurantOwners());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var owner = _restaurantOwnerRepository.FindRestaurantOwnerById(id);
            if (owner == null)
                return NotFound($"Restaurant Owner with ID {id} not found");
            return Ok(owner);
        }

        [HttpPost]
        public IActionResult AddRestaurantOwner(RestaurantOwner owner)
        {
            _restaurantOwnerRepository.SaveRestaurantOwner(owner);
            return CreatedAtAction(nameof(GetOne), new { id = owner.resOwner_id }, owner);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRestaurantOwner(int id, RestaurantOwner owner)
        {
            if (id != owner.resOwner_id)
                return BadRequest("Restaurant Owner ID mismatch");

            _restaurantOwnerRepository.UpdateRestaurantOwner(owner);
            return Ok(new { message = "RestaurantOwner updated successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRestaurantOwner(int id)
        {
            _restaurantOwnerRepository.DeleteRestaurantOwner(id);
            return Ok(_restaurantOwnerRepository.GetRestaurantOwners());
        }
    }
}