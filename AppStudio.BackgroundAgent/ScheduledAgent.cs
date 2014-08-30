using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

using Microsoft.Phone.Shell;
using Microsoft.Phone.Scheduler;

namespace AppStudio.BackgroundAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        const string PERIODIC_TASKNAME = "Frisle";

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override async void OnInvoke(ScheduledTask task)
        {
            if (task.Name.Equals(PERIODIC_TASKNAME, StringComparison.OrdinalIgnoreCase))
            {
                await UpdateCycleTile();
            }
            else
            {
                // If your application uses another ScheduledTask, handle it here
            }

#if DEBUG
            ScheduledActionService.LaunchForTest(PERIODIC_TASKNAME, TimeSpan.FromSeconds(60));
#endif

            NotifyComplete();
        }

        private async Task UpdateCycleTile()
        {
            AppLogs.WriteInfo("UpdateCycleTile", "Executed");
            try
            {
                var imageUrls = await CycleTileAgent.GetTileImages();
                if (imageUrls != null)
                {
                    AppLogs.WriteInfo("UpdateCycleTile", String.Join("\r\n", imageUrls));

                    var cycleTileData = new CycleTileData()
                    {
                        CycleImages = imageUrls
                    };

                    var appTile = ShellTile.ActiveTiles.FirstOrDefault();
                    if (appTile != null)
                    {
                        appTile.Update(cycleTileData);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogs.WriteInfo("UpdateCycleTile", ex.ToString());
            }
            AppLogs.WriteInfo("UpdateCycleTile", "Finished");
        }
    }
}
