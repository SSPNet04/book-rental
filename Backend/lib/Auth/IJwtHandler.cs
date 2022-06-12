using System;
using System.Collections.Generic;
using System.Text;

namespace Auth
{
    public interface IJwtHandler
    {
        ClientJsonWebToken ClientCreate(string userId, string displayName);
        JsonWebToken Create(string username, string displayName, string refreshToken = null, bool mustChangePassword = false);
    }
}
