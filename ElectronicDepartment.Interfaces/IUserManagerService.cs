using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;
using ElectronicDepartment.Web.Shared.Login;
using ElectronicDepartment.Web.Shared.User.Manager;
using ElectronicDepartment.Web.Shared.User.Manager.Responce;
using ElectronicDepartment.Web.Shared.User.Student;
using ElectronicDepartment.Web.Shared.User.Student.Responce;
using ElectronicDepartment.Web.Shared.User.Teacher;
using ElectronicDepartment.Web.Shared.User.Teacher.Responce;

namespace ElectronicDepartment.BusinessLogic
{
    public interface IUserManagerService
    {
        public Task<LoginResult> Login(LoginModel login);

        public Task UpdatePassword(string userId, string password, string newPassword);

        public Task UpdatePassword(string userId, string newPassword);

        public Task<string> CreateStudent(CreateStudentViewModel viewModel);

        public Task UpdateStudent(UpdateStudentViewModel viewModel);

        public Task<BaseStudentViewModel> GetStudent(string id);

        public Task<string> CreateTeacher(CreateTeacherViewModel viewModel);

        public Task UpdateTeacher(UpdateTeacherViewModel viewModel);

        public Task<BaseTeacherViewModel> GetTeacher(string id);

        public Task<string> CreateManager(CreateManagerViewModel viewModel);

        public Task UpdateManager(UpdateManagerViewModel viewModel);

        public Task<BaseManagerViewModel> GetManager(string id);

        public Task<ApiResultViewModel<GetTeacherViewModel>> GetApiTeacherResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests);
        
        public Task<ApiResultViewModel<GetManagerViewModel>> GetApiManagerResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests);
       
        public Task<ApiResultViewModel<GetStudentViewModel>> GetApiStudentResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests);
    }
}