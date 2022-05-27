using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;
using ElectronicDepartment.Web.Shared.CourseTeacher;
using ElectronicDepartment.Web.Shared.CourseTeacher.Responce;

namespace ElectronicDepartment.Interfaces
{
    public interface ICourseTeacherService
    {
        public Task<int> Create(CreateCourseTeacherViewModel viewModel);

        public Task<IEnumerable<int>> CreateRange(IEnumerable<CreateCourseTeacherViewModel> viewModel);

        public Task Update(UpdateCourseTeacherViewModel viewModel);

        public Task<GetCourseTeacherViewModel> Get(int id);

        public Task RemoveTeacher(int id);

        public Task RemoveTeacherRange(IEnumerable<int> ids);

        public Task<IEnumerable<GetCourseTeacherSelectorViewModel>> GetSelector();

        public Task<ApiResultViewModel<GetCourseTeacherViewModel>> GetApiCourseTeacherResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests);
    }
}