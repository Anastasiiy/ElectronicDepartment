using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;
using ElectronicDepartment.Web.Shared.Lesson;
using ElectronicDepartment.Web.Shared.Lesson.Responce;

namespace ElectronicDepartment.Interfaces
{
    public interface ILessonService
    {
        public Task<int> CreateLesson(CreateLessonViewModel viewModel);

        public Task UpdateLesson(UpdateLessonViewModel viewModel);

        public Task<UpdateLessonViewModel> GetLesson(int id);

        public Task<IEnumerable<GetCourseLessonViewModel>> GetCourseLessons(int courseId);

        public Task DeleteLesson(int id);

        public Task<ApiResultViewModel<GetLessonViewModel>> GetApiLessonResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests);
    }
}