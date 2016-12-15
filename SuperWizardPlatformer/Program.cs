using System;
using System.IO;
using System.Runtime;

namespace SuperWizardPlatformer
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var logFile = new StreamWriter("SuperWizardPlatformer.log"))
            {
                // Re-route all console output to log.
                Console.SetOut(logFile);
                Console.SetError(logFile);

                // Change process-wide settings.
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

                // Write start-of-process info to log.
                Console.WriteLine("SuperWizardPlatformer v0.1");
                Console.WriteLine("Running {0}-bit process on {1}-bit system",
                    Environment.Is64BitProcess ? 64 : 32,
                    Environment.Is64BitOperatingSystem ? 64 : 32);
                Console.WriteLine("GCLatencyMode: {0}", GCSettings.LatencyMode);

                using (var game = new Game1())
                {
                    game.Run();
                }

                logFile.Flush();
            }
        }
    }
#endif
}
