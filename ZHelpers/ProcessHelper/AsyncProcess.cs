using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZHelpers.ProcessHelper
{
    public class AsyncProcess:ProcessBase
    {
        string _stringResult;
        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        CancellationToken cancelToken;
        public AsyncProcess(string binaryPath, CancellationToken cancelationToken, params string[] @params):base(binaryPath, @params)
        {
           
            cancelToken = cancelationToken;
            cancelationToken.Register(() => {
                ProcessExited(null, null);
            });
        }

        public async Task<string> GetStringAsync(bool getStandardOutput = true, bool getStandardError = true)
        {
            localProcess.StartInfo.RedirectStandardError = getStandardError;
            localProcess.StartInfo.RedirectStandardOutput = getStandardOutput;
            if (getStandardError) localProcess.ErrorDataReceived += OutputHandler;
            if (getStandardOutput) localProcess.OutputDataReceived += OutputHandler;
            localProcess.Start();
            localProcess.BeginErrorReadLine();
            return await tcs.Task;
        }
        protected override void ProcessExited(object sender, EventArgs e)
        {
            if (cancelToken.IsCancellationRequested)
            {
                tcs.SetException(new OperationCanceledException("Job Canceled"));
            }
            else
            {
                tcs.SetResult(_stringResult);
            }
            localProcess.Dispose();
        }

        protected override void OutputHandler(object sender, DataReceivedEventArgs e)
        {
            _stringResult += e.Data;
            base.OutputHandler(sender, e);
        }

    }
}
