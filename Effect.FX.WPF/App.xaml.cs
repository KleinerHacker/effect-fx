using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using NET.Tools;
using NET.Tools.WPF;

namespace Effect.FX.WPF
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public const int FILENAMES = 0;
        public const int NOSPLASHSWITCH = 1;

        public static ArgumentReader Arguments { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Arguments = new ArgumentReader(false, 
                SwitchArgument.CreateHelpArgument(SwitchCharacter.Slash),
                new PureArgument<String>("File Name", true, true, ""),
                new SwitchArgument(SwitchCharacter.Slash, "nosplash", ""));
            Arguments.AllowUnknownArgs = true;

            if (!Arguments.EvaluateArguments(false, e.Args))
            {
                MessageDialogEx.ShowDialog("The arguments are invalid!", "Error", MessageButtons.OK, MessageIcons.Error);
                return;
            }

            if (Arguments.ArgumentList[NOSPLASHSWITCH].IsSet)
                new MainWindow().Show();
            else
                new Splash().Show();
        }
    }
}
