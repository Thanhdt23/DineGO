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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_customerRepository.GetCustomers());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var customer = _customerRepository.FindCustomerById(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found");
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            _customerRepository.SaveCustomer(customer);
            return CreatedAtAction(nameof(GetOne), new { id = customer.cus_id }, customer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.cus_id)
                return BadRequest("Customer ID mismatch");

            _customerRepository.UpdateCustomer(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            _customerRepository.DeleteCustomer(id);
            return NoContent();
        }
    }
}
