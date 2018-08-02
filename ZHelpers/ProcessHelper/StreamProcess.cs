using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ZHelpers.ProcessHelper
{
    public class StreamProcess:ProcessBase
    {
        MemoryStream _streamResult;
        public event EventHandler ProcessFinished;
        public StreamProcess(string binaryPath,params string[] @params) : base(binaryPath, @params)
        {
        }

        public Stream GetStream(bool getStandardOutput = true, bool getStandardError = true)
        {
            _streamResult = new MemoryStream();
            localProcess.StartInfo.RedirectStandardError = getStandardError;
            localProcess.StartInfo.RedirectStandardOutput = getStandardOutput;
            if (getStandardError) localProcess.ErrorDataReceived += OutputHandler;
            if (getStandardOutput) localProcess.OutputDataReceived += OutputHandler;
            localProcess.Start();
            localProcess.BeginErrorReadLine();
            return _streamResult;
        }

        protected override void ProcessExited(object sender, EventArgs e)
        {
            ProcessFinished?.Invoke(this, e);
        }
        protected override void OutputHandler(object sender, DataReceivedEventArgs e)
        {
            byte[] existingData = System.Text.Encoding.UTF8.GetBytes(e.Data);
            _streamResult.Write(existingData, 0, existingData.Length);
            base.OutputHandler(sender,e);
        }
    }
}
