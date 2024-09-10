using CreateDto;
using CreateInterface;
using Microsoft.AspNetCore.Mvc;

namespace Create.Api.Controllers;

/// <summary>
/// Controlador para manipulação de contatos.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IController _controller;

    public ContactController(IController controller)
    {
        _controller = controller;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ContactDto contactDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _controller.CreateAsync(contactDto);
            return result.Success ? NoContent() : BadRequest(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}
