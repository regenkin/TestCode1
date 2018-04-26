using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using Microsoft.Win32;

namespace KfCrm.Common
{
    class ECBC_CDKEY
    {
    }
    public class SoftReg
    {

        /// <summary> 
        /// ��ȡCPU���� 
        /// </summary> 
        /// <returns>�ַ�����cpu����</returns> 
        public string GetCPUName()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            object obj = rk.GetValue("ProcessorNameString");
            string CPUName = (string)obj;
            return CPUName.TrimStart();
        }
        public string GetCpuInfo()
        {
            //�õ�cpu��Ϣ 
            string _cpuInfo = "";//cpu��Ϣ 
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                _cpuInfo = mo.Properties["ProcessorId"].Value.ToString();

            }
            return _cpuInfo;
        }

        public string GetDiskID()
        {
            string _HDInfo = "";
            ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            foreach (ManagementObject mo in moc1)
            {
                _HDInfo = (string)mo.Properties["Model"].Value;


            }
            return _HDInfo;
        }
        public string GetMacAddress()
        {
            //��ȡ����Ӳ����ַ 

            string _MacAddress = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc2 = mc.GetInstances();
            foreach (ManagementObject mo in moc2)
            {
                if ((bool)mo["IPEnabled"] == true)
                    _MacAddress = mo["MacAddress"].ToString().Replace(":","");
                mo.Dispose();
            }
            return _MacAddress;
        } 




        /// <summary>
        /// ȡ���豸Ӳ�̵ľ���
        /// </summary>
        /// <returns></returns>
        public string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        /// <summary>        
        /// ���CPU�����к�        
        /// </summary>        
        /// <returns></returns>        
        public string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        /// <summary>        
        /// ���ɻ�����        
        /// </summary>        
        /// <returns></returns>    
        public string getMNum()
        {
            string strNum = GetDiskVolumeSerialNumber()+GetMacAddress();//���24λCpu��Ӳ�����к�            
            string strMNum = Common.DEncrypt.DEncrypt.Encrypt(strNum); ;//�����ɵ��ַ�����ȡ��ǰ24���ַ���Ϊ������  
            string MathineCode = "XMK" + strMNum.Substring(0, 2) + "-" + strMNum.Substring(1, 5) + "-" + strMNum.Substring(6, 5) + "-" + strMNum.Substring(12, 5) + "-" + strMNum.Substring(17, 5) + "-" + strMNum.Substring(22, 5) + "-" + strMNum.Substring(27, 5);
            return MathineCode.ToUpper();
        }
        
        /// <summary>        
        /// ����ע����        
        /// </summary>        
        /// <returns></returns>        
        public string getRNum()
        {
            string MNum = DEncrypt.DEncrypt.Encrypt(this.getMNum());//��ȡע����
            MNum = MNum.ToUpper();
            string RegKey = "KINFARRC-" + MNum.Substring(5, 5) + "-" + MNum.Substring(19, 5) + "-" + MNum.Substring(36, 5) + "-" + MNum.Substring(49, 5);

            return RegKey; 
        }

        public string getRRNum()
        {
            return getRNum().Length.ToString();
        }
    }
}
