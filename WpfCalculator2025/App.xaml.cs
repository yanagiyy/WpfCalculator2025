using System.Reflection;
using System.Windows;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net;
using System.IO;    // ビルド構成によって必要になるので消さないように

namespace WpfCalculator2025
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SetupLogging();
            base.OnStartup(e);
        }

        private void SetupLogging()
        {
            var repo = (Hierarchy)LogManager.GetRepository(Assembly.GetEntryAssembly());
            repo.Root.RemoveAllAppenders();

#if DEBUG
            // デバッグビルド：デバッグ出力
            var debugAppender = new DebugAppender
            {
                Layout = new PatternLayout("[%date{HH:mm:ss.fff}] %-5level %logger - %message%newline")
            };
            debugAppender.ActivateOptions();
            repo.Root.AddAppender(debugAppender);
#else
            // リリースビルド：ファイルへ追記のみ (ローリングなし)
            var logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            var fileAppender = new FileAppender
            {
                File           = logFile,
                AppendToFile   = true,  // 追記モード
                LockingModel   = new FileAppender.MinimalLock(),
                Layout         = new PatternLayout("[%date{yyyy-MM-dd HH:mm:ss.fff}] %-5level %logger - %message%newline")
            };
            fileAppender.ActivateOptions();
            repo.Root.AddAppender(fileAppender);
#endif

            repo.Root.Level = log4net.Core.Level.Debug;
            repo.Configured = true;
        }
    }
}
