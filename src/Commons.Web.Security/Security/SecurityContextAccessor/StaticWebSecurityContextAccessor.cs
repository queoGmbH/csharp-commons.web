namespace Commons.Web.Security.SecurityContextAccessor
{
    /// <summary>
    /// Provides access to the static web security context.
    /// </summary>
    public class StaticWebSecurityContextAccessor : ISecurityContextAccessor<ISecurityContext>
    {
        private ISecurityContext _securityContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticWebSecurityContextAccessor"/> class.
        /// </summary>
        /// <param name="securityContextCreator">The security context creator.</param>
        public StaticWebSecurityContextAccessor(SecurityContextCreator securityContextCreator)
        {
            // We initialize it with an empty security context.
            _securityContext = securityContextCreator.CreateEmpty();
        }

        /// <summary>
        /// Gets the current security context.
        /// </summary>
        /// <returns>The current security context.</returns>
        public ISecurityContext GetCurrent()
        {
            return _securityContext;
        }

        /// <summary>
        /// Sets the current security context.
        /// </summary>
        /// <param name="emptySecurityContext">The empty security context to set.</param>
        public void SetCurrent(ISecurityContext emptySecurityContext)
        {
            _securityContext = emptySecurityContext;
        }
    }
}
