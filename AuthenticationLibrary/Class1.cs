using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace AuthenticationLibrary
{
    public class AzureADAuthenticator
    {
        private static string clientId;
        private static string clientSecret;
        private static string authority = "https://login.microsoftonline.com/common";

        public static string GetAuthorizationUrl(string scope, string redirectUri, string clientId, string clientSecret)
        {
            AzureADAuthenticator.clientId = clientId;
            AzureADAuthenticator.clientSecret = clientSecret;

            var authContext = new AuthenticationContext(authority);
            var authUri = authContext.GetAuthorizationRequestUrlAsync(
                scope,
                clientId,
                new Uri(redirectUri),
                UserIdentifier.AnyUser,
                null
            ).Result;

            return authUri.ToString();
        }

        public static string ProcessAuthorizationCode(string code, string scope, string redirectUri, string clientId, string clientSecret)
        {
            AzureADAuthenticator.clientId = clientId;
            AzureADAuthenticator.clientSecret = clientSecret;

            var authContext = new AuthenticationContext(authority);
            var result = authContext.AcquireTokenByAuthorizationCodeAsync(
                code,
                new Uri(redirectUri),
                new ClientCredential(clientId, clientSecret),
                scope
            ).Result;
            Console.WriteLine(result);
            return result.AccessToken;
        }
    }
}

