using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using Queo.Commons.Web.Security.SecurityContextAccessor;

namespace Queo.Commons.Web.Security
{
    /// <summary>
    /// Middleware for handling security context in the application.
    /// </summary>
    public class SecurityContextMiddleware
    {
        private RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityContextMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        public SecurityContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="securityContextHolder">The security context holder.</param>
        /// <param name="contextCreator">The security context creator.</param>
        /// <param name="securityContextAccessor">The security context accessor.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context, ISecurityContextHolder securityContextHolder, SecurityContextCreator contextCreator, ISecurityContextAccessor<ISecurityContext> securityContextAccessor)
        {
            ISecurityContext securityContext;

            ClaimsPrincipal user = context.User;

            if (user.Identity == null || (user.Identity.IsAuthenticated == false && string.IsNullOrEmpty(user.Identity.Name)))
            {
                // there is no authenticated user, so we return an empty context
                securityContext = contextCreator.CreateEmpty();
                securityContextAccessor.SetCurrent(securityContext);
            }
            else if (!securityContextHolder.Has(user))
            {
                securityContext = contextCreator.Create(user);
                securityContextHolder.Add(securityContext);
                securityContextAccessor.SetCurrent(securityContext);
            }
            else
            {
                securityContext = securityContextHolder.GetSecurityContext(user);
                securityContextAccessor.SetCurrent(securityContext);
            }
            AddClaims(user, securityContext);
            await _next(context);
        }

        /// <summary>
        /// Adds all roles and permissions from the security context to the claims of the principal.
        /// </summary>
        /// <param name="user">The claims principal.</param>
        /// <param name="securityContext">The security context.</param>
        private void AddClaims(ClaimsPrincipal user, ISecurityContext securityContext)
        {
            ClaimsIdentity? identity = user.Identity as ClaimsIdentity;
            if (identity != null)
            {
                foreach (string permission in securityContext.Permissions)
                {
                    identity.AddClaim(new Claim("roles", permission));
                }
            }
        }
    }
}
