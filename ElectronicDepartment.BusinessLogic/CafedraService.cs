using ElectronicDepartment.Common.Exceptions;
using ElectronicDepartment.DataAccess;
using ElectronicDepartment.DomainEntities;
using ElectronicDepartment.Web.Shared.Cafedra;
using ElectronicDepartment.Web.Shared.Cafedra.Responce;
using Microsoft.EntityFrameworkCore;
using ElectronicDepartment.Interfaces;
using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.Common;

namespace ElectronicDepartment.BusinessLogic
{
    public class CafedraService : ICafedraService
    {
        ApplicationDbContext _context;

        public CafedraService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CreateCafedraViewModel viewModel)
        {
            var cafedra = new Cafedra();
            Map(cafedra, viewModel);

            await _context.AddAsync(cafedra);
            await _context.SaveChangesAsync();

            return cafedra.Id;
        }

        public async Task<GetCafedraViewModel> Get(int id)
        {
            var cafedra = await _context.Cafedras.FirstOrDefaultAsync(item => item.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(cafedra, nameof(id), id.ToString());

            return ExctractViewModel(cafedra);
        }

        public async Task<IEnumerable<GetCafedraSelectorViewModel>> GetSelector()
        {
            var responce = await _context.Cafedras
                .Where(item => item.DeletedAt == DateTime.MinValue)
                .Select(item => new GetCafedraSelectorViewModel()
            {
                Id = item.Id,
                Name = item.Name
            }).ToListAsync();

            return responce;
        }

        public async Task Update(UpdateCafedraViewModel viewModel)
        {
            var cafedra = await _context.Cafedras.FirstOrDefaultAsync(item => item.Id == viewModel.Id);
            DbNullReferenceException.ThrowExceptionIfNull(cafedra, nameof(viewModel.Id), viewModel.Id.ToString());

            Map(cafedra, viewModel);

            await _context.SaveChangesAsync();
        }

        private GetCafedraViewModel ExctractViewModel(Cafedra cafedra) => new GetCafedraViewModel()
        {
            Id = cafedra.Id,
            Name = cafedra.Name,
            Description = cafedra.Description,
            CreatedAt = cafedra.CreatedAt,
            Phone = cafedra.Phone
        };

        private void Map(Cafedra cafedra, BaseCafedraViewModel viewModel)
        {
            cafedra.Description = viewModel.Description;
            cafedra.Name = viewModel.Name;
            cafedra.Phone = viewModel.Phone;
        }

        public async Task<ApiResultViewModel<GetCafedraViewModel>> GetApiResponce(int pageIndex,
            int pageSize,
            IEnumerable<SortingRequest> sortingRequests = null,
            IEnumerable<FilterRequest> filterRequests = null)
        {
            var dataQuery = _context.Cafedras.AsQueryable();
            var dbResponce = await ApiResult<GetCafedraViewModel>.CreateAsync(item => new GetCafedraViewModel()
            {
                CreatedAt = item.CreatedAt,
                Description = item.Description,
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone
            }, dataQuery, pageIndex, pageSize, sortingRequests, filterRequests);

            return dbResponce;
        }
    }
}