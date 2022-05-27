using ElectronicDepartment.Common.Exceptions;
using ElectronicDepartment.DataAccess;
using ElectronicDepartment.DomainEntities;
using ElectronicDepartment.Web.Shared.Course;
using ElectronicDepartment.Web.Shared.Course.Responce;
using Microsoft.EntityFrameworkCore;
using ElectronicDepartment.Interfaces;
using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;

namespace ElectronicDepartment.BusinessLogic
{
    public class CourseService : ICourseService
    {
        ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CreateCourseViewModel viewModel)
        {
            var course = new Course();
            MapCourse(course, viewModel);

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            return course.Id;
        }

        public async Task Update(UpdateCourseViewModel viewModel)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(item => item.Id == viewModel.Id);
            DbNullReferenceException.ThrowExceptionIfNull(course, nameof(viewModel.Id), viewModel.Id.ToString());

            MapCourse(course, viewModel);

            await _context.SaveChangesAsync();
        }

        public async Task<GetCourseViewModel> Get(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(item => item.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(course, nameof(id), id.ToString());

            return ExtractViewModel(course);
        }

        private void MapCourse(Course course, BaseCourseViewModel viewModel)
        {
            course.Name = viewModel.Name;
            course.Description = viewModel.Description;
        }

        private GetCourseViewModel ExtractViewModel(Course item) => new GetCourseViewModel()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            CreatedAt = item.CreatedAt
        };

        public async Task<IEnumerable<GetCourseSelectorViewModel>> GetSelector()
        {
            var responce = await _context.Courses
                .Where(item => item.DeletedAt == DateTime.MinValue)
                .Select(item => new GetCourseSelectorViewModel()
                {
                    Id = item.Id,
                    Name = item.Name
                }).ToListAsync();

            return responce;
        }

        public async Task<ApiResultViewModel<GetCourseViewModel>> GetApiResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests)
        {
            var dataQuery = _context.Courses.AsQueryable();
            var dbResponce = await ApiResult<GetCourseViewModel>.CreateAsync(item => new GetCourseViewModel()
            {
                CreatedAt = item.CreatedAt,
                Description = item.Description,
                Id = item.Id,
                Name = item.Name,
            }, dataQuery, pageIndex, pageSize, sortingRequests, filterRequests);

            return dbResponce;
        }
    }
}