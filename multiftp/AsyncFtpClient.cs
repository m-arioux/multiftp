﻿using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace multiftp
{
    internal class AsyncFtpClient : IDisposable
    {
        private readonly SftpClient sftpClient;

        public AsyncFtpClient(ConnectionInfo connectionInfo)
        {
            sftpClient = new SftpClient(connectionInfo);
        }

        public void Connect()
        {
            sftpClient.Connect();
        }
        public string WorkingDirectory => sftpClient.WorkingDirectory;

        public Task<IEnumerable<SftpFile>> ListDirectoryAsync(string path) => 
            Task.Factory.FromAsync((callback, stateObject) => sftpClient.BeginListDirectory(path, callback, stateObject), sftpClient.EndListDirectory, null);

        public async Task<Stream> DownloadFile(string path) 
        {
            var stream = new MemoryStream();
            await Task.Factory.FromAsync((callback, stateObject) => sftpClient.BeginDownloadFile(path, stream, callback, stateObject), sftpClient.EndDownloadFile, null);

            return stream;
        }

        public void Disconnect()
        {
            sftpClient.Disconnect();
        }

        public void Dispose()
        {
            sftpClient.Dispose();
        }
    }
}
