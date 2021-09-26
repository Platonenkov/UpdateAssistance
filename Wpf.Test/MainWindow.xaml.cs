using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using AssemblyDataParser;
using Microsoft.Extensions.DependencyInjection;
using Notification.Wpf;
using UpdateAssistance;

namespace Wpf.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUpdateService updateService;
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.Closing += this.MainWindow_Closing;
            updateService = App.Hosting.Services.GetRequiredService<IUpdateService>();
            updateService.StartUpdateTimer(TimeSpan.FromMinutes(1));
            var assembly = Assembly.GetAssembly(typeof(App));
            updateService.SetupSettings(assembly,"Wpf.Test", assembly.ParsePackageVersion(),"Platonenkov",null, "UpdateAssistance");
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            updateService.CanShutdown.Subscribe(canShutDown =>
            {
                if (!canShutDown)
                {
                    // Cancel the shutdown and show the updating indicator
                    e.Cancel = true;

                    App.Notifier.Show("Update","Updating...",NotificationType.Notification);

                    this.Title = "Updating";

                    updateService.CanShutdown.Subscribe(this.CanShutdownChanged);
                }
            }).Dispose();
        }

        private void CanShutdownChanged(bool canShutdown)
        {
            if (canShutdown)
            {
                // safe to shutdown now.
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Application.Current.MainWindow.Close();
                });
            }
        }
        private void RestartCommand()
        {
            var restoreService = App.Hosting.Services.GetRequiredService<IRestoreService>();
            restoreService.SoftApplicationRestart();
        }

    }
}
