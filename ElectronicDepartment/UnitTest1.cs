using ElectronicDepartment.BusinessLogic;
using ElectronicDepartment.DataAccess;
using ElectronicDepartment.DomainEntities;
using ElectronicDepartment.Interfaces;
using ElectronicDepartment.Web.Shared.Common;
using ElectronicDepartment.Web.Shared.Group;
using Microsoft.EntityFrameworkCore;

namespace ElectronicDepartment
{
    public class GroupServiceTest
    {
        [Fact]
        public async Task CreateGroupViewModelValidData()
        {
            // Arrange 
            var testHelper = new TestHelper();
            var DbContext = testHelper.GetInMemoryRepo();
            IGroupService service = new GroupService(DbContext);
            //Act

            var resId = await service.Create(new CreateGroupViewModel()
            {
                Name = "Bor 5 Z"
            });

            var res = await DbContext.Groups.FirstOrDefaultAsync(item => item.Id == resId);

            //Assert
            Assert.NotNull(res);
        }

        [Fact]
        public async Task GetApiResponce()
        {
            // Arrange 
            var testHelper = new TestHelper();
            var DbContext = testHelper.GetInMemoryRepo();
            IGroupService service = new GroupService(DbContext);

            DbContext.Groups.AddRange(new List<Group>()
            {
                new Group(){Id = 1, Name = "1"},
                new Group(){Id = 2, Name = "2"},
                new Group(){Id = 3, Name = "3"},
                new Group(){Id = 4, Name = "4"},
                new Group(){Id = 5, Name = "5"},
                new Group(){Id = 6, Name = "6"},
                new Group(){Id = 7, Name = "7"},
                new Group(){Id = 8, Name = "8"},
                new Group(){Id = 9, Name = "8"},
                new Group(){Id = 10, Name = "8"},
                new Group(){Id = 11, Name = "8"},

            });

            await DbContext.SaveChangesAsync();

            //Act
            var res = await service.GetApiResponce(0, 5, new List<SortingRequest>(), new List<FilterRequest>());

            //Assert
            Assert.Equal(5, res.PageSize);
            Assert.Equal(0, res.PageIndex);
            Assert.Equal(11, res.TotalCount);
            Assert.Equal(3, res.TotalPages);
        }

        [Fact]
        public async Task FilterGetApiResponce()
        {
            // Arrange 
            var testHelper = new TestHelper();
            var DbContext = testHelper.GetInMemoryRepo();
            IGroupService service = new GroupService(DbContext);

            DbContext.Groups.AddRange(new List<Group>()
            {
                new Group(){Id = 1, Name = "1"},
                new Group(){Id = 2, Name = "2"},
                new Group(){Id = 3, Name = "3"},
                new Group(){Id = 4, Name = "4"},
                new Group(){Id = 5, Name = "5"},
                new Group(){Id = 6, Name = "6"},
                new Group(){Id = 7, Name = "7"},
                new Group(){Id = 8, Name = "8"},
                new Group(){Id = 9, Name = "8"},
                new Group(){Id = 10, Name = "8"},
                new Group(){Id = 11, Name = "8"},
            });

            var filter = new FilterRequest()
            {
                FilterColumn = "Name",
                FilterQuery = "8",
                IsPartFilter = true
            };

            await DbContext.SaveChangesAsync();

            //Act
            var res = await service.GetApiResponce(0, 5, new List<SortingRequest>(), new List<FilterRequest>() { filter });

            //Assert
            Assert.Equal(5, res.PageSize);
            Assert.Equal(0, res.PageIndex);
            Assert.Equal(4, res.TotalCount);
            Assert.Equal(1, res.TotalPages);
        }

        [Fact]
        public async Task SortGetApiResponce()
        {
            // Arrange 
            var testHelper = new TestHelper();
            var DbContext = testHelper.GetInMemoryRepo();
            IGroupService service = new GroupService(DbContext);

            DbContext.Groups.AddRange(new List<Group>()
            {
                new Group(){Id = 1, Name = "1"},
                new Group(){Id = 2, Name = "2"},
                new Group(){Id = 3, Name = "3"},
                new Group(){Id = 4, Name = "4"},
                new Group(){Id = 5, Name = "5"},
                new Group(){Id = 6, Name = "6"},
                new Group(){Id = 7, Name = "7"},
                new Group(){Id = 8, Name = "8"},
                new Group(){Id = 9, Name = "8"},
                new Group(){Id = 10, Name = "8"},
                new Group(){Id = 11, Name = "8"},
            });

            var sort = new SortingRequest()
            {
                SortColumn = "Id",
                SortOrder = "DESC"
            };

            await DbContext.SaveChangesAsync();

            //Act
            var res = await service.GetApiResponce(0, 10, new List<SortingRequest>() { sort }, null);

            //Assert
            Assert.Equal(11, res.Data.First().Id);

        }
    }
}


public class TestHelper
{
    private readonly ApplicationDbContext _context;

    public TestHelper()
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseInMemoryDatabase(databaseName: "InMemoryDb");

        var dbContextOptions = builder.Options;
        _context = new ApplicationDbContext(dbContextOptions);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    public ApplicationDbContext GetInMemoryRepo()
    {
        return _context;
    }

}

