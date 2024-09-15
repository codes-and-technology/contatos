using Presenters;
using UpdateInterface;
using Microsoft.AspNetCore.Mvc;

namespace Update.Api.Controllers;

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

    /// <summary>
    /// Atualiza um contato existente.
    /// </summary>
    /// <param name="id">ID do contato a ser atualizado.</param>
    /// <param name="dto">DTO contendo os dados atualizados do contato.</param>
    /// <returns>Um ActionResult contendo o resultado da atualização do contato. Retorna um Ok com o resultado se a operação for bem-sucedida ou um BadRequest com o erro caso contrário.</returns>

    [HttpPut("{id}")]
    //[ProducesResponseType(typeof(Result<Contact>), 200)]
    //[ProducesResponseType(typeof(Result<Contact>), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update(Guid id, ContactDto contactDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _controller.UpdateAsync(id, contactDto);
            return result.Success ? NoContent() : BadRequest(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}
