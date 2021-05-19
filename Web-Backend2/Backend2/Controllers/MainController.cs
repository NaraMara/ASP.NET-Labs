using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend2.Models;
using Backend2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend2.Controllers
{
    public class MainController: Controller
    {
        private readonly ICalculateService calculateService;

        public MainController(ICalculateService calculateService)
        {
            this.calculateService = calculateService;
        }

        public ActionResult Manual()
        {
            if (this.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                 
                String first =  this.Request.Form["First"] ;
                String second =  this.Request.Form["Second"] ;
                if (String.IsNullOrEmpty(first)|| String.IsNullOrEmpty(second))
                {
                    this.ViewBag.Error = "numbers are invalid ";
                    this.ViewBag.ErrorType = 1;
                    if (String.IsNullOrEmpty(first)) this.ViewBag.flag1 = true; else { this.ViewBag.flag1 = false; this.ViewBag.value1 = int.TryParse(first, out int resultFirst) ? resultFirst : 0; }
                    if (String.IsNullOrEmpty(second)) this.ViewBag.flag2 = true; else {this.ViewBag.flag2 = false; this.ViewBag.value2 = int.TryParse(second, out int resultSecond) ? resultSecond : 0; }

                    
                    

                    return this.View();
                }
                if (second == "0" && this.Request.Form["Operator"]=="/")
                {
                    this.ViewBag.Error = "division by zero ";
                    this.ViewBag.ErorType = 0;
                    this.ViewBag.value1 = int.Parse(first);
                    this.ViewBag.value2 = int.Parse(second);
                    return this.View();

                }
                int f=this.ViewBag.value1 = int.Parse(first);
                int s=this.ViewBag.value2 = int.Parse(second);
                this.ViewBag.Operator = this.Request.Form["Operator"];
                this.ViewBag.Result = this.calculateService.Calculate(f, s, this.Request.Form["Operator"]);

            }
            return this.View();
        }
        
        public ActionResult ManualWithSeparateHandlers() 
        {

            return this.View();
        }
        [HttpPost,ActionName("ManualWithSeparateHandlers")]
        [ValidateAntiForgeryToken]
        public ActionResult ManualWithSeparateHandlersConfirm()
        {
             
            String first = this.Request.Form["First"];
            String second = this.Request.Form["Second"];
            if (String.IsNullOrEmpty(first) || String.IsNullOrEmpty(second))
            {
                this.ViewBag.Error = "numbers are invalid ";
                this.ViewBag.ErrorType = 1;
                if (String.IsNullOrEmpty(first)) this.ViewBag.flag1 = true; else { this.ViewBag.flag1 = false; this.ViewBag.value1 = int.TryParse(first, out int resultFirst) ? resultFirst : 0; }
                if (String.IsNullOrEmpty(second)) this.ViewBag.flag2 = true; else { this.ViewBag.flag2 = false; this.ViewBag.value2 = int.TryParse(second, out int resultSecond) ? resultSecond : 0; }

                return this.View();
            }
            if (second == "0" && this.Request.Form["Operator"] == "/")
            {
                this.ViewBag.Error = "division by zero ";
                this.ViewBag.ErorType = 0;
                this.ViewBag.value1= int.Parse(first);
                this.ViewBag.value2= int.Parse(second);
                return this.View();

            }
            int f = this.ViewBag.value1 = int.Parse(first);
            int s = this.ViewBag.value2 = int.Parse(second);
            this.ViewBag.Result = this.calculateService.Calculate(f, s, this.Request.Form["Operator"]);
            return this.View();
        }
        public ActionResult ModelBindingInParameters()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModelBindingInParameters(String  First, String Second, String Operator)
        {
            if (String.IsNullOrEmpty(First) || String.IsNullOrEmpty(Second))
            {
                this.ViewBag.Error = "numbers are invalid ";
                this.ViewBag.ErrorType = 1;
                if (String.IsNullOrEmpty(First)) this.ViewBag.flag1 = true; else { this.ViewBag.flag1 = false;}
                if (String.IsNullOrEmpty(Second)) this.ViewBag.flag2 = true; else { this.ViewBag.flag2 = false;}
                return this.View();
            }
            if (Second == "0" && Operator=="/")
            {
                this.ViewBag.Error = "division by zero ";
                this.ViewBag.ErorType = 0;
                 

                return this.View(new MainViewModel{
                    First = int.TryParse(First, out int cum) ? cum : 0,
                    Second = int.TryParse(Second, out int cum2) ? cum2 : 0,
                    Operator = Operator
                });

            }
            
            int f = int.TryParse(First,out int resultFirst)? resultFirst : 0;
            int s = int.TryParse(Second, out int resultSecond)? resultSecond : 0;
            
            var resultModel = new MainViewModel
            {
                First = f,
                Second = s,
                Operator = Operator,
                
                Result=calculateService.Calculate(f,s,Operator)
            };

            return this.View(resultModel);

        }
        public ActionResult ModelBindingInSeparateModel()
        {
            return this.View(new MainViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModelBindingInSeparateModel(MainViewModel model)
        {
            if (this.ModelState.IsValid)
            {

                if (model.Second == 0 && model.Operator == "/")
                {
                    this.ModelState.AddModelError("Second", "Division by zero is forbidden");
                    return this.View(model);
                }
            model.Result = calculateService.Calculate(model.First, model.Second, model.Operator);
            return this.View(model);
            }
            return this.View(model);
        }
    }
}
