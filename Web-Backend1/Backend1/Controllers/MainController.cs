using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend1.Models;
using Backend1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend1.Controllers
{
    public class MainController: Controller
    {

        private readonly IRandomNumberService randomNumberService;
        private readonly IOperatorsService operatorsService;
        

        public MainController(IRandomNumberService randomNumberService, IOperatorsService operatorsService)
        {
            this.randomNumberService = randomNumberService;
            this.operatorsService = operatorsService;
        }
        public ActionResult PassUsingViewData()
        {
            var first  = this.randomNumberService.GiveRandomNumber(0,5000);
            var second= this.randomNumberService.GiveRandomNumber(0, 5000);
            this.ViewData["First"] =first ;
            this.ViewData["Second"] = second;
            this.ViewData["Sum"] = operatorsService.Sum(first,second);
            this.ViewData["Substruct"] = operatorsService.Subtract(first,second);
            this.ViewData["Divide"] = operatorsService.Divide(first,second);
            this.ViewData["Multyply"] = operatorsService.Multiply(first,second);
            return this.View();
        }

        public ActionResult PassUsingViewBag()
        {
            var first = this.randomNumberService.GiveRandomNumber(0, 5000);
            var second = this.randomNumberService.GiveRandomNumber(0, 5000);
             
            this.ViewBag.first = first;
            this.ViewBag.second = second;
            this.ViewBag.Sum= operatorsService.Sum(first,second);
            this.ViewBag.Substruct = operatorsService.Subtract(first, second);
            this.ViewBag.Divide= operatorsService.Divide(first, second);
            this.ViewBag.Multyply = operatorsService.Multiply(first, second);


            return this.View();
        }

        public ActionResult PassUsingModel()
        {
            var first = randomNumberService.GiveRandomNumber(0, 5000);
            var second = randomNumberService.GiveRandomNumber(0, 5000);

            var model = new NumbersModel
            {
               FirstNumber=first,
               SecondNumber=second,
               Sum=operatorsService.Sum(first,second),
               Substruct=operatorsService.Subtract(first,second),
               Divide=operatorsService.Divide(first,second),
               Multyply=operatorsService.Multiply(first,second)
            };

            return this.View(model);
        }

        public ActionResult AccessServiceDirectly()
        {
            randomNumberService.SetRandomNumbers(0, 5000);
            return this.View();
        }
    }
}
