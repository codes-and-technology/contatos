using Microsoft.AspNetCore.Mvc;
using RegionalContacts.Domain.Dto;
using RegionalContacts.Domain.Dto.Contato;
using RegionalContacts.Domain.Entity;
using RegionalContacts.Service.Services.Interfaces;

namespace RegionalContacts.Api.Controllers
{
    /// <summary>
    /// Controlador para manipulação de contatos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _service;

        /// <summary>
        /// Inicializa uma nova instância do controlador de contatos.
        /// </summary>
        /// <param name="service">Serviço de contato.</param>
        public ContactController(IContactService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria um novo contato.
        /// </summary>
        /// <param name="dto">DTO para criar um contato.</param>
        /// <returns>Um ActionResult contendo o resultado da criação do contato. Retorna um Ok com o resultado se a operação for bem-sucedida ou um BadRequest com o erro caso contrário.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<Contact>), 200)]
        [ProducesResponseType(typeof(Result<Contact>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post(ContactCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.CreateAsync(dto);

                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Obtém contatos filtrados por ID de região.
        /// </summary>
        /// <param name="regionId">ID da região para filtrar os contatos. Opcional.</param>
        /// <returns>Um ActionResult contendo a lista de contatos filtrados pela ID da região. Retorna um Ok com a lista de contatos se a operação for bem-sucedida ou um BadRequest com o erro caso contrário.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Contact>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get([FromQuery] short? regionId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(await _service.FindAsync(regionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um contato existente.
        /// </summary>
        /// <param name="id">ID do contato a ser atualizado.</param>
        /// <param name="dto">DTO contendo os dados atualizados do contato.</param>
        /// <returns>Um ActionResult contendo o resultado da atualização do contato. Retorna um Ok com o resultado se a operação for bem-sucedida ou um BadRequest com o erro caso contrário.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result<Contact>), 200)]
        [ProducesResponseType(typeof(Result<Contact>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put(Guid id, ContactUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.UpdateAsync(id, dto);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Exclui um contato existente.
        /// </summary>
        /// <param name="id">ID do contato a ser excluído.</param>
        /// <returns>Um ActionResult contendo o resultado da exclusão do contato. Retorna um Ok com o resultado se a operação for bem-sucedida ou um BadRequest com o erro caso contrário.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result<Contact>), 200)]
        [ProducesResponseType(typeof(Result<Contact>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.DeleteAsync(id);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
