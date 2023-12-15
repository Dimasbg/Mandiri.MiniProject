using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthApp.Domain.UserService.Models
{
    public class UserListParamDomainModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool IsIncludeRole { get; set; }
    }
}
