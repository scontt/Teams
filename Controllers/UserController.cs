using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Teams.DTO;
using Teams.Interfaces;

namespace Teams.Controllers
{
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
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var user = _mapper.Map<UserDTO>(await _userRepository.GetUserById(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        // [HttpGet("{username:string}")]
        // [ProducesResponseType(200, Type = typeof(User))]
        // [ProducesResponseType(400)]
        // public async Task<ActionResult<User>> GetUserByUsername(string username)
        // {
        //     var user = _mapper.Map<UserDTO>(await _userRepository.GetUserByUsername(username));

        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     return Ok(user);
        // }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ICollection<User>>> GetAllUsers()
        {
            var users = _mapper.Map<ICollection<UserDTO>>(await _userRepository.GetAllUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDTO userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            
        }
    }
}