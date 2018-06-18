using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Uintra.Installer.Infrastructure
{
    public class MachineInfo
    {
        public string CpuId { get; private set; }
        public string MacAddress { get; private set; }
        public string MbSerial { get; private set; }
        public string HddSerial { get; private set; }
        public string Ip { get; private set; }

        public MachineInfo()
        {
            CpuId = GetCpuId();
            MacAddress = GetMACAddress();
            MbSerial = GetMotherBoardSerialNumber();
            HddSerial = GetHddSerialNumber();
            Ip = GetIpAddress();
        }

        private string GetCpuId()
        {
            try
            {
                string processorId = string.Empty;
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    processorId = mo.Properties["processorID"].Value.ToString();
                    break;
                }
                moc.Dispose();
                return processorId;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetMACAddress()
        {
            try
            {
                string MACAddress = String.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if (MACAddress == String.Empty)
                    { // only return MAC Address from first card
                        if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                    }
                    //mo.Dispose();
                }
                moc.Dispose();
                return MACAddress;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetMotherBoardSerialNumber()
        {
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                ManagementObjectCollection moc = mos.Get();
                string motherBoard = "";
                foreach (ManagementObject mo in moc)
                {
                    //motherBoard = (string)mo["SerialNumber"];
                    //motherBoard = (string)mo.GetPropertyValue("SerialNumber");
                    motherBoard = mo.Properties["SerialNumber"].Value.ToString();
                }
                moc.Dispose();
                return motherBoard;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetHddSerialNumber()
        {
            try
            {
                string drive = "C";
                ManagementObject dsk = new ManagementObject(
                    @"win32_logicaldisk.deviceid=""" + drive + @":""");
                dsk.Get();
                string volumeSerial = dsk["VolumeSerialNumber"].ToString();
                dsk.Dispose();
                return volumeSerial;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetIpAddress()
        {
            try
            {
                return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
