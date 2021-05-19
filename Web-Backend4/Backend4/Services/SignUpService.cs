using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend4.Models.Controls;

namespace Backend4.Services
{
    public class SignUpService : ISignUpService
    {
        private readonly ILogger logger;
        private readonly List<User> users = new List<User>();
        public SignUpService(ILogger<IPasswordResetService> logger)
        {
            this.logger = logger;
        }
        class User
        {

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Day { get; set; }
            public Month Month { get; set; }
            public int Year { get; set; }
            public string Gender { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public User(string FirstName, string LastName, int Day, Month month, int Year, string Gender, string Email, string Password)
            {
                this.FirstName = FirstName;
                this.LastName = LastName;
                this.Day = Day;
                this.Month = month;
                this.Year = Year;
                this.Gender = Gender;
                this.Email = Email;
                this.Password = Password;

            }

        }
        public void AddUser(string FirstName, string LastName, int Day, Month Month, int Year, string Gender, string Email, string Password)
        {
            lock (this.users)
            {
                logger.LogInformation($"Adding new User {FirstName} {LastName} with Email{Email}");
                User user = new User(FirstName, LastName, Day, Month, Year, Gender, Email, Password);
                users.Add(user);
            }
        }


        public Boolean VerifyUser(string FirstName, string LastName, int Day, Month Month, int Year, string Gender)
        {
             
            this.logger.LogInformation($"Validating  user's name {FirstName}, last name {LastName}, birthday {Day} {Month} {Year} and gender {Gender}");
            return this.users.Any(x => x.FirstName == FirstName
                && x.LastName == LastName && x.Day == Day && x.Month.Id == Month.Id && x.Month.Name == Month.Name
                && x.Year == Year && x.Gender == Gender);
             

        }
        public void SentLogMessage(string st)
        {
            this.logger.LogInformation(st);
        }
    }
}
