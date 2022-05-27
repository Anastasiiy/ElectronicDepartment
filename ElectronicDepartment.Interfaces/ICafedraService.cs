using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Cafedra;
using ElectronicDepartment.Web.Shared.Cafedra.Responce;
using ElectronicDepartment.Web.Shared.Common;

namespace ElectronicDepartment.Interfaces
{
    public interface ICafedraService
    {
        public Task<int> Create(CreateCafedraViewModel viewModel);

        public Task Update(UpdateCafedraViewModel viewModel);

        public Task<GetCafedraViewModel> Get(int id);

        public Task<IEnumerable<GetCafedraSelectorViewModel>> GetSelector();

        public Task<ApiResultViewModel<GetCafedraViewModel>> GetApiResponce(int pageIndex,
            int pageSize,
            IEnumerable<SortingRequest> sortingRequests = null,
            IEnumerable<FilterRequest> filterRequests = null);
    }
}