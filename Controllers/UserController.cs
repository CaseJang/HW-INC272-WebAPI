using Microsoft.AspNetCore.Mvc;
using WebAPI.DBContext;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<UserController> _logger;

    private readonly DatabaseContext _databaseContext;

    public UserController(ILogger<UserController> logger, DatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
       var users = _databaseContext.Users.ToList();
       return Ok(new { result = users, message = "success"});
    }

    [HttpGet("{id}")]
    public IActionResult GetUserbyId(int id)
    {
        var user =_databaseContext.Users.SingleOrDefault(o => o.Id == id);
        return Ok(new{result=user, message="success"});
    }

    [HttpGet("{User}")]
    public IActionResult GetUserbyUser(string User)
    {
        try
        {
        var user =_databaseContext.Users.SingleOrDefault(o => o.Username == User);
        return Ok(new{result=user, message="success"});
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message= "fail"});
        }
    }

    [HttpPost] // -> https://localhost:5001/user
        public IActionResult CreateUser(User user)
        {
            try
            {
                _databaseContext.Users.Add(user); // command add action
                _databaseContext.SaveChanges(); // commit to database
                return Ok(new {message= "success"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {result = ex.Message, message= "fail"});
            }
        }

    [HttpPut] // -> https://localhost:5001/user
        public IActionResult UpdateUser(User user)
        {
            try
            {
                var _user = _databaseContext.Users.SingleOrDefault(o => o.Id == user.Id);
                if(_user != null)
                {
                    _user.Username = user.Username;
                    _user.Password = user.Password;
                    _user.Position = user.Position;

                    _databaseContext.Users.Update(_user);
                    _databaseContext.SaveChanges();
                    return Ok(new {message= "success"});
                }
                else
                {
                    return Ok(new {message= "fail"});
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {result = ex.Message, message= "fail"});
            }
        }

    [HttpDelete("{id}")] // -> https://localhost:5001/user/1
    public IActionResult DeleteUser(int id)
    {
        try
        {
            var _user = _databaseContext.Users.SingleOrDefault(o => o.Id == id);
            if(_user != null)
            {
                _databaseContext.Users.Remove(_user);
                _databaseContext.SaveChanges();
                return Ok(new {message= "success"});
            }
            else
            {
                return Ok(new {message= "fail"});
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message= "fail"});
        }
    }
}