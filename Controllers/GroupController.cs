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
            var group = _mapper.Map<GroupDTO>(await _groupRepository.GetGroup(groupId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(group);
        }
    }
}