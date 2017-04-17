using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamir.SharpSsh;

namespace FtpTest
{
	class FtpUltility
	{
		public static void FTPUpload(string filePath, string host, int port, string user, string password) {
			string args = filePath + "|" + host + "|" + port.ToString() + "|" + user + "|" + password + "|";
			if (args.IndexOf("||") >= 0) {
				throw new ArgumentException("Invalid parameter. No parameter can be empty.");
			}

			if (host.ToLower().StartsWith("sftp://")) {
				host = host.Substring(7);
			}

			// Check sub folder if any
			string subFolder = "";
			int index = host.IndexOf("/");
			if (index >= 0) {
				subFolder = host.Substring(index);
				host = host.Remove(index, subFolder.Length);
				subFolder = subFolder.Substring(1);
				if (subFolder.EndsWith("/")) {
					subFolder = subFolder.Remove(subFolder.Length - 1, 1);
				}
			}

			Sftp ftp = new Sftp(host, user, password);

			// we need a port
			if (port <= 0) {
				// try to get it from address
				index = host.IndexOf(":");
				if (index >= 0 && index < host.Length - 1) {
					port = int.Parse(host.Substring(index + 1));
					host = host.Substring(0, index);
				}

				// still not able to get it, set 21 as default
				if (port <= 0) {
					port = 21;
				}
			}

			ftp.Connect(port);

			string fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
			if (string.IsNullOrWhiteSpace(fileName)) {
				throw new ArgumentException("File name cannot be found in " + filePath, "filePath");
			}

			string destFilePath = (subFolder.Length > 0 ? subFolder + "/" : "") + fileName;
			try {
				ftp.Put(filePath, destFilePath);
			}
			catch (Exception ex) {
				throw ex;
			}
			finally {
				ftp.Close();
			}
		}
	}
}
