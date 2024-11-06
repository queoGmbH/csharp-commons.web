using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Principal;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Xml;

namespace Queo.Commons.Web.Security
{
    /// <summary>
    /// Holds a list of <see cref="ISecurityContext"/> 
    /// </summary>
    public class SecurityContextHolder : ISecurityContextHolder, ISecurityContextInvalidator
    {
        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new();
        private Dictionary<string, ISecurityContext> _securityContextes = [];
        private readonly Guid _identifier = Guid.NewGuid();

        /// <summary>
        /// Gets the unique identifier of the security context holder.
        /// </summary>
        public Guid Identifier { get { return _identifier; } }

        /// <summary>
        /// Gets the security context for the specified principal.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The security context.</returns>
        /// <exception cref="SecurityException">Thrown when the principal's identity is null or empty.</exception>
        public ISecurityContext GetSecurityContext(IPrincipal principal)
        {
            CheckPrincipalIdentityAndName(principal);
            _readerWriterLockSlim.EnterReadLock();
            try
            {
                _securityContextes.TryGetValue(principal.Identity!.Name!, out ISecurityContext? securityContext);
                if (securityContext == null)
                {
                    throw new SecurityException($"There is no security context for the principal {principal.Identity.Name}.");
                }
                return securityContext;
            }
            finally
            {
                _readerWriterLockSlim.ExitReadLock();
            }
        }

        private static void CheckPrincipalIdentityAndName(IPrincipal principal)
        {
            if (principal.Identity == null || string.IsNullOrEmpty(principal.Identity.Name))
            {
                throw new SecurityException("The principal must provide an identity and a unique name.");
            }
        }

        /// <summary>
        /// Checks if the security context holder has a security context for the specified principal.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>True if the security context holder has a security context for the principal; otherwise, false.</returns>
        /// <exception cref="SecurityException">Thrown when the principal's identity is null or empty.</exception>
        public bool Has(IPrincipal principal)
        {
            CheckPrincipalIdentityAndName(principal);

            _readerWriterLockSlim.EnterReadLock();
            try
            {
                return _securityContextes.ContainsKey(principal.Identity!.Name!);
            }
            finally
            {
                _readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Adds a security context to the cache or updates an existing security context.
        /// </summary>
        /// <param name="securityContext">The security context to add or update.</param>
        public void Add(ISecurityContext securityContext)
        {
            _readerWriterLockSlim.EnterWriteLock();
            try
            {
                _securityContextes[securityContext.IdentityName] = securityContext;
            }
            finally
            {
                _readerWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Invalidates the security context for the specified user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        public void Invalidate(string userName)
        {
            _readerWriterLockSlim.EnterWriteLock();
            try
            {
                _securityContextes.Remove(userName);
            }
            finally
            {
                _readerWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Invalidates all security contexts in the cache.
        /// </summary>
        public void Invalidate()
        {
            _readerWriterLockSlim.EnterWriteLock();
            try
            {
                _securityContextes.Clear();
            }
            finally
            {
                _readerWriterLockSlim.ExitWriteLock();
            }
        }
    }
}
