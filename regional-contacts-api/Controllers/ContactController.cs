using Microsoft.AspNetCore.Mvc;
using RegionalContacts.Core.Dto;
using RegionalContacts.Service.Services.Interfaces;

namespace Regional.Contacts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController(IContactService service) : ControllerBase
{
    private readonly IContactService _service = service;

    [HttpPost]
    public async Task<IActionResult> Post(ContactDto dto)
    {
        try
        {
            var result = await _service.AddAsync(dto);

            return result.Success ? Ok(result): BadRequest(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] short? regionId)
    {
        try
        {
            return Ok(await _service.FindAsync(regionId));
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, ContactDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {

            return BadRequest(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {

            return BadRequest(ex);
        }
    }
}
