
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using Microsoft.Extensions.Configuration;
using DockingAdminPanel.Models;

namespace Sophia.Web.Components
{
    public class MenuSideBar : ViewComponent
    {
    
        private readonly IDataProtector _dataProtector;
      
        private readonly UserManager<User> _userManager;
     
        private readonly IConfiguration _config;
        public MenuSideBar( IDataProtectionProvider dataProtector,  UserManager<User> userManager,IConfiguration config)
        {
           
            _dataProtector = dataProtector.CreateProtector("ProfileEmployeeId");
            _userManager = userManager;
        
            _config = config;
        }

        [Authorize]
        public async System.Threading.Tasks.Task<IViewComponentResult> InvokeAsync()
        {
            UserViewModel userViewModel = new UserViewModel();
            var user = this.HttpContext.User;
            var userid = "";
            var identifier = user.FindFirst(ClaimTypes.NameIdentifier);
            if (identifier != null)
            {
                userid = identifier.Value.ToString();
                
                var Users = await _userManager.FindByIdAsync(userid);

                var roles = await _userManager.GetRolesAsync(Users);
                userViewModel.role = roles.FirstOrDefault();

                
            }
            return View(userViewModel);
        }

    }
}
