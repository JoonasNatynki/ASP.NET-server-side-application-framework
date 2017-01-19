using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace GameWebApi.Common.Security
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {
        private IBasicSecurityService _securityService;

        public BasicAuthenticationMessageHandler(IBasicSecurityService securityService)
        {
            _securityService = securityService;
        }

        private const int PlayerIdIndex = 0;
        private const int PasswordIndex = 1;
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!CanHandleAuthentication(request))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var isAuthenticated = Authenticate(request);

            if (isAuthenticated)
            {
                // Se menee tänne mutta sanoo silti että unauthorized eikä jatka item creationiin
                return await base.SendAsync(request, cancellationToken);
            }

            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            return response;
        }

        private bool CanHandleAuthentication(HttpRequestMessage request)
        {
            if (request.Headers == null)
            {
                return false;
            }

            if (request.Headers.Authorization == null)
            {
                return false;
            }

            return request.Headers.Authorization.Scheme.ToLowerInvariant() == "basic";
        }

        private bool Authenticate(HttpRequestMessage request)
        {
            var credentialParts = GetCredentialParts(request.Headers.Authorization);

            return _securityService.SetPrincipal(credentialParts[PlayerIdIndex], credentialParts[PasswordIndex]);
        }

        private string[] GetCredentialParts(AuthenticationHeaderValue authHeader)
        {
            string encodedCredentials = authHeader.Parameter;
            byte[] credentialBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.ASCII.GetString(credentialBytes);
            var credentialsParts = credentials.Split(':');
            return credentialsParts;
        }
    }
}
