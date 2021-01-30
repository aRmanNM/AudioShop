using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigsController : ControllerBase
    {
        private readonly IConfigRepository _configRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ConfigsController(IConfigRepository configRepo, IUnitOfWork unitOfWork)
        {
            _configRepo = configRepo;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<Config>> GetConfig([FromQuery] string title)
        {
            return Ok(await _configRepo.GetConfigAsync(title));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Config>>> GetAllConfigs()
        {
            return Ok(await _configRepo.GetAllConfigsAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Config>> SetConfig(Config config)
        {
            await _configRepo.SetConfigAsync(config);
            await _unitOfWork.CompleteAsync();
            return Ok(config);
        }
    }
}