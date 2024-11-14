namespace Queo.Commons.Web.Security.SecurityContextAccessor
{
    /// <summary>
    /// Represents an accessor for the security context.
    /// </summary>
    /// <typeparam name="TSecurityContext">The type of the security context.</typeparam>
    public interface ISecurityContextAccessor<TSecurityContext> : ISecurityContextAccessor where TSecurityContext : ISecurityContext
    {

        /// <summary>
        /// Returns the <see cref="ISecurityContext"/> for the current request.
        /// </summary>
        /// <returns>The security context for the current request.</returns>
        new TSecurityContext GetCurrent();

        /// <summary>
        /// Sets the current security context.
        /// </summary>
        /// <param name="emptySecurityContext">The empty security context.</param>
        void SetCurrent(ISecurityContext emptySecurityContext);
    }

    /// <summary>
    /// Represents an accessor for the security context.
    /// </summary>
    public interface ISecurityContextAccessor
    {
        /// <summary>
        /// Returns the <see cref="ISecurityContext"/> for the current request.
        /// </summary>
        /// <returns>The security context for the current request.</returns>
        ISecurityContext GetCurrent();
    }
}
