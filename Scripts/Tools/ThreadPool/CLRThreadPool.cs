using System;
using System.Threading;

public class CLRThreadPool : IThreadPool
{
	public void QueueUserWorkItem(WaitCallback work, object obj)
	{
		ThreadPool.QueueUserWorkItem(work, obj);
	}

	public void QueueUserWorkItem(WaitCallback work)
	{
		ThreadPool.QueueUserWorkItem(work, null);
	}

	public void Dispose() { }
}
