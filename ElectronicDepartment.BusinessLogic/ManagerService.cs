using ElectronicDepartment.BusinessLogic.Helpers;
using ElectronicDepartment.Common.Exceptions;
using ElectronicDepartment.DataAccess;
using ElectronicDepartment.DomainEntities;
using ElectronicDepartment.Web.Shared.User;
using ElectronicDepartment.Web.Shared.User.Manager;
using ElectronicDepartment.Web.Shared.User.Student;
using ElectronicDepartment.Web.Shared.User.Student.Responce;
using ElectronicDepartment.Web.Shared.User.Teacher;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ElectronicDepartment.Common.Constants;
using ElectronicDepartment.Web.Shared.Common;
using ElectronicDepartment.Interfaces;
using ElectronicDepartment.Web.Shared;
using ElectronicDepartment.Web.Shared.User.Teacher.Responce;
using ElectronicDepartment.Web.Shared.User.Manager.Responce;
using ElectronicDepartment.Web.Shared.Login;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ElectronicDepartment.BusinessLogic
{
    public partial class ManagerService : ApplicationUserService, IUserManagerService
    {
        public ApplicationDbContext _context;
        public UserManager<ApplicationUser> _userManager;
        public IPasswordHasher<ApplicationUser> _passwordHasher;
        public IPasswordValidator<ApplicationUser> _passwordValidator;
        public RoleManager<IdentityRole> _roleManager;

        public ManagerService(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IPasswordValidator<ApplicationUser> passwordValidator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
        }

        public async Task<ApiResultViewModel<GetTeacherViewModel>> GetApiTeacherResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests)
        {
            var dataQuery = _context.Teachers.AsQueryable();
            var dbResponce = await ApiResult<GetTeacherViewModel>.CreateAsync(item => new GetTeacherViewModel()
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                AcademicAcredition = item.AcademicAcredition,
                BirthDay = item.BirthDay,
                Email = item.Email,
                FirstName = item.FirstName,
                Gender = item.Gender,
                LastName = item.LastName,
                MiddleName = item.MiddleName,
                PhoneNumber = item.PhoneNumber
            }, dataQuery, pageIndex, pageSize, sortingRequests, filterRequests);

            return dbResponce;
        }

        public async Task<ApiResultViewModel<GetStudentViewModel>> GetApiStudentResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests)
        {
            var dataQuery = _context.Students.AsQueryable();
            var dbResponce = await ApiResult<GetStudentViewModel>.CreateAsync(item => new GetStudentViewModel()
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                GroupId = item.GroupId,
                BirthDay = item.BirthDay,
                Email = item.Email,
                FirstName = item.FirstName,
                Gender = item.Gender,
                LastName = item.LastName,
                MiddleName = item.MiddleName,
                PhoneNumber = item.PhoneNumber
            }, dataQuery, pageIndex, pageSize, sortingRequests, filterRequests);

            return dbResponce;
        }

        public async Task<ApiResultViewModel<GetManagerViewModel>> GetApiManagerResponce(int pageIndex, int pageSize, IEnumerable<SortingRequest> sortingRequests, IEnumerable<FilterRequest> filterRequests)
        {
            var dataQuery = _context.Managers.AsQueryable();
            var dbResponce = await ApiResult<GetManagerViewModel>.CreateAsync(item => new GetManagerViewModel()
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                BirthDay = item.BirthDay,
                Email = item.Email,
                FirstName = item.FirstName,
                Gender = item.Gender,
                LastName = item.LastName,
                MiddleName = item.MiddleName,
                PhoneNumber = item.PhoneNumber
            }, dataQuery, pageIndex, pageSize, sortingRequests, filterRequests);

            return dbResponce;
        }

        public async Task UpdatePassword(string userId, string password, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            DbNullReferenceException.ThrowExceptionIfNull(user, nameof(userId), userId);

            var result = await _userManager.ChangePasswordAsync(user, password, newPassword);

            if (result.Succeeded)
            {
                return;
            }

            throw new Exception(result.Errors.Select(item => item.Description).Aggregate("", (res, item) => res += item));
        }

        public async Task UpdatePassword(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            DbNullReferenceException.ThrowExceptionIfNull(user, nameof(userId), userId);

            var result = await _passwordValidator.ValidateAsync(_userManager, user, newPassword);

            if (result.Succeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
                await _userManager.UpdateAsync(user);
            }

            throw new Exception(result.Errors.Select(item => item.Description).Aggregate("", (res, item) => res += item));
        }

        public async Task<LoginResult> Login(LoginModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            DbNullReferenceException.ThrowExceptionIfNull(user, nameof(login.Email), login.Email);

            var userRole = await _context.UserRoles.FirstOrDefaultAsync(item => item.UserId == user.Id);
            var role = await _context.Roles.FirstOrDefaultAsync(item => item.Id == userRole.RoleId);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login.Email), new Claim(ClaimTypes.Role, role.Name) };

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: DateTime.Now,
                    claims: claims,
                    expires: DateTime.Now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var result = await _passwordValidator.ValidateAsync(_userManager, user, login.Password);

            if (result.Succeeded)
            {
                var loginResult = new LoginResult()
                {
                    Email = login.Email,
                    Message = "",
                    Success = true,
                    Role = role.Name,
                    JWTBearer = encodedJwt
                };

                return loginResult;
            }

            throw new Exception(result.Errors.FirstOrDefault().Description);
        }
    }

    //Student
    public partial class ManagerService
    {
        public async Task<BaseStudentViewModel> GetStudent(string id)
        {
            var item = await _context.Students.FirstOrDefaultAsync(item => item.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(item, nameof(id), id.ToString());

            return new BaseStudentViewModel()
            {
                GroupId = item.GroupId,
                BirthDay = item.BirthDay,
                Email = item.Email,
                FirstName = item.FirstName,
                Gender = item.Gender,
                LastName = item.LastName,
                MiddleName = item.MiddleName,
                PhoneNumber = item.PhoneNumber
            };
        }

        public async Task<string> CreateStudent(CreateStudentViewModel viewModel)
        {
            var student = new Student();
            Map(student, viewModel);

            var result = await _userManager.CreateAsync(student, "a" + "A" + "!" + student.GetHashCode().ToString());

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(student, STUDENTROLE);
            }

            await _context.SaveChangesAsync();

            return student.Id;
        }

        public async Task UpdateStudent(UpdateStudentViewModel viewModel)
        {
            var student = await _context.Students.FirstOrDefaultAsync(item => item.Id == viewModel.Id);
            DbNullReferenceException.ThrowExceptionIfNull(student, nameof(viewModel.Id), viewModel.Id);

            await ValidateGroup(viewModel);

            Map(student, viewModel);

            await _context.SaveChangesAsync();
        }

        public async Task<GetStudentViewModel> Get(string id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(item => item.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(student, nameof(id), id);

            return ExtractViewModel(student);
        }

        private async Task ValidateGroup(BaseStudentViewModel viewModel)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(item => item.Id == viewModel.GroupId);
            DbNullReferenceException.ThrowExceptionIfNull(group, nameof(viewModel.GroupId), viewModel.GroupId.ToString());
        }

        private void Map(Student student, BaseStudentViewModel viewModel)
        {
            MapApplicationUser(student, viewModel);
            student.UserType = UserType.Teacher;
            student.GroupId = viewModel.GroupId;
        }

        private GetStudentViewModel ExtractViewModel(Student student) => new GetStudentViewModel()
        {
            Id = student.Id,
            FirstName = student.FirstName,
            MiddleName = student.MiddleName,
            LastName = student.LastName,
            BirthDay = student.BirthDay,
            Email = student.Email,
            Gender = student.Gender,
            GroupId = student.GroupId,
            PhoneNumber = student.PhoneNumber
        };
    }

    //Teacher
    public partial class ManagerService
    {
        public async Task<string> CreateTeacher(CreateTeacherViewModel viewModel)
        {
            await ValidateCafedra(viewModel);

            var teacher = new Teacher();
            Map(teacher, viewModel);

            var result = await _userManager.CreateAsync(teacher, "a" + "A" + "!" + teacher.GetHashCode().ToString());

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(teacher, TEACHERROLE);
            }
            else
            {
                throw new Exception(result.Errors.FirstOrDefault().Description?.ToString());
            }

            return teacher.Id;
        }

        public async Task<BaseTeacherViewModel> GetTeacher(string id)
        {
            var item = await _context.Teachers.FirstOrDefaultAsync(item => item.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(item, nameof(id), id.ToString());

            return new BaseTeacherViewModel()
            {
                AcademicAcredition = item.AcademicAcredition,
                CafedraId = item.CafedraId,
                BirthDay = item.BirthDay,
                Email = item.Email,
                FirstName = item.FirstName,
                Gender = item.Gender,
                LastName = item.LastName,
                MiddleName = item.MiddleName,
                PhoneNumber = item.PhoneNumber
            };
        }

        public async Task UpdateTeacher(UpdateTeacherViewModel viewModel)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(item => item.Id == viewModel.Id);
            DbNullReferenceException.ThrowExceptionIfNull(teacher, nameof(viewModel.Id), viewModel.Id.ToString());

            await ValidateCafedra(viewModel);

            Map(teacher, viewModel);

            await _context.SaveChangesAsync();
        }

        private async Task ValidateCafedra(BaseTeacherViewModel viewModel)
        {
            var cafedra = await _context.Cafedras.FirstOrDefaultAsync(item => item.Id == viewModel.CafedraId);
            DbNullReferenceException.ThrowExceptionIfNull(cafedra, nameof(viewModel.CafedraId), viewModel.CafedraId.ToString());
        }

        private void Map(Teacher teacher, BaseTeacherViewModel viewModel)
        {
            MapApplicationUser(teacher, viewModel);
            teacher.UserType = UserType.Teacher;
            teacher.CafedraId = viewModel.CafedraId;
            teacher.AcademicAcredition = viewModel.AcademicAcredition;
        }
    }

    //Manager
    public partial class ManagerService
    {
        public async Task<string> CreateManager(CreateManagerViewModel viewModel)
        {
            var manager = new Manager();
            Map(manager, viewModel);

            var result = await _userManager.CreateAsync(manager, "a" + "A" + "!" + manager.GetHashCode().ToString());

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(manager, MANAGERROLE);
            }

            return manager.Id;
        }

        public async Task UpdateManager(UpdateManagerViewModel viewModel)
        {
            var manager = await _context.Managers.FirstOrDefaultAsync(item => item.Id == viewModel.Id);
            DbNullReferenceException.ThrowExceptionIfNull(manager, nameof(viewModel.Id), viewModel.Id.ToString());

            Map(manager, viewModel);

            await _context.SaveChangesAsync();
        }

        public async Task<BaseManagerViewModel> GetManager(string id)
        {
            var item = await _context.Managers.FirstOrDefaultAsync(item => item.Id == id);
            DbNullReferenceException.ThrowExceptionIfNull(item, nameof(id), id.ToString());

            return new BaseManagerViewModel()
            {
                BirthDay = item.BirthDay,
                Email = item.Email,
                FirstName = item.FirstName,
                Gender = item.Gender,
                LastName = item.LastName,
                MiddleName = item.MiddleName,
                PhoneNumber = item.PhoneNumber
            };
        }

        private void Map(Manager manager, BaseManagerViewModel viewModel)
        {
            MapApplicationUser(manager, viewModel);
            manager.UserType = UserType.Manager;
        }
    }

    
}