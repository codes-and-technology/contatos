using Presenters;
using DeleteInterface;
using Microsoft.AspNetCore.Mvc;

namespace Delete.Api.Controllers;

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
    /// Exclui um contato existente.
    /// </summary>
    /// <param name="id">ID do contato a ser excluído.</param>
    /// <returns>Um ActionResult contendo o resultado da exclusão do contato. Retorna um Ok com o resultado se a operação for bem-sucedida ou um BadRequest com o erro caso contrário.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _controller.DeleteAsync(id);
            return result.Success ? NoContent() : BadRequest(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}
