using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId) {
            Console.WriteLine($"--> Getting Commands for Platform: {platformId}");
            if (!_commandRepo.PlatfromExsits(platformId)){
                return NotFound();
            }

            var commands = _commandRepo.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Getting Command for Platform: {platformId}");
            if (!_commandRepo.PlatfromExsits(platformId)){
                return NotFound();
            }

            var command = _commandRepo.GetCommand(platformId, commandId);

            if (command is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Create Command for Platform: {platformId}");
            if (!_commandRepo.PlatfromExsits(platformId)){
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);

            if (command is null) 
            {
                return NotFound();
            }

            _commandRepo.CreateCommand(platformId, command);
            _commandRepo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId, commandId = commandReadDto.Id}, commandReadDto);
        }
    }
}