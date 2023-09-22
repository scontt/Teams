using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Teams.DTO;
using Teams.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Teams.Controllers
{
    // [Authorize]
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

        [HttpGet("{userId:int}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> GetUser([FromQuery]int userId)
        {
            var user = _mapper.Map<UserDTO>(_userRepository.GetUserById(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
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
        [HttpPost("register")]
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
        [HttpPost("login")]
        [ProducesResponseType(401)]
        public async Task<IResult> Login([FromBody] UserLogin userLogin)
        {
            var user = _userRepository.GetUserByUsername(userLogin.Username);
            if (!_userRepository.VerifyHashedPassword(user.Password, userLogin.Password))
                return Results.Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            
            var ecnodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new 
            {
                access_token = ecnodedJwt,
                username = userLogin.Username
            };

            return Results.Json(response);
        }
    }
}