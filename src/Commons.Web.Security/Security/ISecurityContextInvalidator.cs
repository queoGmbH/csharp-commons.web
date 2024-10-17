namespace Queo.Commons.Web.Security
{
    /// <summary>
    /// Can be used to invalidate security contexts.
    /// </summary>
    public interface ISecurityContextInvalidator
    {
        /// <summary>
        /// Invalidates the security context for the user with the specified user name.
        /// </summary>
        /// <param name="userName"></param>
        void Invalidate(string userName);

        /// <summary>
        /// Invalidates all security contexts
        /// </summary>
        void Invalidate();
    }
}
