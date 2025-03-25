using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;
using Microsoft.EntityFrameworkCore;

namespace DineGO_Api.Data
{
    public class CustomerDAO
    {
        private readonly ApplicationDbContext _context;

        public CustomerDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all Customers
        public List<Customer> GetCustomers()
        {
            try
            {
                return _context.customers.ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Error fetching Customers: {e.Message}");
            }
        }

        // Get Customer by ID
        public Customer FindCustomerById(int id)
        {
            try
            {
                return _context.customers.SingleOrDefault(x => x.cus_id == id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error finding Customer: {e.Message}");
            }
        }

        // Save a new Customer
        public void SaveCustomer(Customer Customer)
        {
            try
            {
                _context.customers.Add(Customer);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error saving Customer: {e.Message}");
            }
        }

        // Update Customer details
        public void UpdateCustomer(Customer Customer)
        {
            try
            {
                _context.Entry(Customer).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error updating Customer: {e.Message}");
            }
        }

        // Delete Customer by ID
        public void DeleteCustomer(int id)
        {
            try
            {
                var Customer = _context.customers.SingleOrDefault(x => x.cus_id == id);
                if (Customer != null)
                {
                    _context.customers.Remove(Customer);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting Customer: {e.Message}");
            }
        }

        public Customer ChangePassword(string email, string newPassword)
        {
            try
            {
                var customer = _context.customers.SingleOrDefault(c => c.cus_email == email);
                if (customer != null)
                {
                    customer.cus_password = newPassword; // Nên hash mật khẩu trước khi lưu
                    _context.SaveChanges();
                    return customer;
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception($"Error changing password: {e.Message}");
            }
        }

    }
}