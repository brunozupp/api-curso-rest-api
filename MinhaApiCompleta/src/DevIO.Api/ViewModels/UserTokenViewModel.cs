using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.ViewModels
{
    public class UserTokenViewModel
    {
        public string Id { get; set; } // Id do usuário

        public string Email { get; set; }

        public IEnumerable<ClaimViewModel> Claims { get; set; }
    }
}
