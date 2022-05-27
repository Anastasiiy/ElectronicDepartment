using ElectronicDepartment.DataAccess;
using ElectronicDepartment.DomainEntities;
using ElectronicDepartment.Web.Shared.Mark;
using ElectronicDepartment.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using ElectronicDepartment.Web.Shared.Mark.Responce;
using ElectronicDepartment.Interfaces;
using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;

namespace ElectronicDepartment.BusinessLogic
{
    public class StudentOnLessonService : IStudentOnLessonService
    {
        ApplicationDbContext _context;

        public StudentOnLessonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CreateMarkViewModel viewModel)
        {
            await Validate(viewModel);

            var mark = await _context.Marks
                .FirstOrDefaultAsync(item => item.LessonId == viewModel.LessonId 
                && item.StudentId == viewModel.StudentId);

            if(mark is null)
            {
                mark = new StudentOnLesson();
                Map(mark, viewModel);

                await _context.Marks.AddAsync(mark);
                await _context.SaveChangesAsync();
            }

            return mark.Id;
        }

        public async Task Update(UpdateMarkViewModel viewModel)
        {
            var mark = await _context.Marks.FirstOrDefaultAsync(item => item.Id == viewModel.Id);
            DbNullReferenceException.ThrowExceptionIfNull(mark, nameof(viewModel.Id), viewModel.Id.ToString());

            await Validate(viewModel);
            Map(mark, viewModel);

            await _context.SaveChangesAsync();
        }

        public async Task<GetMarkResponce> Get(int id)
        {
            var mark = await _context.Marks.FirstOrDefaultAsync(item => item.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(mark, nameof(id), id.ToString());

            return ExtractMarkResponce(mark);
        }

        public Task GetAllMark()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GetStudentOnTheLessonViewModel>> GetStudentsWithMarkViewModel(int id)
        {
            var data = await _context.Marks
                .Where(item => item.DeletedAt == DateTime.MinValue)
                .Where(item => item.LessonId == id)
                .Select(item => new
                {
                    item.Id,
                    item.StudentId,
                    item.Student.FirstName,
                    item.Student.MiddleName,
                    item.Student.LastName,
                    item.Student.GroupId,
                    item.Student.Group.Name,
                    item.Mark
                }).ToArrayAsync();

            var responce = data.Select(item => new GetStudentOnTheLessonViewModel()
            {
                Id = item.Id,
                StudentGroupId = item.GroupId,
                StudentId = item.StudentId,
                StudentGroupName = item.Name,
                Mark = item.Mark,
                StudentFullName = $"{item.FirstName} {item.MiddleName} {item.LastName}"
            });

            return responce;
        }

        public async Task<IEnumerable<GetStudentSelectViewModel>> GetStudentSelector()
        {
            var data = await _context.Students
                .Where(item => item.DeletedAt == DateTime.MinValue)
                .Select(item => new
                {
                    item.Id,
                    item.Email,
                    item.FirstName,
                    item.MiddleName,
                    item.LastName,
                    item.GroupId,
                    item.Group.Name,
                    LessonIds = item.StudentOnLessons.Select(item => item.LessonId)
                }).ToListAsync();

            var responce = data.Select(item => new GetStudentSelectViewModel()
            {
                Id = item.Id,
                GroupId = item.GroupId,
                GroupName = item.Name,
                LessonIds = item.LessonIds.ToList(),
                FullName = $"{item.FirstName} {item.MiddleName} {item.LastName}"
            });

            return responce;
        }

        public async Task<ApiResultViewModel<GetMarkResponce>> GetApiMarkResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests)
        {
            var dataQuery = _context.Marks.AsQueryable();
            var dbResponce = await ApiResult<GetMarkResponce>.CreateAsync(item => new GetMarkResponce()
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                LessonId = item.LessonId,
                StudentId = item.StudentId,
                FirstName = item.Student.FirstName,
                LastName = item.Student.LastName,
                MiddleName = item.Student.MiddleName,
                Value = item.Mark
            }, dataQuery, pageIndex, pageSize, sortingRequests, filterRequests);

            return dbResponce;
        }

        private GetMarkResponce ExtractMarkResponce(StudentOnLesson item) => new GetMarkResponce()
        {
            Id = item.Id,
            StudentId = item.StudentId,
            LessonId = item.LessonId,
            CreatedAt = item.CreatedAt,
            Value = item.Mark
        };

        private async Task Validate(BaseMarkViewModel viewModel)
        {
            await ValidateLesson(viewModel);
            await ValidateStudent(viewModel);
        }

        private async Task ValidateLesson(BaseMarkViewModel viewModel)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(item => item.Id == viewModel.LessonId);
            DbNullReferenceException.ThrowExceptionIfNull(lesson, nameof(viewModel.LessonId), viewModel.LessonId.ToString());
        }

        private async Task ValidateStudent(BaseMarkViewModel viewModel)
        {
            var student = await _context.Students.FirstOrDefaultAsync(item => item.Id == viewModel.StudentId);
            DbNullReferenceException.ThrowExceptionIfNull(student, nameof(viewModel.StudentId), viewModel.StudentId.ToString());
        }

        private void Map(StudentOnLesson mark, BaseMarkViewModel viewModel)
        {
            mark.Mark = viewModel.Value;
            mark.StudentId = viewModel.StudentId;
            mark.LessonId = viewModel.LessonId;
        }

        public async Task Remove(int id)
        {
            var mark = await _context.Marks.FirstOrDefaultAsync(item => item.Id == id);

            if(mark is not null)
            {
                _context.Remove(mark);
                await _context.SaveChangesAsync();
            }
        }
    }
}