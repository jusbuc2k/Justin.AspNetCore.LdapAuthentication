# Justin.AspNetCore.LdapAuthentication

Provides LDAP password authentication for an existing ASP.NET Core Identity user store via an LDAP bind. I created this for a project I'm working on with a very basic need, so it is not an exchaustive provider by any means.

## License

MIT

## Features

- LDAP password authentication via a custom UserManager against any existing UserManager/UserStore combination.
- Does not (yet) provide an LDAP based UserStore implementation. **You must still create users in your identity store before they can authenticate**. In the
example MVC application, you must first register a user account with a username/password, and then the user can authenticate with LDAP. Obviously this is not ideal,
and in a real world application, you would implement your own IUserStore.
- Does not support password changes or resets.

## Possible Future Features

- Full implementation of IUserStore and applicable interfaces around LDAP.

## Dependencies

- NETStandard 1.6.0
- Novell.Directory.Ldap.NETStandard 2.3.6
- Microsoft.AspNetCore.Identity 1.0.1

## Getting Started

Setup ASP.NET Identity Core

Install the NuGet Package 

```
Install-Package -Pre Justin.AspNetCore.LdapAuthentication
```

Create LdapAuth settings in appsettings.json:

```json
  "LdapAuth": {
    "Hostname": "<<ldap server host name goes here>>",
    "Port": 389,
    "Domain": "<<optional domain name goes here>>"
  },
```

Configure options and the custom User Manager in Startup *before* Identity:

```csharp
// Use a configuration file section
services.Configure<Justin.AspNetCore.LdapAuthentication.LdapAuthenticationOptions>(this.Configuration.GetSection("LdapAuth"));

// OR you can use services.AddLdapAuthentication(setupAction => {...}) to configure the 
// options manually instead of loading the configuration from Configuration.
services.Configure<Justin.AspNetCore.LdapAuthentication.LdapAuthenticationOptions>(options => {
    options.Hostname = "host-name-here";
    options.Port = 389;
    options.Domain = "domain-here"
});

// Add the custom user manager.
services.AddIdentity<IdentityUser, IdentityRole>()
    .AddUserManager<Justin.AspNetCore.LdapAuthentication.LdapUserManager<IdentityUser>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

Use the normal sign-in method, and it will valid the user's passwod via an LDAP bind.

```csharp
...
result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
...
```

## Other Notes

By default, the result of the user store GetNormalizedUserNameAsync() method on the UserStore as the value for the distguished name when performing an LDAP bind. You can customize this by implementing a custom user store and the interface Justin.AspNetCore.LdapAuthentication.IUserLdapStore, which provides a GetDistinguishedNameAsync method that will be used instead of the normalized username.

