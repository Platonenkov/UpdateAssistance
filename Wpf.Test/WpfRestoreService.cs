using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using UpdateAssistance;

namespace Wpf.Test
{
    public class WpfRestoreService : IRestoreService
    {
        /// <inheritdoc/>
        public bool ShouldRestoreState => Environment.GetCommandLineArgs().Contains(RecoveryManager.RestartCommandLine);

        /// <inheritdoc/>
        public void HardApplicationRestart()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Process.GetCurrentProcess().Kill();
        }

        /// <inheritdoc/>
        public void SoftApplicationRestart()
        {
            var updateService = App.Hosting.Services.GetRequiredService<IUpdateService>();

            if (updateService.IsInstalled)
            {
                // Do a reboot through Squirrel to allow for installing updates if required
                var assembly = Assembly.GetEntryAssembly();
                var updateDotExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Update.exe");

                var gmdcClientName = Path.GetFileName(Application.ResourceAssembly.Location);

                var psi = new ProcessStartInfo
                {
                    FileName = updateDotExe,
                    Arguments = $"-processStart {gmdcClientName} --process-start-args={RecoveryManager.RestartCommandLine}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                Process.Start(psi);
            }
            else
            {
                // If not installed just reboot normally
                Process.Start(Application.ResourceAssembly.Location, RecoveryManager.RestartCommandLine);
            }

            // Kill the current instance
            Application.Current.Shutdown();
        }
    }
}
