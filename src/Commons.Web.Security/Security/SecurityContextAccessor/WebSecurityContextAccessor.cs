using System;
using System.Security;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace Commons.Web.Security.SecurityContextAccessor
{
    /// <summary>
    /// Accessor for the current <see cref="ISecurityContext"/> in a web application.
    /// Can be injected in every component when you need to access the security context.
    /// </summary>
    public class WebSecurityContextAccessor : ISecurityContextAccessor<ISecurityContext>
    {
        private readonly ISecurityContextHolder securityContextHolder;
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSecurityContextAccessor"/> class.
        /// </summary>
        /// <param name="securityContextHolder">The security context holder.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public WebSecurityContextAccessor(ISecurityContextHolder securityContextHolder, IHttpContextAccessor httpContextAccessor)
        {
            this.securityContextHolder = securityContextHolder;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the current security context.
        /// </summary>
        /// <returns>The current security context.</returns>
        public ISecurityContext GetCurrent()
        {
            HttpContext? httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new SecurityException("There is no HttpContext available, when trying to get the security context.");
            }
            ClaimsPrincipal? principal = httpContext.User;
            if (principal == null)
            {
                throw new SecurityException("There is no HttpContext available, when trying to get the security context.");
            }
            ISecurityContext securityContext = securityContextHolder.GetSecurityContext(principal);
            return securityContext;
        }

        /// <summary>
        /// Sets the current security context.
        /// </summary>
        /// <param name="emptySecurityContext">The empty security context.</param>
        public void SetCurrent(ISecurityContext emptySecurityContext)
        {
            throw new NotImplementedException();
        }
    }
}
