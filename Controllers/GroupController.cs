using Microsoft.AspNetCore.Mvc;
using Teams.Interfaces;
using AutoMapper;
using Teams.DTO;

namespace Teams.Controllers
{
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupController(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        [HttpGet("{groupId}")] 
        public async Task<ActionResult<Group>> GetGroup(int groupId)
        {
            var group = _mapper.Map<GroupDTO>(_groupRepository.GetGroup(groupId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(group);
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromBody]GroupDTO groupCreate, int userId)
        {
            if (groupCreate == null)
                return BadRequest(ModelState);

            var groupMap = _mapper.Map<Group>(groupCreate);

            if (!_groupRepository.CreateGroup(groupMap))
            {
                ModelState.AddModelError("", "Ой, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            await AddNewMember(groupMap.Id, userId);

            return Ok("Группа успешно создана");
        }

        [HttpPut("{groupId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateGroup([FromQuery] int groupId, [FromBody] GroupDTO updatedGroup)
        {
            if (updatedGroup == null)
                return BadRequest(ModelState);

            if (!_groupRepository.IsGroupExists(groupId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var group = _groupRepository.GetGroup(groupId);

            var created = group.Created;

            var groupMap = _mapper.Map<Group>(updatedGroup);

            groupMap.Created = created;
            groupMap.Id = groupId;

            if (!_groupRepository.UpdateGroup(groupMap))
            {
                ModelState.AddModelError("", "Ой, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPost("newMember")]
        public async Task<IActionResult> AddNewMember([FromQuery]int groupId, [FromQuery]int userId)
        {
            var newMember = new Member()
            {
                UserId = userId,
                GroupId = groupId
            };

            if (!_groupRepository.AddNewMember(newMember))
            {
                ModelState.AddModelError("", "Ой, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            return Ok(newMember);
        }

        [HttpDelete("deleteMember")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMember([FromBody] MemberDTO memberToDelete)
        {
            if (memberToDelete == null)
                return BadRequest(ModelState);

            var memberMap = _mapper.Map<Member>(memberToDelete);

            if (!_groupRepository.DeleteMember(memberMap))
            {
                ModelState.AddModelError("", "Ой, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            return NoContent(); 
        }
    }
}