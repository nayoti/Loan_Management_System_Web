using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Models;
using Scrypt;
using Microsoft.AspNetCore.Http;
using LoanManagementSystem.Helpers;
namespace LoanManagementSystem.Controllers
{
    public class CustomerSignUpController : Controller
    {
        private readonly LoanManagementContext _context;

        public CustomerSignUpController(LoanManagementContext context)
        {
            _context = context;

        }
        // GET: CustomerSignUp/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("userId") != null && HttpContext.Session.GetInt32("isAdmin") == 1)
            {
                return RedirectToAction(actionName: "Index", controllerName: "Administrator");
            }
            if (HttpContext.Session.GetInt32("userId") != null && HttpContext.Session.GetInt32("isAdmin") == 0)
            {
                return RedirectToAction(actionName: "Index", controllerName: "Customer");
            }
            return View();
        }

        // POST: CustomerSignUp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,User_Name,User_Password")] UserAccount userAccount)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            if (ModelState.IsValid)
            {

                //check if it exist
                if (UserAccountExists(userAccount.User_Name))
                {
                    TempData["AlertMessage"] = "User Already Exists";
                    TempData["AlertType"] = "danger";
                    return RedirectToAction(nameof(Create));
                }
                //Hash Password Before Storing
                userAccount.User_Password = encoder.Encode(userAccount.User_Password);
                _context.Add(userAccount);
                await _context.SaveChangesAsync();             
                //should redirect to customer dashboard
                UserAccount User = _context.Accounts.FirstOrDefault(e => e.User_Name == userAccount.User_Name);

                HttpContext.Session.SetInt32("userId", User.Id);
                HttpContext.Session.SetInt32("isAdmin", 0);
                HttpContext.Session.SetString("username", User.User_Name);

                UserAccount user = new UserAccount();
                user.User_Name = User.User_Name;
                user.User_Password = User.User_Password;

                SessionHelper.SetObjectAsJson(HttpContext.Session, "user", user);
                return RedirectToAction(actionName: "Index", controllerName: "Customer");
            }
            return View(userAccount);
        }

        public void ActivateAdminAccount()
        {
           
            ScryptEncoder encoder = new ScryptEncoder();
            UserAccount User = new UserAccount() {
                User_Name = "admin",
                User_Password = encoder.Encode("admin"),
                IsAdmin = true
                };
             _context.Accounts.Add(User);
             _context.SaveChangesAsync();
            
        }
        private bool UserAccountExists(string User_Name)
        {
            return _context.Accounts.Any(e => e.User_Name == User_Name);
        }
    }
}
