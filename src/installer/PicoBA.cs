﻿using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using PicoTorrentBootstrapper.ViewModels;
using PicoTorrentBootstrapper.Views;

namespace PicoTorrentBootstrapper
{
    public sealed class PicoBA : BootstrapperApplication
    {
        public static Dispatcher Dispatcher { get; private set; }

        public static Window View { get; private set; }

        protected override void Run()
        {
            try
            {
                Dispatcher = Dispatcher.CurrentDispatcher;
                View = new MainView();
                View.Closed += (sender, e) => Dispatcher.InvokeShutdown();
                View.Loaded += (sender, e) => Engine.CloseSplashScreen();

                Engine.Log(LogLevel.Verbose, "Launching PicoTorrent bootstrapper");

                var viewModel = new MainViewModel(this, View);
                View.Closed += (sender, e) => Dispatcher.InvokeShutdown();
                View.DataContext = viewModel;
                View.Show();

                Engine.Detect();

                Dispatcher.Run();

                Engine.Quit(viewModel.Result);
            }
            catch (Exception e)
            {
                Engine.Log(LogLevel.Error, $"Failed to run bootstrapper: {e}");
                throw;
            }
        }
    }
}
