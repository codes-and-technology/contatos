using Presenters;
using Refit;

namespace External.Interfaces
{
    public interface IContactExternal
    {
        [Get("/api/Contact")]
        Task<IApiResponse<IEnumerable<ContactConsultingDto>>> Get();
    }
}
