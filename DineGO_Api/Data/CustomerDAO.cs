using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DineGO_Api.Data;
using DineGO_Api.Model;

public class CustomerDAO
{
    private readonly ApplicationDbContext _context;

    public CustomerDAO(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all customers
    public List<Customer> GetCustomers()
    {
        try
        {
            return _context.customers.ToList();
        }
        catch (Exception e)
        {
            throw new Exception($"Error fetching customers: {e.Message}");
        }
    }

    // Get customer by ID
    public Customer FindCustomerById(int id)
    {
        try
        {
            return _context.customers.SingleOrDefault(x => x.cus_id == id);
        }
        catch (Exception e)
        {
            throw new Exception($"Error finding customer: {e.Message}");
        }
    }

    // Save a new customer
    public void SaveCustomer(Customer customer)
    {
        try
        {
            _context.customers.Add(customer);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception($"Error saving customer: {e.Message}");
        }
    }

    // Update customer details
    public void UpdateCustomer(Customer customer)
    {
        try
        {
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception($"Error updating customer: {e.Message}");
        }
    }

    // Delete customer by ID
    public void DeleteCustomer(int id)
    {
        try
        {
            var customer = _context.customers.SingleOrDefault(x => x.cus_id == id);
            if (customer != null)
            {
                _context.customers.Remove(customer);
                _context.SaveChanges();
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error deleting customer: {e.Message}");
        }
    }
}
