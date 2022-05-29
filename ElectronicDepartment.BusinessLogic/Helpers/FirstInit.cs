using ElectronicDepartment.Common.Enums;
using ElectronicDepartment.DataAccess;
using ElectronicDepartment.DomainEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ElectronicDepartment.Common.Constants;

namespace ElectronicDepartment.BusinessLogic.Helpers
{
    public static class FirstInit
    {
        public static async Task InitRoles(RoleManager<IdentityRole> _roleManager, IEnumerable<string> roles)
        {
            foreach (var item in roles)
            {
                var res = await _roleManager.FindByNameAsync(item);

                if (res is null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(item));
                }
            }
        }

        public static async Task InitStartData(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            await InitManager(userManager);
            await InitAdmin(userManager);

            await InitStudent(userManager, dbContext);
            await InitTeacher(userManager, dbContext);
        }

        public static async Task InitStudent(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            var password = "aStudent123!";
            var student = new Student()
            {
                BirthDay = DateTime.Now,
                Email = @"Student@gmail.com",
                FirstName = "Student",
                MiddleName = "Student",
                LastName = "Student",
                Gender = Gender.None,
                UserName = @"Student",
                EmailConfirmed = true,
                UserType = UserType.Student,
                PhoneNumber = "++3777777",
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(student.Email);

            if (user is null)
            {
                await InitGroup(dbContext);
                student.Group = await dbContext.Groups.FirstOrDefaultAsync();

                var result = await userManager.CreateAsync(student, password);
            }

            var roles = await userManager.GetRolesAsync(student);

            if (roles.Count() == 0)
            {
                await userManager.AddToRoleAsync(student, STUDENTROLE);
            }
        }

        public static async Task InitManager(UserManager<ApplicationUser> userManager)
        {
            var password = "aManager123!";
            var manager = new Manager()
            {
                BirthDay = DateTime.Now,
                Email = @"Manager@gmail.com",
                FirstName = "Manager",
                MiddleName = "Manager",
                LastName = "Manager",
                Gender = Gender.None,
                UserName = @"Manager",
                EmailConfirmed = true,
                UserType = UserType.Manager,
                PhoneNumber = "++35555555",
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(manager.Email);

            if (user is null)
            {
                var result = await userManager.CreateAsync(manager, password);
            }

            var roles = await userManager.GetRolesAsync(manager);

            if (roles.Count() == 0)
            {
                await userManager.AddToRoleAsync(manager, MANAGERROLE);
            }
        }

        public static async Task InitTeacher(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            var password = "aTeacher123!";
            var teacher = new Teacher()
            {
                BirthDay = DateTime.Now,
                Email = @"Teacher@gmail.com",
                FirstName = "Teacher",
                MiddleName = "Teacher",
                LastName = "Teacher",
                Gender = Gender.None,
                UserName = @"Teacher",
                EmailConfirmed = true,
                UserType = UserType.Teacher,
                PhoneNumber = "++389999999",
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(teacher.Email);

            if (user is null)
            {
                await InitCafedra(dbContext);
                var cafedra = await dbContext.Cafedras.FirstOrDefaultAsync();

                var result = await userManager.CreateAsync(teacher, password);
            }

            var roles = await userManager.GetRolesAsync(teacher);

            if (roles.Count() == 0)
            {
                await userManager.AddToRoleAsync(teacher, TEACHERROLE);
            }
        }

        public static async Task InitAdmin(UserManager<ApplicationUser> userManager)
        {
            var password = "admin123!";
            var admin = new Admin()
            {
                BirthDay = DateTime.Now,
                Email = @"admin123@gmail.com",
                FirstName = "Admin",
                MiddleName = "Admin",
                LastName = "Admin",
                Gender = Gender.None,
                UserName = @"adminAdmin",
                EmailConfirmed = true,
                UserType = UserType.Admin,
                PhoneNumber = "+380111111111",
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(admin.Email);

            if (user is null)
            {
                var result = await userManager.CreateAsync(admin, password);
            }

            var roles = await userManager.GetRolesAsync(admin);

            if (roles.Count() == 0)
            {
                await userManager.AddToRoleAsync(admin, ADMINROLE);
            }
        }

        private static async Task InitGroup(ApplicationDbContext dbContext)
        {
            var group1 = new Group()
            {
                Name = "PP-22-ZZ",
            };


            await dbContext.Groups.AddAsync(group1);

            await dbContext.SaveChangesAsync();
        }

        public static async Task InitCafedra(ApplicationDbContext dbContext)
        {
            var cafedra1 = new Cafedra()
            {
                Name = "Botanica",
                Description = "Cafedra Botanici",
                Phone = "+3809944622",
            };

            await dbContext.Cafedras.AddAsync(cafedra1);

            await dbContext.SaveChangesAsync();
        }
    }
}