using System;
using System.Collections.Generic;
using DineGO_Api.Data;
using DineGO_Api.Model;

namespace DineGO_Api.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDAO _customerDAO;
        public CustomerRepository(CustomerDAO customerDAO)
        {
            _customerDAO = customerDAO;
        }
        public List<Customer> GetCustomers() => _customerDAO.GetCustomers();
        public Customer FindCustomerById(int id) => _customerDAO.FindCustomerById(id);
        public void SaveCustomer(Customer c) => _customerDAO.SaveCustomer(c);
        public void UpdateCustomer(Customer c) => _customerDAO.UpdateCustomer(c);
        public void DeleteCustomer(int id) => _customerDAO.DeleteCustomer(id);
    }
}
