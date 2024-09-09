using CreateDto;
using System.ComponentModel.DataAnnotations;

namespace CreateUseCases.Helpers
{
    public static class Validator
    {
        public static void Valid<T1, T2>(this ResultDto<T1> result, T2 dto) where T1 : class
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(dto);

            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(dto, validationContext, validationResults, true);

            if (!isValid)
                result.Errors = validationResults.Select(r => r.ErrorMessage).ToList();
        }
    }
}
