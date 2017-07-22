#if !WINDOWS_PHONE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mscoree;

namespace Flai.IO
{
    public static class AppDomainHelper
    {
        public static AppDomain[] GetAllAppDomains()
        {
            List<AppDomain> appDomains = new List<AppDomain>();
            IntPtr handle = IntPtr.Zero;
            CorRuntimeHost host = new CorRuntimeHost();
            try
            {
                host.EnumDomains(out handle);
                while (true)
                {
                    object domain;
                    host.NextDomain(handle, out domain);
                    if (domain == null)
                    {
                        break;
                    }

                    appDomains.Add((AppDomain)domain);
                }
            }
            finally
            {
                host.CloseEnum(handle);
            }

            return appDomains.ToArray();
        }
    }
}
#endif