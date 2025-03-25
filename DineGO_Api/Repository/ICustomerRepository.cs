using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;

public interface ICustomerRepository
{
    List<Customer> GetCustomers();
    Customer FindCustomerById(int ID);
    void SaveCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int customerId);
}