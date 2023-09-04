using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using Teams.DTO;
using Teams.Interfaces;

namespace Teams.Controllers
{
    [Route("api/[controller]")]
    public class TargetController : ControllerBase
    {
        private readonly ITargetRepository _targetRepository;
        private readonly IMapper _mapper;

        public TargetController(ITargetRepository targetRepository, IMapper mapper)
        {
            _targetRepository = targetRepository;
            _mapper = mapper;
        }

        [HttpPost("newTarget")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTarget([FromBody]TargetDTO target, List<int> executorsId)
        {
            if (target == null)
                return BadRequest(ModelState);

            var targetMap = _mapper.Map<Target>(target);

            if(!_targetRepository.CreateTarget(targetMap))
            {
                ModelState.AddModelError("", "Ой, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            int lastTargetId = _targetRepository.GetLastTarget();

            foreach (int eId in executorsId)
                await AddNewExecutor(eId, lastTargetId);

            return Ok("Задание успешно создано");
        }

        [HttpGet("allTargets")]
        [ProducesResponseType(200, Type = typeof(Target))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Target>> GetTargets()
        {
            var target = _mapper.Map<ICollection<TargetDTO>>(_targetRepository.GetTargets());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(target);
        }

        [HttpGet("{targetId}")]
        [ProducesResponseType(200, Type = typeof(Target))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Target>> GetTarget(int targetId)
        {
            var target = _mapper.Map<TargetDTO>(_targetRepository.GetTarget(targetId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(target);
        }

        [HttpPost("newExecutor")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddNewExecutor([FromQuery]int executorId, [FromQuery]int targetId)
        {
            var executor = new ExecutorDTO()
            {
                UserId = executorId,
                TargetId = targetId
            };

            var executorMap = _mapper.Map<Executor>(executor);

            if (!_targetRepository.addNewExecutor(executorMap))
            {
                ModelState.AddModelError("", "Ой, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            return Ok("Исполнители успешно добавлены");
        }

        [HttpDelete("deleteExecutor")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteExecutor([FromQuery]int userId, [FromQuery]int targetId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ExecutorDTO executor = new ExecutorDTO()
            {
                UserId = userId,
                TargetId = targetId
            };

            var executorMap = _mapper.Map<Executor>(executor);
            
            if (!_targetRepository.DeleteExecutor(executorMap))
            {
                ModelState.AddModelError("", "Ой, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpPut("{targetId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTarget(int targetId, [FromBody]TargetDTO updatedTarget)
        {
            if (updatedTarget == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var target = _targetRepository.GetTarget(targetId);

            var targetMap = _mapper.Map<Target>(updatedTarget);

            targetMap.Id = targetId;
            targetMap.Created = target.Created;
            targetMap.Groupid = target.Groupid;

            if (!_targetRepository.UpdateTarget(targetMap))
            {
                ModelState.AddModelError("", "Ой, что-то пошло не так :/");
                return StatusCode(500, ModelState);
            }

            return NoContent();
            
        }

    }
}