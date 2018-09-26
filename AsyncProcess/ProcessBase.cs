using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProcess
{
    public abstract class ProcessBase
    {
        string _binaryPath;
        string[] _exeParams;
        protected Process localProcess;
        public ProcessBase(string binaryPath, params string[] @params)
        {
            _binaryPath = binaryPath;
            _exeParams = @params;
            localProcess = new Process();
            localProcess.EnableRaisingEvents = true;
            localProcess.StartInfo.UseShellExecute = false;
            localProcess.Exited += ProcessExited;
            localProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            localProcess.StartInfo.CreateNoWindow = true;
            localProcess.StartInfo.FileName = _binaryPath;
            localProcess.StartInfo.Arguments = $"{_binaryPath} {String.Join(" ", _exeParams)}";

        }

        protected virtual void ProcessExited(object sender, EventArgs e)
        {
            localProcess.Dispose();
        }

        protected virtual void OutputHandler(object sender, DataReceivedEventArgs e){}
    }
}
