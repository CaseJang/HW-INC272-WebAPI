using Microsoft.AspNetCore.Mvc;
using WebAPI.DBContext;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<UserController> _logger;

    private readonly DatabaseContext _databaseContext;

    public LoginController(ILogger<UserController> logger, DatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    [HttpPut] //login Function
        public IActionResult login(User user)
        {
            try
            {
                var _user = _databaseContext.Users.SingleOrDefault(o => o.Username == user.Username);
                if(_user.Password == user.Password)
                {
                    return Ok(new {message= "login success"});
                }
                else
                {
                    return Ok(new {message= "login fail"});
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {result = ex.Message, message= "fail"});
            }
        }

    [HttpGet("{state}")] //logout function
    public IActionResult logout(User user, string state) 
    {
       try
            {
                var _user = _databaseContext.Users.SingleOrDefault(o => o.Username == user.Username);
                if(state == "logout"&&_user.Username == user.Username)
                {
                    return Ok(new {logoutuser = user.Username, message = "logout success"});
                }
                else
                {
                    return Ok(new {message= "logout fail"});
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {result = ex.Message, message= "fail"});
            }
       
    }
    [HttpPut("{state}")] //change password function
        public IActionResult ChangePW(User user, string state)
        {
            try
            {
                var _user = _databaseContext.Users.SingleOrDefault(o => o.Username == user.Username);
                if(_user != null&&state == "Forgot")
                {
                    if(_user.Username == user.Username)
                    {
                    _user.Password = user.Password;
                    _databaseContext.Users.Update(_user);
                    _databaseContext.SaveChanges();
                    return Ok(new {message= "change password success", user = user.Username, Newpassword = user.Password});
                    }
                    else
                    {
                    return Ok(new {message= "change password fail"});
                    }
                     
                }
                else
                {
                    return Ok(new {message= "change password fail"});
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {result = ex.Message, message= "fail"});
            }
        }
}