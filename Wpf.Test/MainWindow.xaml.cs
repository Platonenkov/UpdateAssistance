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
            Application.Current.MainWindow.Closing += new CancelEventHandler(this.MainWindow_Closing);
            updateService = App.Hosting.Services.GetRequiredService<IUpdateService>();
            updateService.StartUpdateTimer(TimeSpan.FromMinutes(1));
            var assembly = Assembly.GetAssembly(typeof(NotificationManager));
            updateService.SetupSettings(assembly, "Notification.Wpf", assembly.ParsePackageVersion(),"Platonenkov", "", "Notification.Wpf");
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

    }
}
