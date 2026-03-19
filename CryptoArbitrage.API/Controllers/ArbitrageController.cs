using Microsoft.AspNetCore.Mvc;
using CryptoArbitrage.Domain.Interfaces; // Garanta que este caminho está correto para sua interface

namespace CryptoArbitrage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArbitrageController : ControllerBase
    {
        private readonly IArbitrageAlertRepository _repository;

        public ArbitrageController(IArbitrageAlertRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alerts = await _repository.GetAllAsync();
            return Ok(alerts);
        }
    }
}