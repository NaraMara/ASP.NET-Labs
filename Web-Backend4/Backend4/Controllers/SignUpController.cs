using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend4.Models.SignUp;
using Backend4.Services;
using Backend4.Controllers;
namespace Backend4.Controllers
{
    public class SignUpController : Controller
    {
        private readonly ISignUpService signUpService;
        public SignUpController(ISignUpService signUpService)
        {
            this.signUpService = signUpService;
        }
        public IActionResult SignUp()
        {
            ControlsController controlsController = new ControlsController();
           

            this.ViewBag.AllMonths = controlsController.GetAllMonths();

            SignUpFirstPageViewModel model = new SignUpFirstPageViewModel();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp( SignUpFirstPageViewModel model)
        {
            ControlsController controlsController = new ControlsController();
            this.ViewBag.AllMonths = controlsController.GetAllMonths();
           if (!this.ModelState.IsValid)
            {
                 
                return this.View();
            }

            if (signUpService.VerifyUser(model.FirstName,model.LastName, model.Day, model.Month, model.Year, model.Gender) )
            {
                 
                return this.View("SignUpAlreadyExists", model);
            }
            else
            {
                return this.View("SignUpCredentials", new SignUpSecondPageViewModel
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Month=model.Month,
                    Day=model.Day,
                    Year=model.Year,
                    Gender=model.Gender
                }) ;
            }
        
            

             
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpCredentials(SignUpSecondPageViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                 
                return this.View(model);
            }
            ControlsController controlsController = new ControlsController();
            this.ViewBag.AllMonths = controlsController.GetAllMonths();
            signUpService.AddUser(model.FirstName,model.LastName, model.Day, model.Month, model.Year,model.Gender,model.Email,model.Password);

            return this.View("SignUpResult", model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpAlreadyExists(SignUpFirstPageViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            ControlsController controlsController = new ControlsController();
            this.ViewBag.AllMonths = controlsController.GetAllMonths();
             
           

            return this.View("SignUpCredentials", new SignUpSecondPageViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Day = model.Day,
                Month=model.Month,
                Year = model.Year,
                Gender = model.Gender,
                IsEverExisted = true
            }) ;

        }



    }
}
