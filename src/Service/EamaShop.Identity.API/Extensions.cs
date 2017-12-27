using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    /// <summary>
    /// ex
    /// </summary>
    public static class Extensions
    {
        public static long GetId(this ClaimsPrincipal principal)
        {
            return long.Parse(principal.FindFirstValue("Id"));
        }
    }
}
