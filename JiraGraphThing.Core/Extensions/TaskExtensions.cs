using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace JiraGraphThing.Core.Extensions
{
    public static class TaskExtensions
    {
        public static Task<T> AttachErrorHandler<T>(this Task<T> task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Debug.WriteLine(t.Exception.Message);
                    ExceptionDispatchInfo
                           .Capture(t.Exception)
                           .Throw();
                }

                return t.Result;
            });
        }
    }
}
