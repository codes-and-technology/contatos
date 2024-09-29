using ConsultingEntitys;
using ConsultingInterface.Controllers;
using Microsoft.AspNetCore.Mvc;
using Presenters;

namespace Consulting.Api.Controllers
{
    /// <summary>
    /// Controlador para manipulação de contatos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IConsultingContactController _contactController;

        public ContactController(IConsultingContactController contactController)
        {
            _contactController = contactController;
        }

        /// <summary>
        /// Obtém contatos filtrados por ID de região.
        /// </summary>
        /// <param name="regionId">ID da região para filtrar os contatos. Opcional.</param>
        /// <returns>Um ActionResult contendo a lista de contatos filtrados pela ID da região. Retorna um Ok com a lista de contatos se a operação for bem-sucedida ou um BadRequest com o erro caso contrário.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ConsultingResult<IEnumerable<ContactEntity>>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get([FromQuery] short? regionId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(await _contactController.ConsultingAsync(regionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
