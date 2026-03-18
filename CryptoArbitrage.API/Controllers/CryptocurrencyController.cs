using CryptoArbitrage.Application.DTOs;
using CryptoArbitrage.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoArbitrage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptocurrencyController : ControllerBase
{
    private readonly ICryptoService _cryptoService;

    // Construtor: A API pede o serviço para a camada de Application
    public CryptocurrencyController(ICryptoService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterCryptoRequest request)
    {
        await _cryptoService.ExecuteAddAsync(request);
        
        return StatusCode(201);
    }
}