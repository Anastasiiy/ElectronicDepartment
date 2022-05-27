using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;
using ElectronicDepartment.Web.Shared.Course;
using ElectronicDepartment.Web.Shared.Course.Responce;

namespace ElectronicDepartment.Interfaces
{
    public interface ICourseService
    {
        public Task<int> Create(CreateCourseViewModel viewModel);

        public Task Update(UpdateCourseViewModel viewModel);

        public Task<GetCourseViewModel> Get(int id);

        public Task<IEnumerable<GetCourseSelectorViewModel>> GetSelector();
        
        public Task<ApiResultViewModel<GetCourseViewModel>> GetApiResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests);
    }
}