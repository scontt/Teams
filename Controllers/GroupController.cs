using Microsoft.AspNetCore.Mvc;
using Teams.Interfaces;
using AutoMapper;
using Teams.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Teams.Controllers
{
    [Authorize]
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

        [HttpGet("getallgroups")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> GetAllGroups()
        {
            var groups = _mapper.Map<ICollection<GroupDTO>>(_groupRepository.GetAllGroups());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(groups);
        }

        [HttpGet("{groupId}")] 
        public async Task<ActionResult<Group>> GetGroup(int groupId)
        {
            var group = _mapper.Map<GroupDTO>(_groupRepository.GetGroup(groupId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(group);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Group>> CreateGroup([FromBody]GroupDTO groupCreate)
        {
            if (groupCreate == null)
                return BadRequest(groupCreate);

            var groupMap = _mapper.Map<Group>(groupCreate);
            int userId = groupMap.OwnerId;

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

        [AllowAnonymous]
        [HttpGet("usergroups")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IResult> GetUsersGroups(int userId)
        {
            var groups = _mapper.Map<ICollection<GroupDTO>>(_groupRepository.GetUsersGroups(userId));
            return Results.Json(groups);
        }
    }
}