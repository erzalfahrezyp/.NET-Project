using MeinProfil.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MeinProfil.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IWebHostEnvironment _hosting;

        public UserController(UserManager<AppUser> userManager, AppIdentityDbContext identityDbContext, IWebHostEnvironment hosting)
        {
            _userManager = userManager;
            _identityDbContext = identityDbContext;
            _hosting = hosting;
        }
        [Authorize]
        public IActionResult Index()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var usernameLogin = _userManager.GetUserName(currentUser);

            var data = _identityDbContext.AppUsers.Where(o => o.UserName == usernameLogin).SingleOrDefault();

            ViewData["username"] = data.UserName;
            ViewData["name"] = data.Fullname;
            return View();
        }
        [Authorize]
        [Route("user/{username}")]
        public IActionResult Profile(string username)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var usernameLogin = _userManager.GetUserName(currentUser);

            var data = _identityDbContext.AppUsers.Where(o => o.UserName == username).SingleOrDefault();

            if (usernameLogin == username)
            {
                return View(data);
            }
            return View("PublicProfile", data);
        }
        [Authorize]
        [Route("user/members")]
        public async Task<IActionResult> Members()
        {
            var members = await _identityDbContext.AppUsers.ToListAsync();

            return View(members);
        }
        [Authorize]
        public IActionResult Edit()
        {
            List<string> fieldNames = new List<string>()
            {
                "Fullname",
                "Email",
                "Address",
                "City"
            };
            ViewBag.FieldNames = new SelectList(fieldNames);
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAsync(EditView editView)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var idLogin = _userManager.GetUserId(currentUser);
            string uniqueFileName = UploadImage(editView.ImagePath);

            var data = await _userManager.FindByIdAsync(idLogin);
            typeof(AppUser).GetProperty(editView.FieldName).SetValue(data, editView.NewValue);
            data.Path = uniqueFileName;

            var result = await _userManager.UpdateAsync(data);
            return RedirectToAction("Profile", "User", new { username = User.Identity.Name });
        }
        //[Authorize]
        //public IActionResult EditPict()
        //{
        //    return View();
        //}
        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> EditPictAsync(EditPictView editPictView)
        //{
        //    System.Security.Claims.ClaimsPrincipal currentUser = this.User;
        //    var idLogin = _userManager.GetUserId(currentUser);
        //    string uniqueFileName = UploadImage(editPictView.ImagePath);

        //    var data = await _userManager.FindByIdAsync(idLogin);
        //    //typeof(AppUser).GetProperty(editPictView.FieldName).SetValue(data, editPictView.NewValue);
        //    data.Path = uniqueFileName;

        //    var result = await _userManager.UpdateAsync(data);
        //    return RedirectToAction("Profile", "User", new { username = User.Identity.Name });
        //}


        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = string.Empty;
            if (file != null)
            {
                string uploadFolder = _hosting.WebRootPath + "/upload/";
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
