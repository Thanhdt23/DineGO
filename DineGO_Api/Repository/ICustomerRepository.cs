using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;

namespace DineGO_Api.Repository
{
    public interface ICustomerRepository
    {
        public Customer IsMailExist(string email);
        public Customer ChangPassword(string email,string newpassword);
    }
}