using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Justin.AspNetCore.LdapAuthentication
{
    /// <summary>
    /// A class that provides password verification against an LDAP store by attempting to bind.
    /// </summary>
    public class LdapAuthentication :  IDisposable
    {
        private readonly LdapAuthenticationOptions _options;
        private readonly LdapConnection _connection;
        private bool _isDisposed = false;
        
        /// <summary>
        /// Initializes a new instance with the the given options.
        /// </summary>
        /// <param name="options"></param>
        public LdapAuthentication(LdapAuthenticationOptions options)
        {
            _options = options;
            _connection = new LdapConnection();
        }

        /// <summary>
        /// Cleans up any connections and other resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }            

            _connection.Dispose();
            _isDisposed = true;
        }

        /// <summary>
        /// Gets a value that indicates if the password for the user identified by the given DN is valid.
        /// </summary>
        /// <param name="distinguishedName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidatePassword(string distinguishedName, string password)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(LdapConnection));
            }

            if (string.IsNullOrEmpty(_options.Hostname))
            {
                throw new InvalidOperationException("The LDAP Hostname cannot be empty or null.");
            }

            _connection.Connect(_options.Hostname, _options.Port);

            try
            {
                _connection.Bind(distinguishedName, password);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _connection.Disconnect();
            }
        }
    }

}
