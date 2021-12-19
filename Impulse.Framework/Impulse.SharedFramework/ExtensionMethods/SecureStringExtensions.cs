using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Impulse.SharedFramework.ExtensionMethods
{
    public static class SecureStringExtensions
    {
        public static string GetInsecureString(this SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);

            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }
    }
}
