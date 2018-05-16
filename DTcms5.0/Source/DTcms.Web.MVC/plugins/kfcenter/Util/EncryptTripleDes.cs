using System;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Util
{
	/// <summary>
	/// ʹ��TripleDes�㷨���м��ܽ��ܴ���.
	/// </summary>
	public class EncryptTripleDes
	{
        /// <summary>
        /// ���캯��
        /// </summary>
		public EncryptTripleDes()
		{
			
		}
		/// <summary>
		/// ʹ��ȱʡ��Կ�ַ�������
		/// </summary>
		/// <param name="original">����</param>
		/// <returns>����</returns>
		public  string Encrypt(string original)
		{
            return Encrypt(original, "Kinfar-laifujun");
		}
		/// <summary>
		/// ʹ��ȱʡ��Կ����
		/// </summary>
		/// <param name="original">����</param>
		/// <returns>����</returns>
		public  string Decrypt(string original)
		{
            return Decrypt(original, "Kinfar-laifujun", System.Text.Encoding.Default);
		}
		/// <summary>
		/// ʹ�ø�����Կ����
		/// </summary>
		/// <param name="original">����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public  string Decrypt(string original, string key)
		{
			return Decrypt(original,key,System.Text.Encoding.Default);
		}
		/// <summary>
		/// ʹ��ȱʡ��Կ����,����ָ�����뷽ʽ����
		/// </summary>
		/// <param name="original">����</param>
		/// <param name="encoding">���뷽ʽ</param>
		/// <returns>����</returns>
		public  string Decrypt(string original,Encoding encoding)
		{
            return Decrypt(original, "Kinfar-laifujun", encoding);
		}
		/// <summary>
		/// ʹ�ø�����Կ����
		/// </summary>
		/// <param name="original">ԭʼ����</param>
		/// <param name="key">��Կ</param>
		/// <param name="encoding">�ַ����뷽��</param>
		/// <returns>����</returns>
		public  string Encrypt(string original, string key)  
		{  
			byte[] buff = System.Text.Encoding.Default.GetBytes(original);  
			byte[] kb = System.Text.Encoding.Default.GetBytes(key);
			return Convert.ToBase64String(Encrypt(buff,kb));      
		}  
    
		/// <summary>
		/// ʹ�ø�����Կ����
		/// </summary>
		/// <param name="encrypted">����</param>
		/// <param name="key">��Կ</param>
		/// <param name="encoding">�ַ����뷽��</param>
		/// <returns>����</returns>
		public  string Decrypt(string encrypted, string key,Encoding encoding)  
		{       
			byte[] buff = Convert.FromBase64String(encrypted);  
			byte[] kb = System.Text.Encoding.Default.GetBytes(key);
			return encoding.GetString(Decrypt(buff,kb));      
		}  
		/// <summary>
		/// ����MD5ժҪ
		/// </summary>
		/// <param name="original">����Դ</param>
		/// <returns>ժҪ</returns>
		public  byte[] MakeMD5(byte[] original)
		{
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();   
			byte[] keyhash = hashmd5.ComputeHash(original);       
			hashmd5 = null;  
			return keyhash;
		}

		/// <summary>
		/// ʹ�ø�����Կ����
		/// </summary>
		/// <param name="original">����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public  byte[] Encrypt(byte[] original, byte[] key)  
		{  
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();       
			des.Key =  MakeMD5(key);
			des.Mode = CipherMode.ECB;  
     
			return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);     
		}  

		/// <summary>
		/// ʹ�ø�����Կ��������
		/// </summary>
		/// <param name="encrypted">����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public  byte[] Decrypt(byte[] encrypted, byte[] key)  
		{  
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();  
			des.Key =  MakeMD5(key);    
			des.Mode = CipherMode.ECB;  

			return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
		}  
  
		/// <summary>
		/// ʹ�ø�����Կ����
		/// </summary>
		/// <param name="original">ԭʼ����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public  byte[] Encrypt(byte[] original)  
		{
            byte[] key = System.Text.Encoding.Default.GetBytes("Kinfar-laifujun"); 
			return Encrypt(original,key);     
		}  

		/// <summary>
		/// ʹ��ȱʡ��Կ��������
		/// </summary>
		/// <param name="encrypted">����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public  byte[] Decrypt(byte[] encrypted)  
		{
            byte[] key = System.Text.Encoding.Default.GetBytes("Kinfar-laifujun"); 
			return Decrypt(encrypted,key);     
		}  
	}
}
