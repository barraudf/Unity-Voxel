using System;
using System.Threading;

public interface IThreadPool : IDisposable
{
	void QueueUserWorkItem(WaitCallback work, object obj);
	void QueueUserWorkItem(WaitCallback work);
}
