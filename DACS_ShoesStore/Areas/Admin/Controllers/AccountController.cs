using DACS_ShoesStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DACS_ShoesStore.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            this.UserManager = userManager;
            SignInManager = signInManager;
        }

       

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Admin/Account
        /* public ActionResult Index()
         {
             var item = db.Users.ToList();
             int currentPage = 1;
             int pageSize = 10;
             var userList = db.Users.OrderBy(s=>s.UserName).Skip((currentPage-1)* pageSize).Take(pageSize).ToList();

             List<UserListModel> result = new List<UserListModel>();

             for (int i = 0; i < userList.Count; i++) {
                 var roles = UserManager.GetRoles(userList[i].Id).ToList();
                 string roleStr = String.Join(",", roles.ToArray());
                 UserListModel usModel = new UserListModel();
                 usModel.Id = userList[i].Id;
                 usModel.Name = userList[i].UserName;
                 usModel.RoleName = roleStr;
                 result.Add(usModel);
             }

             //moel réult ra
             //thay them model rooif h goi ra thoi 

             return View(result);
         }*/
        public ActionResult Index()
        {
            var item = db.Users.ToList();
            return View(item);
        }

        // GET: Admin/Account/Create
        public ActionResult Create()
        {
            ViewBag.Role =new SelectList (db.Roles.ToList(), "Id " ," Name");
            return View();
        }

        //
        // POST: Admin/Account/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName/*Email = model.Email */,PhoneNumber = model.PhoneNumber};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Id ", " Name");


            // If we got this far, something failed, redisplay form
            return View(model);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}