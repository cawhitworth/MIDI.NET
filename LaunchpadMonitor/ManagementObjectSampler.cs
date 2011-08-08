using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace LaunchpadMonitor
{
    class ManagementObjectSampler : ISampler
    {
        ManagementObject managementObject;
        string propertyName;

        public ManagementObjectSampler(ManagementObject managementObject, string propertyName)
        {
            this.managementObject = managementObject;
            this.propertyName = propertyName;
        }

        public int Sample()
        {
            managementObject.Get();
            return Convert.ToInt32(managementObject[propertyName]);
        }

    }
}
