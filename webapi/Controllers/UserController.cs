using Microsoft.AspNetCore.Mvc;
using webapi.Interfaces;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger,
        IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        [HttpGet("{id}")]
        public IActionResult GET(int id){
            try
            {
                var user = _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error geting users");
                return StatusCode(500,"Error Server");
            }
        }
        [HttpGet]
        public IActionResult GetAll(){
            try
            {
                var users = _userRepository.GetAllUsers();
                if(users == null){
                    return NotFound();
                }
                return Ok(users);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error geting all users");
                return StatusCode(500,"Error Server");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            try
            {
                _userRepository.CreateUser(user);
                _logger.LogInformation("User Created succesfully");
                return CreatedAtAction(nameof(GET),new{id = user.Id}, user);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error creating users");
                return StatusCode(500,"Error Server");
            }
        }
        
        [HttpPut("{id}")]
        public IActionResult Update([FromBody]User user)
        {
            try{
                if (_userRepository.UpdateUser(user))
                {
                    return NoContent();
                }else{
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error updating users");
                return StatusCode(500,"Error Server");
            }
        }

        [HttpPut("changeRol/{id}")]
        public IActionResult Update(int idUser, [FromBody] string newRol)
        {
            try
            {
                var user = _userRepository.GetUserById(idUser);
                user.Rol = newRol;
                if (_userRepository.UpdateUser(user))
                {
                    return NoContent();
                }else{
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error updating users");
                return StatusCode(500,"Error Server");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_userRepository.DeleteUser(id))
                {
                    return NoContent();
                }else{
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error deleting users");
                return StatusCode(500,"Error Server");
            }
        }
    }
}