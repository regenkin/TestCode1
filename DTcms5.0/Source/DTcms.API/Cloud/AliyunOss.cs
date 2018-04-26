using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DTcms.API.Cloud
{
    public class AliyunOss
    {
        string EndPoint = string.Empty;
        string AccessKeyId = string.Empty;
        string AccessKeySecret = string.Empty;
        Aliyun.OSS.OssClient client;

        public AliyunOss(string endpoint, string accessKeyId, string accessKeySecret)
        {
            EndPoint = endpoint;
            AccessKeyId = accessKeyId;
            AccessKeySecret = accessKeySecret;
            client = new Aliyun.OSS.OssClient("https://" + EndPoint, AccessKeyId, AccessKeySecret);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="byteData">文件流数组</param>
        /// <param name="bucketName">存储空间名称</param>
        /// <param name="key">文件名</param>
        /// <param name="customDmain">自定义域名</param>
        /// <param name="result">成功则返回文件地址否则返回错误信息</param>
        /// <returns>是否上传成功</returns>
        public bool PutObject(byte[] byteData, string bucketName, string key, string customDmain, out string result)
        {
            using (Stream fileStream = new MemoryStream(byteData))//转成Stream流  
            {
                string md5 = Aliyun.OSS.Util.OssUtils.ComputeContentMd5(fileStream, byteData.Length);
                try
                {
                    //将文件md5值赋值给meat头信息，服务器验证文件MD5  
                    var objectMeta = new Aliyun.OSS.ObjectMetadata
                    {
                        ContentMd5 = md5,
                    };
                    //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5)
                    client.PutObject(bucketName, key.TrimStart('/'), fileStream, objectMeta);
                    if (string.IsNullOrEmpty(customDmain))
                    {
                        result = "http://" + bucketName + "." + EndPoint + "/" + key; //默认网址
                    }
                    else
                    {
                        result = customDmain.TrimEnd('/') + "/" + key; //自定义网址
                    }
                    return true;
                }
                catch (Exception e)
                {
                    result = e.Message;
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="bucketName">存储空间名称</param>
        /// <param name="filePath">文件地址(含http://)</param>
        /// <param name="customDmain">自定义域名</param>
        /// <param name="result">错误信息</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteObject(string bucketName, string filePath, string customDomain, out string result)
        {
            string pointDmain = "http://" + bucketName + "." + EndPoint + "/"; //默认网址
            if (!string.IsNullOrEmpty(customDomain))
            {
                pointDmain = customDomain.TrimEnd('/') + "/"; //自定义网址
            }
            if (!filePath.StartsWith(pointDmain))
            {
                result = "该文件不属于OSS存储对象";
                return false;
            }
            string fileKey = filePath.Replace(pointDmain, ""); //去除网址
            try
            {
                client.DeleteObject(bucketName, fileKey);
                result = "文件删除成功";
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取所有文件列表
        /// </summary>
        /// <param name="bucketName">存储空间名称</param>
        /// <param name="fileTypes">文件扩展名，如.jpg,.gif</param>
        /// <param name="result">结果信息</param>
        /// <returns>LIST</returns>
        public List<string> ListObjects(string bucketName, string fileTypes, string customDmain, out string result)
        {
            try
            {
                List<string> fileList = new List<string>();
                Aliyun.OSS.ObjectListing listResult = null;
                string nextMarker = string.Empty;
                do
                {
                    var listObjectsRequest = new Aliyun.OSS.ListObjectsRequest(bucketName)
                    {
                        Marker = nextMarker,
                        MaxKeys = 100
                    };
                    listResult = client.ListObjects(listObjectsRequest);

                    foreach (var summary in listResult.ObjectSummaries)
                    {
                        if (fileTypes.Contains(Path.GetExtension(summary.Key).ToLower()))
                        {
                            if (!string.IsNullOrEmpty(customDmain))
                            {
                                fileList.Add(customDmain.TrimEnd('/') + "/" + summary.Key); //自定义网址
                            }
                            else
                            {
                                fileList.Add("http://" + bucketName + "." + EndPoint + "/" + summary.Key); //默认网址
                            }
                        }
                    }
                    nextMarker = listResult.NextMarker;
                } while (listResult.IsTruncated);

                result = "文件获取成功";
                return fileList;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return null;
            }

        }

    }
}
