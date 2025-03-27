using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;
using DineGO_Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DineGO_Api.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_reservationRepository.GetReservations());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var reservation = _reservationRepository.FindReservationById(id);
            if (reservation == null)
                return NotFound($"Reservation with ID {id} not found");
            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult AddReservation(Reservation reservation)
        {
            _reservationRepository.SaveReservation(reservation);
            return CreatedAtAction(nameof(GetOne), new { id = reservation.res_id }, reservation);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, Reservation reservation)
        {
            if (id != reservation.reser_id)
                return BadRequest("Reservation ID mismatch");

            _reservationRepository.UpdateReservation(reservation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            _reservationRepository.DeleteReservation(id);
            return NoContent();
        }

        [HttpGet("cus_id")]
        public IActionResult GetReservationsWithRestaurantName(int cus_id)
        {
            return Ok(_reservationRepository.GetResByCusId(cus_id));
        }

        [HttpGet("res_id")]
        public IActionResult GetReservationByRestaurant(int res_id)
        {
            var reservations = _reservationRepository.GetResByResId(res_id);

            return Ok(reservations ?? new List<Reservation>());
        }
    }
}
