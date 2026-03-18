using CryptoArbitrage.Application.DTOs;
using CryptoArbitrage.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoArbitrage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptocurrencyController : ControllerBase
{
    private readonly ICryptoService _cryptoService;
    private readonly IArbitrageService _arbitrageService;

    // Construtor: A API pede o serviço para a camada de Application
    public CryptocurrencyController(ICryptoService cryptoService, IArbitrageService arbitrageService)
    {
        _cryptoService = cryptoService;
        _arbitrageService = arbitrageService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterCryptoRequest request)
    {
        await _cryptoService.ExecuteAddAsync(request);

        return StatusCode(201);
    }

    [HttpGet("price/{symbol}")]
    public async Task<IActionResult> GetPrice(string symbol)
    {
        var result = await _cryptoService.GetCurrentPriceAsync(symbol);

        if (result.Price == 0)
        {
            return NotFound(new { message = "Symbol not found on Binance" });
        }

        return Ok(result);
    }

    [HttpGet("compare/{symbol}")]
    public async Task<IActionResult> Compare(string symbol)
    {
        var result = await _arbitrageService.CalculateArbitrageAsync(symbol);
        return Ok(result);
    }

    [HttpGet("alerts/recent")]
    public async Task<IActionResult> GetRecentAlerts([FromQuery] int count = 10)
    {
        var alerts = await _cryptoService.GetRecentAlertsAsync(count);
        return Ok(alerts);
    }
}