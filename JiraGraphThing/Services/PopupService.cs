using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JiraGraphThing.Services
{
    public static class PopupService
    {
        private static Task openDialogTask;

        public static async Task ShowPopup(string message)
        {
            await Window.Current.Dispatcher.RunTaskAsync(async ()=>
            {
                if (openDialogTask != null)
                {
                    await openDialogTask;
                }

                var cd = new ContentDialog();
                cd.PrimaryButtonText = "OK";
                cd.Title = message;

                try
                {
                    openDialogTask = cd.ShowAsync().AsTask();
                    await openDialogTask;
                }
                finally
                {
                    openDialogTask = null;
                }
            }, CoreDispatcherPriority.Normal);
        }


        /// <summary>
        /// https://github.com/Microsoft/Windows-task-snippets/blob/master/tasks/UI-thread-task-await-from-background-thread.md
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dispatcher"></param>
        /// <param name="func"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static async Task<T> RunTaskAsync<T>(this CoreDispatcher dispatcher, Func<Task<T>> func, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            await dispatcher.RunAsync(priority, async () =>
            {
                try
                {
                    taskCompletionSource.SetResult(await func());
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            });
            return await taskCompletionSource.Task;
        }

        //https://github.com/Microsoft/Windows-task-snippets/blob/master/tasks/UI-thread-task-await-from-background-thread.md
        // There is no TaskCompletionSource<void> so we use a bool that we throw away.
        public static async Task RunTaskAsync(this CoreDispatcher dispatcher,
            Func<Task> func, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal) =>
            await RunTaskAsync(dispatcher, async () => { await func(); return false; }, priority);
    }
}
