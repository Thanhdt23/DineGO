using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;
using DineGO_Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantOwnerController : Controller
    {
        private readonly IRestaurantOwnerRepository _restaurantOwnerRepository;
        public RestaurantOwnerController(IRestaurantOwnerRepository restaurantOwnerRepository )
        {
            _restaurantOwnerRepository = restaurantOwnerRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_restaurantOwnerRepository.GetRestaurantOwners());
        }
        [HttpGet("id")]
        public IActionResult GetOne(int Id)
        {
            return Ok(_restaurantOwnerRepository.FindRestaurantOwnerById(Id));
        }
        [HttpGet("cusId")]
        public IActionResult GetRestaurantOwnerByCusId(int Id)
        {
            return Ok(_restaurantOwnerRepository.FindRestaurantOwnersByCusId(Id));
        }

        [HttpPost]
        public IActionResult Addblog(RestaurantOwner restaurantOwner)
        {
            _restaurantOwnerRepository.SaveRestaurantOwner(restaurantOwner);
            return Ok(new { resOwner_id = restaurantOwner.resOwner_id});
        }
        [HttpPut]
        public IActionResult Updateblog(RestaurantOwner restaurantOwner)
        {
            _restaurantOwnerRepository.UpdateRestaurantOwner(restaurantOwner);
            return Ok(_restaurantOwnerRepository.GetRestaurantOwners());
        }
        [HttpDelete]
        public IActionResult Deleteblog(int Id)
        {
            _restaurantOwnerRepository.DeleteRestaurantOwner(Id);
            return Ok(_restaurantOwnerRepository.GetRestaurantOwners());
        }
    }
}