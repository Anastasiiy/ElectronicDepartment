using ElectronicDepartment.DomainEntities;
using ElectronicDepartment.Web.Shared.User;
using ElectronicDepartment.Interfaces;

namespace ElectronicDepartment.BusinessLogic
{
    public class ApplicationUserService
    {
        protected virtual void MapApplicationUser(in ApplicationUser user, in BaseUserViewModel viewModel)
        {
            user.EmailConfirmed = true;

            user.PhoneNumber = viewModel.PhoneNumber;
            user.Email = viewModel.Email;
            user.UserName = viewModel.Email;
            user.BirthDay = viewModel.BirthDay;
            user.Gender = viewModel.Gender;
            user.FirstName = viewModel.FirstName;
            user.MiddleName = viewModel.MiddleName;
            user.LastName = viewModel.LastName;
        }
    }
}