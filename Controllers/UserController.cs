using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Teams.DTO;
using Teams.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using Teams.Services;

namespace Teams.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("getbyid/{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> GetUser([FromQuery]int userId)
        {
            var user = _mapper.Map<UserDTO>(_userRepository.GetUserById(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("getbyname")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IResult> GetUserByUsername([FromQuery]string username)
        {
            var user = _mapper.Map<UserDTO>(_userRepository.GetUserByUsername(username));

            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);
                
            return Results.Json(user);
        }

        [HttpGet("allUsers")]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ICollection<User>>> GetAllUsers()
        {
            var users = _mapper.Map<ICollection<UserDTO>>(_userRepository.GetAllUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("auth/signup")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDTO userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            userCreate.Password = _userRepository.HashPassword(userCreate.Password);

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "ОЙ, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            return Ok("Пользователь успешно создан");
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateUser([FromQuery] int userId, [FromBody] UserDTO userUpdate)
        {
            if (userUpdate == null)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userUpdate);
            userMap.Id = userId;
            
            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Ой, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("auth/signin")]
        [ProducesResponseType(401)]
        public async Task<IResult> Login([FromBody] UserLogin userLogin)
        {
            var _user = _mapper.Map<UserDTO>(_userRepository.GetUserByUsername(userLogin.Username));

            if (_user == null)
                return Results.Conflict();

            if (!_userRepository.VerifyHashedPassword(_user.Password, userLogin.Password))
                return Results.Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, _user.Username) };
            var ecnodedJwt = AuthService.GetJwtToken(claims);

            var userJson = JsonConvert.SerializeObject(_user);

            var response = new 
            {
                access_token = ecnodedJwt,
                user = userJson
            };

            return Results.Json(response);
        }
    }
}