using ElectronicDepartment.BusinessLogic.Helpers;
using ElectronicDepartment.Common.Exceptions;
using ElectronicDepartment.DataAccess;
using ElectronicDepartment.DomainEntities;
using ElectronicDepartment.Web.Shared.Group;
using ElectronicDepartment.Web.Shared.Group.Responce;
using Microsoft.EntityFrameworkCore;
using ElectronicDepartment.Interfaces;
using ElectronicDepartment.Web.Shared.Mark.Responce;
using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;

namespace ElectronicDepartment.BusinessLogic
{
    public class GroupService : IGroupService
    {
        ApplicationDbContext _context;

        public GroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CreateGroupViewModel viewModel)
        {
            var group = new Group();
            Map(group, viewModel);

            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();

            return group.Id;
        }

        public async Task<GetGroupViewModel> Get(int id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(group, nameof(id), id.ToString());

            var dataQuery = _context
                .Students
                .AsQueryable()
                .Where(student => student.GroupId == group.Id);

            var result = ExtractViewModel(group);
            
            return result;
        }

        public async Task Update(UpdateGroupViewModel viewModel)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(item => item.Id == viewModel.Id);
            DbNullReferenceException.ThrowExceptionIfNull(group, nameof(viewModel.Id), viewModel.Id.ToString());

            Map(group, viewModel);

            await _context.SaveChangesAsync();
        }

        private void Map(Group group, BaseGroupViewModel viewModel)
        {
            group.Name = viewModel.Name;
        }

        private GetGroupViewModel ExtractViewModel(Group group)
        {
            return new GetGroupViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                CreatedAt = group.CreatedAt,
            };
        }

        public async Task<IEnumerable<GetGroupSelectorViewModel>> GetSelector()
        {
            var responce = await _context.Groups
                            .Where(item => item.DeletedAt == DateTime.MinValue)
                            .Select(item => new GetGroupSelectorViewModel()
                            {
                                Id = item.Id,
                                Name = item.Name
                            }).ToListAsync();

            return responce;
        }

        public async Task<ApiResultViewModel<GetGroupViewModel>> GetApiResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests)
        {
            var dataQuery = _context.Groups.AsQueryable();
            var dbResponce = await ApiResult<GetGroupViewModel>.CreateAsync(item => new GetGroupViewModel()
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                Name = item.Name,
                StudentCount = item.Students.Count()
            }, dataQuery, pageIndex, pageSize, sortingRequests, filterRequests);

            return dbResponce;
        }
    }
}