using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Justin.AspNetCore.LdapAuthentication
{
    /// <summary>
    /// Provides a custom user store that overrides password related methods to valid the user's password against LDAP.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class LdapUserManager<TUser> : Microsoft.AspNetCore.Identity.UserManager<TUser>
        where TUser: class
    {
        private readonly LdapAuthenticationOptions _ldapOptions;

        /// <summary>
        /// Initializes an instance.
        /// </summary>
        /// <param name="store"></param>
        /// <param name="optionsAccessor"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="userValidators"></param>
        /// <param name="passwordValidators"></param>
        /// <param name="keyNormalizer"></param>
        /// <param name="errors"></param>
        /// <param name="services"></param>
        /// <param name="logger"></param>
        /// <param name="ldapOptions"></param>
        public LdapUserManager(
            IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger, IOptions<LdapAuthenticationOptions> ldapOptions
        ) : base(
            store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger
        )
        {
            _ldapOptions = ldapOptions.Value;    
        }

        /// <summary>
        /// Checks the given password agains the configured LDAP server.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override async Task<bool> CheckPasswordAsync(TUser user, string password)
        {
            using (var auth = new LdapAuthentication(_ldapOptions))
            {
                string dn;

                // This gives a custom way to extract the DN from the user if it is different from the username.
                // It seems more like this would be a feature of the user store, but we can't get user store from userManager
                // and all the methods we really need for sign-in are on user manager.
                if (this.Store is IUserLdapStore<TUser>)
                {
                    dn = await((IUserLdapStore<TUser>)this.Store).GetDistinguishedNameAsync(user);
                }
                else
                {
                    dn = await this.Store.GetNormalizedUserNameAsync(user, CancellationToken.None);
                }

                if (string.IsNullOrEmpty(dn))
                {
                    return false;
                }

                if (auth.ValidatePassword(dn, password))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Throws a NotSupportedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public override Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a NotSupportedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override Task<IdentityResult> AddPasswordAsync(TUser user, string password)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Throws a NotSupportedException.
        /// </summary>
        /// <param name="store"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected override Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<TUser> store, TUser user, string password)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a NotSupportedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public override Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword)
        {
            throw new NotSupportedException();
        }

    }

}
