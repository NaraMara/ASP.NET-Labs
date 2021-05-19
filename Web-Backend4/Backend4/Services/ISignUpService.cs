using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend4.Models.Controls;
namespace Backend4.Services
{
    public interface ISignUpService
    {
        Boolean VerifyUser(string FirstName, string LastName, int Day, Month Month, int Year, string Gender);
        void AddUser(string FirstName, string LastName, int Day,  Month month, int Year, string Gender, string Email, string Password);
         void SentLogMessage(string st);
    }
}
