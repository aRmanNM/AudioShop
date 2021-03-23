using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigsController : ControllerBase
    {
        private readonly IConfigRepository _configRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ConfigsController(IConfigRepository configRepo, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configRepo = configRepo;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<Config>> GetConfig([FromQuery] string title)
        {
            return Ok(await _configRepo.GetConfigAsync(title));
        }

        [HttpGet("{group}")]
        public async Task<ActionResult<IEnumerable<Config>>> GetGroupConfigs(string group)
        {
            return Ok(await _configRepo.GetConfigsByGroupAsync(group));
        }

        [Authorize(Roles="Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Config>>> GetAllConfigs()
        {
            return Ok(await _configRepo.GetAllConfigsAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Config>> SetConfig(Config config)
        {
            _configRepo.SetConfig(config);
            await _unitOfWork.CompleteAsync();
            return Ok(config);
        }
    }
}