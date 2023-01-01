using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Api.Auth
{
    public interface IApiAuthenticationTokenProvider
    {
        string Provide(IdentityUser user);
    }
}
