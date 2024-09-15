using System.ComponentModel.DataAnnotations;

namespace Presenters;

public class ContactDto
{
    /// <summary>
    /// Nome do contato.
    /// </summary>
    [Required(ErrorMessage = "O nome do contato é obrigatório.")]
    [MinLength(10, ErrorMessage = "O nome do contato não pode ter menos de 10 caracteres.")]
    [MaxLength(250, ErrorMessage = "O nome do contato não pode ter mais de 250 caracteres.")]
    public string Name { get; set; }

    /// <summary>
    /// Número de telefone do contato.
    /// </summary>
    [Required(ErrorMessage = "O número de telefone é obrigatório.")]
    [RegularExpression(@"^[0-9]{8,9}$", ErrorMessage = "O número de telefone deve conter apenas números e ter 8 ou 9 dígitos.")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Endereço de e-mail do contato.
    /// </summary>
    [EmailAddress(ErrorMessage = "O endereço de e-mail é inválido.")]
    public string Email { get; set; }

    /// <summary>
    /// Número da região do telefone do contato.
    /// </summary>
    [Required(ErrorMessage = "O número da região é obrigatório.")]
    [Range(11, 99, ErrorMessage = "O número da região deve estar entre 11 e 99.")]
    public short RegionNumber { get; set; }
}
