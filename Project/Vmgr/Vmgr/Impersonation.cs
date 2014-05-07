using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;

namespace Vmgr
{
    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle() : base(true) { }

        internal SafeTokenHandle(IntPtr handle)
            : base(true)
        {
            SetHandle(handle);
        }

        internal static SafeTokenHandle InvalidHandle
        {
            get { return new SafeTokenHandle(IntPtr.Zero); }
        }

        [DllImport("kernel32", SetLastError = true), SuppressUnmanagedCodeSecurity, ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private static extern bool CloseHandle(IntPtr handle);

        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }

    public static class Impersonation
    {

        private const string uacRegistryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
        private const string uacRegistryValue = "EnableLUA";

        private static uint STANDARD_RIGHTS_READ = 0x00020000;
        private static uint TOKEN_QUERY = 0x0008;
        private static uint TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);


        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);

        public enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        public enum TOKEN_ELEVATION_TYPE
        {
            TokenElevationTypeDefault = 1,
            TokenElevationTypeFull,
            TokenElevationTypeLimited
        }

        #region PRIVATE PROPERTIES

        private static bool _isUserTokenCached = false;

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public static bool IsUacEnabled
        {
            get
            {
                bool result = false;

                RegistryKey uacKey = Registry.LocalMachine.OpenSubKey(uacRegistryKey, false);

                try
                {
                    result = uacKey.GetValue(uacRegistryValue).Equals(1);
                }
                catch
                {
                    //  No registry key found.
                }

                return result;
            }
        }

        public static bool IsProcessElevated
        {
            get
            {
                if (IsUacEnabled)
                {
                    IntPtr tokenHandle;

                    if (!OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_READ, out tokenHandle))
                    {
                        throw new ApplicationException("Could not get process token.  Win32 Error Code: " + Marshal.GetLastWin32Error());
                    }
                    else
                    {
                        if (SafeTokenHandle == null)
                            SafeTokenHandle = new SafeTokenHandle(Process.GetCurrentProcess().Handle);
                    }

                    TOKEN_ELEVATION_TYPE elevationResult = TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault;

                    int elevationResultSize = Marshal.SizeOf((int)elevationResult);

                    uint returnedSize = 0;

                    IntPtr elevationTypePtr = Marshal.AllocHGlobal(elevationResultSize);

                    bool success = GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenElevationType, elevationTypePtr, (uint)elevationResultSize, out returnedSize);
                    if (success)
                    {
                        elevationResult = (TOKEN_ELEVATION_TYPE)Marshal.ReadInt32(elevationTypePtr);
                        bool isProcessAdmin = elevationResult == TOKEN_ELEVATION_TYPE.TokenElevationTypeFull;
                        return isProcessAdmin;
                    }
                    else
                    {
                        throw new ApplicationException("Unable to determine the current elevation.");
                    }
                }
                else
                {
                    WindowsIdentity identity = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    bool result = principal.IsInRole(WindowsBuiltInRole.Administrator);
                    return result;
                }
            }
        }

        public static SafeTokenHandle SafeTokenHandle = null;

        public static bool IsUserTokenCached
        {
            get
            {
                return SafeTokenHandle != null
                    && _isUserTokenCached == true;
            }
        }

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public static void ClearCache()
        {
            SafeTokenHandle = null;
            _isUserTokenCached = false;
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        /// <summary>
        /// Impersonation is performed using the process account.
        /// </summary>
        /// <param name="d"></param>
        public static void Impersonate(ProcessImpersonationDelegate d)
        {
            using (WindowsImpersonationContext impersonatedUser = WindowsIdentity.Impersonate(IntPtr.Zero))
            {
                d.DynamicInvoke();
            }
        }

        public static void Impersonate(string userName, string password, string domain, bool cacheUserToken, ProcessImpersonationDelegate d)
        {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_INTERACTIVE = 2;

            bool returnValue = IsUserTokenCached;

            if (!IsUserTokenCached)
                returnValue = LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out SafeTokenHandle);

            if (!returnValue)
                throw new SecurityException("Credentials invalid");

            if (!cacheUserToken)
            {
                _isUserTokenCached = false;

                using (SafeTokenHandle)
                {
                    WindowsIdentity newId = new WindowsIdentity(SafeTokenHandle.DangerousGetHandle());
                    using (WindowsImpersonationContext impersonatedUser = newId.Impersonate())
                    {
                        d.DynamicInvoke();
                    }
                }
            }
            else
            {
                _isUserTokenCached = true;

                WindowsIdentity newId = new WindowsIdentity(SafeTokenHandle.DangerousGetHandle());
                using (WindowsImpersonationContext impersonatedUser = newId.Impersonate())
                {
                    d.DynamicInvoke();
                }
            }
        }

        #endregion

        #region EVENTS

        #endregion

    }

    public delegate void ProcessImpersonationDelegate();
}
