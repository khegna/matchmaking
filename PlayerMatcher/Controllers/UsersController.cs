﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PlayerMatcher;
using Newtonsoft.Json;

namespace PlayerMatcher.Controllers
{
    public class UsersController : Controller
    {
        private PlayerMatcherEntities db = new PlayerMatcherEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        public User UserLoggedInSmart(User user) { 
        
            var employeeLoggedIn = db.Users.Where(x => x.User_Name == user.User_Name && x.User_Password == user.User_Password).FirstOrDefault();
            return employeeLoggedIn;

        }

        /*private EmployeeRepository _userRepository;
            private ManagementSystemEntities db2 = new ManagementSystemEntities();

            public EmployeeController()
            {
                _userRepository = new EmployeeRepository();
            }
            */

            [AllowAnonymous]
            public ActionResult SmartLogin(string returnUrl)
            {
                ViewBag.ReturnUrl = returnUrl;
                var employeeSession = (User)Session["user"];

                if (employeeSession != null)
                {
                return View();
                    //return RedirectToAction("WelcomePage", "Employee", new { employee = Session["employee"] });
                }
                else
                {
                    return View();
                }
            }

            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public ActionResult SmartLogin(User user)
            {
                var userLoggedIn = UserLoggedInSmart(user);
                if (userLoggedIn != null)
                {
                    ViewBag.message = "loggedin";
                    ViewBag.triedOnce = "yes";


                    Session["user"] = userLoggedIn;

                    var userS = (User)Session["user"];
                    //ViewBag.employeeTitle = employeeS.JobTitle;

                    return RedirectToAction("Welcome", "Users", new { username = userLoggedIn?.User_Name });
                }
                else
                {
                    ViewBag.triedOnce = "yes";
                    return View();
                }

            }
            public ActionResult login()
            {
                var userSession = (User)Session["user"];

                if (userSession != null)
                {
                    return RedirectToAction("Welcome", "Users", new { user = Session["user"] });
                }
                else
                {
                    return View();
                }
            }
            public ActionResult logout()
            {
                Session["user"] = null;
                return RedirectToAction("SmartLogin", "Users");
                //return View(new { employee = Session["employee"] });
            }
            [HttpPost]
            public ActionResult login(User user)
            {
                var userSM = UserLoggedInSmart(user);
                if (userSM != null)
                {
                    ViewBag.message = "loggedin";
                    ViewBag.triedOnce = "yes";


                    Session["user"] = userSM;

                    var userS = (User)Session["user"];
                    ViewBag.employeeTitle = userS.Is_Online;

                    return RedirectToAction("Welcome", "Users", new { username = userSM?.User_Name });
                }
                else
                {
                    ViewBag.triedOnce = "yes";
                    return View();
                }
            }
            // GET: Users/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Welcome() {
            return View();
        }

        // POST: Users/Create.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Name,User_Password,Is_Online")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("SmartLogin");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_ID,User_Name,User_Password,Is_Online")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
