using Microsoft.AspNetCore.Mvc;
using RegionalContacts.Core.Dto;
using RegionalContacts.Service.Services.Interfaces;

namespace Regional.Contacts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _service;

        public ContactController(IContactService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ContactDto dto)
        {
            try
            {
                await _service.AddAsync(dto);   
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
    }
}
