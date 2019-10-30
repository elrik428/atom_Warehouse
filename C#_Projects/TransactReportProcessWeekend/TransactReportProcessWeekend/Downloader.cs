using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace TransactReportImport
{
    class Downloader
    {
        private WebResponse response;

        private Stream stream;
        WebProxy proxy;
        private long size;
        private string authuser, authpasswd;
        private string proxyserver;
        private int proxyport;
        private const int downloadBlockSize = 1024;
        private string downloadingTo;
        private string url;

        public string AuthUser
        {
            get { return authuser; }
            set { authuser = value; }
        }

        public string AuthPasswd
        {
            get { return authpasswd; }
            set { authpasswd = value; }
        }

        public string DownloadingTo
        {
            get { return downloadingTo; }
            set { downloadingTo = value; }
        }

        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        static Downloader()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(certificateCallBack);
        }

        static bool certificateCallBack(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public Downloader(string ProxyServer, int ProxyPort)
        {
            proxyserver = ProxyServer;
            proxyport = ProxyPort;

            // TEST
            proxy = new WebProxy(proxyserver, proxyport);
            
            proxy.UseDefaultCredentials = true;
            // TEST
        }

        private long GetFileSize(string url)
        {
            WebResponse response = null;
            long size = -1;
            try
            {
                response = GetRequest().GetResponse();
                size = response.ContentLength;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return size;
        }

        private WebRequest GetRequest()
        {
            WebRequest request = WebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " +
                Convert.ToBase64String(new ASCIIEncoding().GetBytes(authuser + ":" + authpasswd))); // fWu%AQA9

            // ASSUME PROXY
            //request.Proxy = proxy;
            
            // ASSUME PROXY

            return request;
        }

        public void HttpStartDownload()
        {
            try
            {
                long urlSize = GetFileSize(url);
                size = urlSize;

                WebRequest req = GetRequest();
                response = (WebResponse)req.GetResponse();
            }
            catch (Exception e)
            {
                throw new ArgumentException(String.Format(
                    "Error downloading \"{0}\": {1}", url, e.Message), e);
            }
            try
            {
                ValidateResponse(response, url);
                string destFileName = Path.GetFileName(response.ResponseUri.ToString());

                // The place we're downloading to (not from) must not be a URI,
                // because Path and File don't handle them...
                destFileName = destFileName.Replace("file:///", "").Replace("file://", "");
                this.downloadingTo = Path.Combine(downloadingTo, destFileName);

                // Create the file on disk here, so even if we don't receive any data of the file
                // it's still on disk. This allows us to download 0-byte files.
                if (!File.Exists(downloadingTo))
                {
                    FileStream fs = File.Create(downloadingTo);
                    fs.Close();
                }

                // create the download buffer
                byte[] buffer = new byte[downloadBlockSize];

                int readCount;

                // update how many bytes have already been read
                long totalDownloaded = size;
                stream = response.GetResponseStream();

                while ((int)(readCount = stream.Read(buffer, 0, downloadBlockSize)) > 0)
                {
                    // break on cancel

                    // update total bytes read
                    totalDownloaded += readCount;

                    // save block to end of file
                    SaveToFile(buffer, readCount, this.downloadingTo);
                }
            }
            catch (UriFormatException e)
            {
                throw new ArgumentException(
                    String.Format("Could not parse the URL \"{0}\" - it's either malformed or is an unknown protocol.", url), e);
            }
        }

        private void SaveToFile(byte[] buffer, int count, string fileName)
        {
            FileStream f = null;
            
            try
            {
                f = File.Open(fileName, FileMode.Append, FileAccess.Write);
                f.Write(buffer, 0, count);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(
                    String.Format("Error trying to save file \"{0}\": {1}", fileName, e.Message), e);
            }

            if (f != null)
                f.Close();
        }

        private static void ValidateResponse(WebResponse response, string url)
        {
            if (response is HttpWebResponse)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                // If it's an HTML page, it's probably an error page. Comment this
                // out to enable downloading of HTML pages.
                if (httpResponse.ContentType.Contains("text/html") || httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ArgumentException(
                        String.Format("Could not download \"{0}\" - a web page was returned from the web server.",
                        url));
                }
            }
            else if (response is FtpWebResponse)
            {
                FtpWebResponse ftpResponse = (FtpWebResponse)response;
                if (ftpResponse.StatusCode == FtpStatusCode.ConnectionClosed)
                    throw new ArgumentException(
                        String.Format("Could not download \"{0}\" - FTP server closed the connection.", url));
            }
            // FileWebResponse doesn't have a status code to check.
        }

        public void ftpDownload(string ftpAddress, string fileToGet, string BatchFilePath, string ftpOutputDirectory, string username, string password)
        {
            string stringToWrite = "get " + fileToGet + " " + ftpOutputDirectory + fileToGet;

            FileStream fs = new FileStream(BatchFilePath, FileMode.Create, FileAccess.Write);
            fs.Write(Encoding.ASCII.GetBytes(stringToWrite), 0, stringToWrite.Length);
            fs.Close();

            Process p = Process.Start(@"psftp", ftpAddress + @" -l "+username + " -pw " + password + " -b " + BatchFilePath);
            while (!p.HasExited) ;
            p.Close();
        }

        public void ftpDownload(string ftpAddress, string fileToGet, string BatchFilePath, string ftpOutputDirectory)
        {
            string stringToWrite = "get " + fileToGet + " " + ftpOutputDirectory + fileToGet;

            FileStream fs = new FileStream(BatchFilePath, FileMode.Create, FileAccess.Write);
            fs.Write(Encoding.ASCII.GetBytes(stringToWrite), 0, stringToWrite.Length);
            fs.Close();

            //1Z82R59W0441619485

            //Process p = Process.Start("AlphaCardlinkPSFTP.bat");
            //while (!p.HasExited) ;

            //p.Close();
        }

        public void DeleteFiles(string ftpOutputDirectory, string ftpFile, string HttpOutputDirectory, string httpFile)
        {
            if(ftpFile != "")
                File.Delete(ftpOutputDirectory + ftpFile);

            if(httpFile != "")
                File.Delete(HttpOutputDirectory + httpFile);
        }
    }

    public class ErrorMessage
    {
        private string errorsource;
        private string description;
        private static int errorcount;

        public ErrorMessage()
        {
            errorsource = "";
            description = "";
        }

        public string ErrorSource
        {
            set { errorsource = value; }
            get { return errorsource; }
        }

        public string Description
        {
            set { description = value; }
            get { return description; }
        }

        public static int ErrorCount
        {
            set { errorcount = value; }
            get { return errorcount; }
        }
    }
}
 

