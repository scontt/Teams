using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using Teams.DTO;
using Teams.Interfaces;

namespace Teams.Controllers
{
    public class TargetController : ControllerBase
    {
        private readonly ITargetRepository _targetRepository;
        private readonly IMapper _mapper;

        public TargetController(ITargetRepository targetRepository, IMapper mapper)
        {
            _targetRepository = targetRepository;
            _mapper = mapper;
        }

        [HttpPost("NewTarget")]
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

        [HttpGet("AllTargets")]
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

        [HttpPost("{newExecutor}")]
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
    }
}