// The MIT License (MIT)
// 
// Copyright (c) 2015 Daniel Franklin. http://blazedsp.com/
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Frontend
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Blaze.DSP.Library.Constants;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Azure;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Facebook;
    using Microsoft.Owin.Security.Google;

    using Models;

    using Owin;

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public static void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/account/login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(TimeSpan.FromMinutes(30), (manager, user) => user.GenerateUserIdentityAsync(manager)),

                    // TODO: Cookies suck, use bearer tokens!
                    OnApplyRedirect = ctx =>
                    {
                        if (!IsAjaxRequest(ctx.Request) && !IsJsonRequest(ctx.Request))
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                    }
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Microsoft Auth
            var mskey = CloudConfigurationManager.GetSetting(SettingsFrontend.MicrosoftAuthenticationKey);
            var mssec = CloudConfigurationManager.GetSetting(SettingsFrontend.MicrosoftAuthenticationSecret);
            if (!string.IsNullOrWhiteSpace(mskey) && !string.IsNullOrWhiteSpace(mssec))
            {
                app.UseMicrosoftAccountAuthentication(mskey, mssec);
            }

            // Twitter Auth
            var twkey = CloudConfigurationManager.GetSetting(SettingsFrontend.TwitterAuthenticationKey);
            var twsec = CloudConfigurationManager.GetSetting(SettingsFrontend.TwitterAuthenticationSecret);
            if (!string.IsNullOrWhiteSpace(twkey) && !string.IsNullOrWhiteSpace(twsec))
            {
                app.UseTwitterAuthentication(twkey, twsec);
            }

            // Facebook Auth
            var fbkey = CloudConfigurationManager.GetSetting(SettingsFrontend.FacebookAuthenticationKey);
            var fbsec = CloudConfigurationManager.GetSetting(SettingsFrontend.FacebookAuthenticationSecret);
            if (!string.IsNullOrWhiteSpace(fbkey) && !string.IsNullOrWhiteSpace(fbsec))
            {
                // NOTE: This is the default implementation
                //app.UseFacebookAuthentication(fbkey, fbsec);

                // NOTE: This is the extended implementation
                var facebookOptions = new FacebookAuthenticationOptions
                {
                    AppId = fbkey,
                    AppSecret = fbsec,
                    Provider = new FacebookAuthenticationProvider
                    {
                        OnAuthenticated = context =>
                        {
                            context.Identity.AddClaim(new Claim("FacebookAccessToken", context.AccessToken));
                            return Task.FromResult(0);
                        }
                    },
                    SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
                    SendAppSecretProof = true
                };
                facebookOptions.Scope.Add("This is for requested permissions");
                app.UseFacebookAuthentication(facebookOptions);
            }

            // Google+ Auth
            var gokey = CloudConfigurationManager.GetSetting(SettingsFrontend.GoogleAuthenticationKey);
            var gosec = CloudConfigurationManager.GetSetting(SettingsFrontend.GoogleAuthenticationSecret);
            if (!string.IsNullOrWhiteSpace(gokey) && !string.IsNullOrWhiteSpace(gosec))
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
                {
                    ClientId = gokey,
                    ClientSecret = gosec
                });
            }
        }

        private static bool IsAjaxRequest(IOwinRequest request)
        {
            var query = request.Query;

            if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            {
                return true;
            }

            var headers = request.Headers;

            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        private static bool IsJsonRequest(IOwinRequest request)
        {
            var headers = request.Headers;

            return ((headers != null) && (headers["Content-Type"] == "application/json"));
        }
    }
}