using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System.Text.Json;
namespace SampleMVC.Controllers;

public class HomeController : Controller
{

    private readonly IRoleBLL _rolebll;
    private readonly IUserBLL _userBLL;

    public HomeController(IRoleBLL roleBLL, IUserBLL userBLL)
    {
        _rolebll = roleBLL;
        _userBLL = userBLL;
    }
    // Home/Index
    public IActionResult Index()
    {
        //check if session not null
        if (HttpContext.Session.GetString("user") != null)
        {
            var userDto = JsonSerializer.Deserialize<UserDTO>(HttpContext.Session.GetString("user"));
            //ViewBag.Message = $"Welcome {userDto.FirstName} {userDto.LastName}";
        }

        ViewData["Title"] = "Home Page";
        return View();
    }





    public IActionResult AddNewRole()
    {

        return View("AddNewRole");
    }
    [HttpPost]
    public IActionResult AddNewRole(string RoleName)
    {
        _rolebll.AddRole(RoleName);
        return View("index");
    }

    public IActionResult AddUserRole()
    {


        var dropdwnroles = _rolebll.GetAllRoles();
        var dropdwnuser = _userBLL.GetAll();
        ViewBag.allroles = dropdwnroles;
        ViewBag.alluser = dropdwnuser;
        return View();
    }
    [HttpPost]
    public IActionResult AddUserRole(string Username, int RoleId)
    {


        try
        {
            _rolebll.AddUserToRole(Username, RoleId);
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Category Success !</div>";
        }
        catch (Exception ex)
        {

            TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
        }
        return RedirectToAction("Index");
    }

    [Route("/Hello/ASP")]
    public IActionResult HelloASP()
    {
        return Content("Hello ASP.NET Core MVC!");
    }

    // Home/About
    public IActionResult About()
    {
        return View();
    }
    public IActionResult SeeAllUser()
    {
        var users = _userBLL.GetAll();
        var listUsers = new SelectList(users, "Username", "Username");
        ViewBag.Users = listUsers;

        var roles = _rolebll.GetAllRoles();
        var listRoles = new SelectList(roles, "RoleID", "RoleName");
        ViewBag.Roles = listRoles;

        var usersWithRoles = _userBLL.GetAllWithRoles();
        return View(usersWithRoles);
    }


    public IActionResult Contact()
    {
        return Content("This is the Contact action method...");
    }

    //action method for uploading file
    public IActionResult UploadFilePics()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UploadFilePics(IFormFile file)
    {
        if (file != null)
        {
            if (Helper.IsImageFile(file.FileName))
            {
                //random file name based on GUID
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pics", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                ViewBag.Message = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>File uploaded successfully !</div>";
            }
            else
            {
                ViewBag.Message = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>File is not an image file !</div>";
            }
        }
        return View();
    }
}
