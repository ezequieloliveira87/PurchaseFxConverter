namespace PurchaseFxConverter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(IPurchaseTransactionService purchaseTransactionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseTransactionRequest request)
    {
        try
        {
            var id = await purchaseTransactionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new
            {
                id
            }, new
            {
                id
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PurchaseTransactionViewModel>> GetById(Guid id)
    {
        var transaction = await purchaseTransactionService.GetByIdAsync(id);
        if (transaction is null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpGet("{id:guid}/convert")]
    public async Task<IActionResult> Convert(Guid id, [FromQuery] string currency)
    {
        try
        {
            var result = await purchaseTransactionService.ConvertTransactionAsync(id, currency);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new
            {
                error = ex.Message
            });
        }
    }
}