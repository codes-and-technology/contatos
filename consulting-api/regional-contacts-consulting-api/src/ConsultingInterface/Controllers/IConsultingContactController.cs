using Presenters;

namespace ConsultingInterface.Controllers;

public interface IConsultingContactController
{
    Task<List<ContactDto>> ConsultingAsync(short? regionId);
}
