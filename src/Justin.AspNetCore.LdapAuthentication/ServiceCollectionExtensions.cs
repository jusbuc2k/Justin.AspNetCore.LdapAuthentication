using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Justin.AspNetCore.LdapAuthentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for configuring the DI container
    /// </summary>`
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures <see cref="LdapAuthenticationOptions"/> for the given service collection.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="collection"></param>
        /// <param name="setupAction"></param>
        public static void AddLdapAuthentication<TUser>(this IServiceCollection collection, Action<LdapAuthenticationOptions> setupAction = null)
            where TUser: class
        {
            if (setupAction != null)
            {
                collection.Configure(setupAction);
            }
        }
    }
}
