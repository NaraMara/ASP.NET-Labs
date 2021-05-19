using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend3.Models;
using Backend3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend3.Controllers
{
    public class QuizController : Controller
    {

        private readonly ICalculateService calculateService;
        private readonly IRandomNumberService randomNumberService;
        public QuizController(ICalculateService calculateService, IRandomNumberService randomNumberService)
        {
            this.calculateService = calculateService;
            this.randomNumberService = randomNumberService;
        }
        public IActionResult Quiz()
        {

            var model = new QuizModel();
            model.NewQuestion = MakeNewQuestion();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Quiz(CounterAction action, QuizModel model)
        {


            this.ValidateCounter(model);
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            switch (action)
            {
                case CounterAction.Submit:
                    model.CurrentCount++;
                    model.Questions.Add(model.NewQuestion);
                    model.NewQuestion = MakeNewQuestion();
                    this.ModelState.Remove("CurrentCount");
                    this.ModelState.Remove("NewQuestion.FirstNumber");
                    this.ModelState.Remove("NewQuestion.SecondNumber");
                    this.ModelState.Remove("NewQuestion.Operator");

                    return this.View(model);

                case CounterAction.Finish:

                    model.Questions.Add(model.NewQuestion);
                    var i = 0;
                    foreach (var question in model.Questions)
                    {
                        if (
                           this.calculateService.Calculate(
                           question.FirstNumber,
                           question.SecondNumber,
                           question.Operator) == question.Answer
                           )

                            question.isCorrect = true;
                        else
                            question.isCorrect = false;
                        if (question.isCorrect) i++;
                    }
                    model.RightAnswCount = i;
                    return this.View("QuizResult", model);
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
        private Question MakeNewQuestion()
        {
            int f = this.randomNumberService.GiveRandomNumberInRange(0, 100);
            int s = this.randomNumberService.GiveRandomNumberInRange(0, 100);
            string o = this.randomNumberService.GiveRandomOperator();
            if (o == "/" && s == 0)
            {
                while (s == 0)
                {
                    s = this.randomNumberService.GiveRandomNumberInRange(0, 100);
                }
            }
            Question q = new Question
            {
                FirstNumber = f,
                SecondNumber = s,
                Operator = o

            };
            return q;
        }
        private void ValidateCounter(QuizModel model)
        {
            var expectedCount = model.CurrentCount;
            var actualCount = model.Questions.Count;
            if (expectedCount != actualCount)
            {
                this.ModelState.AddModelError("", "Counter state is invalid");
            }
        }
    }
}
