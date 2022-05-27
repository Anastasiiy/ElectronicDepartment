using ElectronicDepartment.Common.Exceptions;
using ElectronicDepartment.DataAccess;
using ElectronicDepartment.DomainEntities;
using ElectronicDepartment.Web.Shared.CourseTeacher;
using ElectronicDepartment.Web.Shared.CourseTeacher.Responce;
using Microsoft.EntityFrameworkCore;
using ElectronicDepartment.Interfaces;
using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;

namespace ElectronicDepartment.BusinessLogic
{
    public class CourseTeacherService : ICourseTeacherService
    {
        ApplicationDbContext _context;

        public CourseTeacherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<int>> CreateRange(IEnumerable<CreateCourseTeacherViewModel> viewModel)
        {
            var responce = new List<int>();

            foreach (var item in viewModel)
            {
                var entity = await GetOrCreateDbEntity(item);
                responce.Add(entity.Id);
            }

            await _context.SaveChangesAsync();

            return responce;
        }

        public async Task<int> Create(CreateCourseTeacherViewModel viewModel)
        {
            var entity = GetOrCreateDbEntity(viewModel);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        protected async Task<CourseTeacher> GetOrCreateDbEntity(CreateCourseTeacherViewModel viewModel)
        {
            await Validate(viewModel);

            var courseTeacher = await _context.CourseTeachers
                .FirstOrDefaultAsync(item => item.CourseId == viewModel.CourseId
                && item.TeacherId == viewModel.TeacherId);

            if (courseTeacher is null)
            {
                courseTeacher = new CourseTeacher();
                Map(courseTeacher, viewModel);

                await _context.AddAsync(courseTeacher);
            }

            return courseTeacher;
        }

        protected async Task Validate(BaseCourseTeacherViewModel viewModel)
        {
            await ValidateTeacher(viewModel);
            await ValidateCourse(viewModel);
        }

        protected async Task ValidateTeacher(BaseCourseTeacherViewModel viewModel)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(item => item.Id == viewModel.TeacherId.ToString());
            DbNullReferenceException.ThrowExceptionIfNull(teacher, nameof(viewModel.TeacherId), viewModel.TeacherId.ToString());
        }

        protected async Task ValidateCourse(BaseCourseTeacherViewModel viewModel)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(item => item.Id == viewModel.CourseId);
            DbNullReferenceException.ThrowExceptionIfNull(course, nameof(viewModel.CourseId), viewModel.CourseId.ToString());
        }

        public async Task Update(UpdateCourseTeacherViewModel viewModel)
        {
            var courseTeacher = await _context.CourseTeachers.FirstOrDefaultAsync(item => item.Id == viewModel.Id);
            DbNullReferenceException.ThrowExceptionIfNull(courseTeacher, nameof(viewModel.Id), viewModel.Id.ToString());
            await Validate(viewModel);

            var anotherTeacher = await _context.CourseTeachers
                .FirstOrDefaultAsync(item => item.CourseId == viewModel.CourseId
                && item.TeacherId == viewModel.TeacherId);

            if(anotherTeacher is null)
            {
                Map(courseTeacher, viewModel);
                await _context.SaveChangesAsync();
            }

            throw new Exception($"Teacher already in this course");
        }

        public async Task<GetCourseTeacherViewModel> Get(int id)
        {
            var courseTeacher = await _context.CourseTeachers.FirstOrDefaultAsync(item => item.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(courseTeacher, nameof(id), id.ToString());

            return ExtractViewModel(courseTeacher);
        }

        protected GetCourseTeacherViewModel ExtractViewModel(CourseTeacher item) => new GetCourseTeacherViewModel()
        {
            Id = item.Id,
            CourseId = item.CourseId,
            TeacherId = item.TeacherId,
            CreatedAt = item.CreatedAt
        };

        protected void Map(CourseTeacher courseTeacher, BaseCourseTeacherViewModel viewModel)
        {
            courseTeacher.CourseId = viewModel.CourseId;
            courseTeacher.TeacherId = viewModel.TeacherId;
        }

        public async Task<IEnumerable<GetCourseTeacherSelectorViewModel>> GetSelector()
        {
            var responce = await _context.Teachers
                            .Where(item => item.DeletedAt == DateTime.MinValue)
                            .Select(item => new GetCourseTeacherSelectorViewModel
                            {
                                Id = item.Id,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                MiddleName = item.MiddleName,
                                AcademicAcredition = item.AcademicAcredition,
                            }).ToListAsync();

            return responce;
        }

        public async Task RemoveTeacher(int id)
        {
            await Delete(id);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTeacherRange(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                await Delete(id);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ApiResultViewModel<GetCourseTeacherViewModel>> GetApiCourseTeacherResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests)
        {
            var dataQuery = _context.CourseTeachers.AsQueryable();
            var dbResponce = await ApiResult<GetCourseTeacherViewModel>.CreateAsync(item => new GetCourseTeacherViewModel()
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                CourseId = item.CourseId,
                TeacherId = item.TeacherId
            }, dataQuery, pageIndex, pageSize, sortingRequests, filterRequests);

            return dbResponce;
        }

        protected async Task Delete(int id)
        {
            var course = await _context.CourseTeachers.FirstOrDefaultAsync(item => item.Id == id);

            if (course is not null)
            {
                _context.CourseTeachers.Remove(course);
            }
        }
    }
}