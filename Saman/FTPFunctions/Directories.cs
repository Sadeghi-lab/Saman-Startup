using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Saman.FTPFunctions
{
    class model
    {
        public bool result;
        public string message;
    }
    internal class Directories
    {
        private const string uri = "ftp://51.77.179.54/download";
        private const string user = "apache";
        private const string pass = "Saman2051678";
        public List<string> GetDirectories()
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(uri);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            List<string> directories = new List<string>();

            string line = streamReader.ReadLine();
            if (line.Contains(".")) line = streamReader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                if (line.Contains(".") || string.IsNullOrEmpty(line))
                {
                    line = streamReader.ReadLine();
                    continue;
                }
                directories.Add(line);
                line = streamReader.ReadLine();
            }

            streamReader.Close();
            return directories;
        }

        public List<string> GetFilesName(string category)
        {
            string url = uri + "/" + category;
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(url);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            List<string> directories = new List<string>();

            string line = streamReader.ReadLine();
            if (line == "." || line == "..") line = streamReader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                if (line == "." || line == ".." || string.IsNullOrEmpty(line))
                {
                    line = streamReader.ReadLine();
                    continue;
                }
                directories.Add(line);
                line = streamReader.ReadLine();
            }

            streamReader.Close();
            return directories;
        }

        public model Download(string category, string filename, string path)
        {
            try
            {
                string url = uri + "/" + category.Trim() + "/" + filename;
                string inputfilepath = path;

                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential(user, pass);
                    byte[] fileData = request.DownloadData(url);

                    using (FileStream file = File.Create(inputfilepath))
                    {
                        file.Write(fileData, 0, fileData.Length);
                        file.Close();
                    }
                    return new model() { message = "", result = true };
                }
            }
            catch (Exception ex)
            {
                return new model() { result = false, message = ex.Message };

            }

        }




    }
}