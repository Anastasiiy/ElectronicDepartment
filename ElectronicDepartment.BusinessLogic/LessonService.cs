using ElectronicDepartment.Common.Exceptions;
using ElectronicDepartment.DataAccess;
using ElectronicDepartment.DomainEntities;
using ElectronicDepartment.Web.Shared.CourseTeacher;
using ElectronicDepartment.Web.Shared.Lesson;
using ElectronicDepartment.Web.Shared.Lesson.Responce;
using Microsoft.EntityFrameworkCore;
using ElectronicDepartment.Interfaces;
using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;

namespace ElectronicDepartment.BusinessLogic
{
    public class LessonService : CourseTeacherService, ILessonService
    {
        ApplicationDbContext _context;

        public LessonService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CreateLesson(CreateLessonViewModel viewModel)
        {
            await Validate(viewModel);

            var courseTeacher = await GetOrCreateDbEntity(new CreateCourseTeacherViewModel()
            {
                CourseId = viewModel.CourseId,
                TeacherId = viewModel.TeacherId
            });

            var lesson = new Lesson();
            Map(lesson, viewModel, courseTeacher);

            courseTeacher.Lessons.Add(lesson);

            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();

            return lesson.Id;
        }

        private void Map(Lesson lesson, BaseLessonViewModel viewModel, CourseTeacher courseTeacher)
        {
            lesson.LessonType = viewModel.LessonType;
            lesson.LessonStart = viewModel.LessonStart;
            lesson.Duration = viewModel.Duration;
        }

        public async Task UpdateLesson(UpdateLessonViewModel viewModel)
        {
            await Validate(viewModel);

            var lesson = await _context.Lessons.FirstOrDefaultAsync(item => item.Id == viewModel.Id);
            DbNullReferenceException.ThrowExceptionIfNull(lesson, nameof(viewModel.Id), viewModel.Id.ToString());

            var courseTeacher = await GetOrCreateDbEntity(new CreateCourseTeacherViewModel()
            {
                CourseId = viewModel.CourseId,
                TeacherId = viewModel.TeacherId
            });

            Map(lesson, viewModel, courseTeacher);

            courseTeacher.Lessons.Add(lesson);

            try
            {
                var res = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<UpdateLessonViewModel> GetLesson(int id)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(item => item.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(lesson, nameof(id), id.ToString());

            return ExtractViewModel(lesson);
        }

        private UpdateLessonViewModel ExtractViewModel(Lesson item) => new UpdateLessonViewModel()
        {
            Id = item.Id,
            CourseId = item.CourseTeacher.CourseId,
            TeacherId = item.CourseTeacher.TeacherId,
            LessonType = item.LessonType,
            LessonStart = item.LessonStart,
            Duration = item.Duration
        };

        private async Task Validate(BaseLessonViewModel viewModel)
        {
            await ValidateTeacher(viewModel);
            await ValidateCourse(viewModel);
        }

        private async Task ValidateTeacher(BaseLessonViewModel viewModel)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(item => item.Id == viewModel.TeacherId);
            DbNullReferenceException.ThrowExceptionIfNull(teacher, nameof(viewModel.TeacherId), viewModel.TeacherId.ToString());
        }

        private async Task ValidateCourse(BaseLessonViewModel viewModel)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(item => item.Id == viewModel.CourseId);
            DbNullReferenceException.ThrowExceptionIfNull(course, nameof(viewModel.CourseId), viewModel.CourseId.ToString());
        }

        public async Task<IEnumerable<GetCourseLessonViewModel>> GetCourseLessons(int courseId)
        {
            var lessons = await _context.Lessons
                .Where(item => item.DeletedAt == DateTime.MinValue)
                .Where(item => item.CourseTeacher.CourseId == courseId)
                .Select(item => new
                {
                    item.Id,
                    item.CourseTeacher.Teacher.FirstName,
                    item.CourseTeacher.Teacher.MiddleName,
                    item.CourseTeacher.Teacher.LastName,
                    item.CourseTeacher.TeacherId,
                    item.Duration,
                    item.LessonStart,
                    item.LessonType,
                    TotalStudentOnLesson = item.StudentOnLessons
                        .Where(item => item.DeletedAt == DateTime.MinValue)
                        .Count()
                })
                .ToListAsync();

            var result = lessons.Select(item => new GetCourseLessonViewModel()
            {
                Id = item.Id,
                Duration = item.Duration,
                LessonStart = item.LessonStart,
                LessonType = item.LessonType,
                TeacherId = item.TeacherId,
                TeacherFullName = $"{item.FirstName} {item.MiddleName} {item.LastName}",
                TotalStudentOnLesson = item.TotalStudentOnLesson
            });

            return result;
        }

        public async Task<ApiResultViewModel<GetLessonViewModel>> GetApiLessonResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests)
        {
            var dataQuery = _context.Lessons.AsQueryable();
            var dbResponce = await ApiResult<GetLessonViewModel>.CreateAsync(item => new GetLessonViewModel()
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                CourseId = item.CourseTeacher.CourseId,
                CourseName = item.CourseTeacher.Course.Name,
                CourseTeacherId = item.CourseTeacherId,
                CourseTeacher = item.CourseTeacher.Teacher.LastName,
                Duration = item.Duration,
                LessonStart = item.LessonStart,
                LessonType = item.LessonType
            }, dataQuery, pageIndex, pageSize, sortingRequests, filterRequests);

            return dbResponce;
        }

        public async Task DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(item => item.Id == id);

            if(lesson is not null)
            {
                _context.Remove(lesson);
                await _context.SaveChangesAsync();
            }
        }
    }
}