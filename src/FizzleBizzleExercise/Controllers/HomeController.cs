using System;
using Microsoft.AspNetCore.Mvc;

namespace FizzleBizzleExercise.Controllers
{
    //Implementing the interface as well as the controller
    public class HomeController : Controller, Interface
    {
        //Implementing the values from the interface
        public int Fizz { set; get; }
        public int Buzz { set; get; }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        } 
        //Post Method to pull the form from the front end
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FizzBuzzHandler(int start, int end, int fizz, int buzz, bool onoffswitch, string predchoice, int bazzval)
        {
            this.Fizz = fizz;
            this.Buzz = buzz;
            Predicate<int> bazz;
            //Error catch for dividing by zero
            if (fizz == 0 || buzz == 0)
            {
                ViewBag.error = "Fizz and Buzz Values cannot be Zero!";
                return View("Index");
            }
            //Simple check if they implemented the bazz switch
            if (predchoice != null)
            {
                //Switch statement to assign the correct predicate
                switch (predchoice)
                {
                    case "gt":
                        bazz = delegate (int a) { return a > bazzval; };
                        break;
                    case "lt":
                        bazz = delegate (int a) { return a < bazzval; };
                        break;
                    case "gte":
                        bazz = delegate (int a) { return a >= bazzval; };
                        break;
                    case "lte":
                        bazz = delegate (int a) { return a <= bazzval; };
                        break;
                    default:
                        throw new Exception("Not a valid predicate");
                }
                //Method call for the bazz implementation
                //Assigning it to an array 
                ViewBag.results = FizzBuzzBazz(start, end, bazz);
            }
            else
            {
                //Method call for the implementation without bazz
                ViewBag.results = FizzBuzz(start, end);
            }
            //Parsing the array and putting into a message that can be passed to front end
            ViewBag.message = string.Join(", ", ViewBag.results);
            return View("Index");
        }
        //method implementation w/o bazz
        public string[] FizzBuzz(int start, int end)
        {
            int counter = 0;
            string[] results = new string[(end - start) + 1];
            //loop through array
            for (int i = start; i < end + 1; i++)
            {
                //initialize empty string in element of array
                results[counter] = "";
                if (i % Fizz == 0)
                {
                    //append fizz
                    results[counter] += "fizz";
                }
                if (i % Buzz == 0)
                {
                    //append buzz
                    results[counter] += "buzz";
                }
                //if neither was appended assign the number as a string
                if (results[counter].Length == 0)
                {
                    results[counter] += i.ToString();
                }
                counter++;
            }
            //return array
            return results;
        }
        //method implementation w/ bazz
        public string[] FizzBuzzBazz(int start, int end, Predicate<int> bazz)
        {
            int counter = 0;
            string[] results = new string[(end - start) + 1];
            //loop through array
            for (int i = start; i < end + 1; i++)
            {
                //initialize empty string in element of array
                results[counter] = "";
                if (i % Fizz == 0)
                {
                    //append fizz
                    results[counter] = "Fizz";
                }
                if (i % Buzz == 0)
                {
                    //append buzz
                    results[counter] += "Buzz";
                    //if the predicate is true and the other statements are true append bazz
                    if (bazz(i) && results[counter] == "FizzBuzz")
                    {
                        results[counter] += "Bazz";
                    }
                }
                //if nothing is appended add number as a string
                if (results[counter].Length == 0)
                {
                    results[counter] += i.ToString();
                }
                counter++;
            }
            return results;
        }
    }
}
