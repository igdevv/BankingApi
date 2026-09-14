using BankingApi.Dtos.Transfers;
using BankingApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransfersController: ControllerBase
{
    private readonly TransferService _service;

    public TransfersController(TransferService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<TransferResponse>> Create(CreateTransferRequest request)
    {
        try
        {
            var response = await _service.TransferAsync(request);

            return Ok(response);
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }


    }
}
