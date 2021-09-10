using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MathCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notification.Wpf;
using UpdateAssistance;

namespace Wpf.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static NotificationManager Notifier { get; set; }
        private static IHost __Hosting;

        public static IHost Hosting => __Hosting
            ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
           .CreateDefaultBuilder(args)
           .AddServices(typeof(App))              // Добавляем все сервисы из сборки указанного типа
           .ConfigureServices(ConfigureServices)  // Добавляем дополнительные сервисы вручную в методе ниже
           .AddServiceLocator();                  // Подключаем класс ServiceLocator

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddScoped<IUpdateService, UpdateAssist>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Hosting;
            base.OnStartup(e);
            await host.StartAsync(); // При запуске приложения запускаем хост
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using var host = Hosting; // using - уничтожит хост после остановки приложения
            base.OnExit(e);
            await host.StopAsync();   // При остановке приложения - останавливаем хост
        }
        public App()
        {
            Notifier = new NotificationManager();
        }

    }
}
