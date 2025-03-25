using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineGO_Api.Repository
{
    public interface IMailSenderRepository
    {
        public void SendOTP(string mailSender);
        void SendMail(string mailSender, string subject, Func<string> bodyGenerator);
    }
}