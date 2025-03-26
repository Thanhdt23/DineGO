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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepositoy;
        public PaymentController(IPaymentRepository paymentRepositoy)
        {
            _paymentRepositoy = paymentRepositoy;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_paymentRepositoy.GetPayments());
        }
        [HttpGet("id")]
        public IActionResult GetOne(int ID)
        {
            return Ok(_paymentRepositoy.FindPaymentById(ID));
        }

        [HttpPost]
        public IActionResult AddPayment(Payment p)
        {
            _paymentRepositoy.SavePayment(p);
            return Ok(_paymentRepositoy.GetPayments());
        }
        [HttpPut]
        public IActionResult UpdatePayment(Payment p)
        {
            _paymentRepositoy.UpdatePayment(p);
            return Ok(_paymentRepositoy.GetPayments());
        }
        [HttpDelete]
        public IActionResult DeletePayment(int Id)
        {
            _paymentRepositoy.DeletePayment(Id);
            return Ok(_paymentRepositoy.GetPayments());
        }

        [HttpGet("cus_id")]
        public IActionResult GetPaymentsByCustomer(int cus_id)
        {
            var payments = _paymentRepositoy.GetByCusId(cus_id);

            return Ok(payments ?? new List<Payment>());

        }
    }
}