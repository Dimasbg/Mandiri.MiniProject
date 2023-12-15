using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthApp.Domain.RoleService.Models
{
    public class RoleListParamDomainModel
    {
        public string? Name { get; set; }
        public bool IsIncludeUser { get; set; }
    }
}
