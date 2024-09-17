using System.ComponentModel.DataAnnotations;

namespace DeleteEntitys
{
    public class ContactEntity : EntityBase
    {
        public ContactEntity()
        {
            
        }

        public ContactEntity(string name, string phoneNumber, string email)
        {
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
        }

        [Required(ErrorMessage = "O nome do contato é obrigatório.")]
        [MinLength(10, ErrorMessage = "O nome do contato não pode ter menos de 10 caracteres.")]
        [MaxLength(250, ErrorMessage = "O nome do contato não pode ter mais de 250 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O número de telefone é obrigatório.")]
        [RegularExpression(@"^[0-9]{8,9}$", ErrorMessage = "O número de telefone deve conter apenas números e ter 8 ou 9 dígitos.")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "O endereço de e-mail é inválido.")]
        public string Email { get; set; }

        public PhoneRegionEntity PhoneRegion { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, PhoneNumber: {PhoneNumber}, Email: {Email}, PhoneRegion: {PhoneRegion}";
        }
    }
}
