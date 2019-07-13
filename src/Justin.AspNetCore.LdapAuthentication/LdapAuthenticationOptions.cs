namespace Justin.AspNetCore.LdapAuthentication
{
    /// <summary>
    /// Represents options that configure LDAP authentication.
    /// </summary>
    public class LdapAuthenticationOptions
    {
        /// <summary>
        /// Gets or sets the LDAP server host name.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the TCP port on which the LDAP server is running. 
        /// </summary>
        public int Port { get; set; } = 389;

        /// <summary>
        /// Gets or sets the domain name to use as distinguished name in conjuction with the username
        /// </summary>
        public string Domain { get; set; }
    }
}
