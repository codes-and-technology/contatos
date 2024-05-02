using RegionalContacts.Core.Dto;
using RegionalContacts.Core.Entity;
using RegionalContacts.Core.Repositories.Interfaces;
using RegionalContacts.Service.Services.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RegionalContacts.Service.Services;

public partial class ContactService(IUnitOfWork unitOfWork) : IContactService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IList<ContactDto>> FindAsync(short? regionId)
    {
        List<ContactDto> list = [];

        var result = await _unitOfWork.Contacts.FindAllAsync();

        if (regionId.HasValue)
            result = result.Where(f => f.PhoneRegion.RegionNumber == regionId.Value).ToList();

        foreach (var item in result)
        {
            list.Add(new ContactDto
            {
                Email = item.Email,
                Name = item.Name,
                PhoneNumber = item.PhoneNumber,
                RegionNumber = item.PhoneRegion.RegionNumber
            });
        }

        return list;
    }

    public async Task<Result<Contact>> AddAsync(ContactDto dto)
    {
        var result = new Result<Contact>();

        await BasicValidation(dto, result);

        if (result.Errors.Count > 0)
            return result;

        PhoneRegion phoneRegion = await ValidatePhoneRegionAsync(dto);

        Contact contact = new()
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            CreatedDate = DateTime.Now,
            Id = Guid.NewGuid(),
            PhoneRegion = phoneRegion
        };

        await _unitOfWork.Contacts.AddAsync(contact);

        await _unitOfWork.CommitAsync();

        result.Object = contact;

        return result;
    }

    public async Task<Result<Contact>> UpdateAsync(Guid id, ContactDto dto)
    {
        var result = new Result<Contact>();

        await BasicValidation(dto, result);
        if (result.Errors.Count > 0)
            return result;        

        var phoneRegion = await _unitOfWork.PhoneRegions.GetByRegionNumberAsync(dto.RegionNumber);

        var contact = await _unitOfWork.Contacts.FindByIdAsync(id);
        contact.Name = dto.Name;
        contact.Email = dto.Email;
        contact.PhoneNumber = dto.PhoneNumber;

        if (dto.RegionNumber != contact.PhoneRegion.RegionNumber && phoneRegion is null)
        {
            phoneRegion = new PhoneRegion
            {
                CreatedDate = DateTime.Now,
                RegionNumber = dto.RegionNumber,
                Id = Guid.NewGuid(),
            };

            await _unitOfWork.PhoneRegions.AddAsync(phoneRegion);
        }
            
        contact.PhoneRegion = phoneRegion;

        await _unitOfWork.CommitAsync();

        result.Object = contact;
        return result;
    }

    public async Task<Result<Contact>> DeleteAsync(Guid id)
    {
        var result = new Result<Contact>();
        var contact = await _unitOfWork.Contacts.FindByIdAsync(id);

        if (contact is null)
        {
            result.Errors.Add("Contato não encontrado");
            return result;
        }

        await _unitOfWork.Contacts.DeleteAsync(contact);
        await _unitOfWork.CommitAsync();

        result.Success = true;
        return result;
    }

    private async Task BasicValidation(ContactDto dto, Result<Contact> result)
    {
        if (dto is null)
            result.Errors.Add("Não foi possível validar o contato, verifique os dados informados");

        if (string.IsNullOrEmpty(dto.Name))
            result.Errors.Add("Nome é um campo obrigatório");

        if (dto.Name.Length > 250)
            result.Errors.Add("O campo Nome deve ter no máximo 250 caracteres");

        if (string.IsNullOrEmpty(dto.PhoneNumber))
            result.Errors.Add("Telefone é um campo obrigatório");

        if (!IsValidPhoneNumber(dto))
            result.Errors.Add("Telefone inválido, deve conter apenas o número de telefone sem o DDD");

        if (dto.RegionNumber == 0)
            result.Errors.Add("DDD é um campo obrigatório");

        if (!IsValidEmail(dto.Email))
            result.Errors.Add("Email inválido");

        if (dto.Email.Length > 250)
            result.Errors.Add("O campo Email deve ter no máximo 250 caracteres");

        var contact = await _unitOfWork.Contacts.FindByPhoneNumberAsync(dto.PhoneNumber, dto.RegionNumber);
        if (contact is not null)
        {
            result.Errors.Add("Este telefone já está cadastrado");
        }

        if (result.Errors.Count == 0)
            result.Success = true;
    }
    private bool IsValidPhoneNumber(ContactDto dto)
    {
        dto.PhoneNumber = dto.PhoneNumber.Trim()
           .Replace(" ", "")
           .Replace("-", "")
           .Replace("(", "")
           .Replace(")", "");

        if (dto.PhoneNumber.Length != 9)
            return false;

        return true;
    }
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
    private async Task<PhoneRegion> ValidatePhoneRegionAsync(ContactDto dto)
    {
        var phoneRegion = await _unitOfWork.PhoneRegions.GetByRegionNumberAsync(dto.RegionNumber);

        phoneRegion ??= new PhoneRegion { CreatedDate = DateTime.Now, Id = Guid.NewGuid(), RegionNumber = dto.RegionNumber };

        return phoneRegion;
    }
}
