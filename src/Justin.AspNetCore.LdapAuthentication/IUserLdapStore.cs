using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Justin.AspNetCore.LdapAuthentication
{
    /// <summary>
    /// Represents a user store that can provide the DN for an LDAP user
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public interface IUserLdapStore<TUser>
        where TUser : class
    {
        /// <summary>
        /// When implemented in a derived class, gets the DN that should be used to attempt an LDAP bind for validatio of a user's password.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetDistinguishedNameAsync(TUser user);
    }
}
