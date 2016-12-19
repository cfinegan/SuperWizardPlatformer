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
            StreamWriter logFile = null;

            try
            {
                logFile = new StreamWriter("SuperWizardPlatformer.log");

                // Re-route all console output to log.
                Console.SetOut(logFile);
                Console.SetError(logFile);
            }
            catch (Exception e)
            {
                Console.OpenStandardOutput();
                Console.OpenStandardError();
                Console.Error.WriteLine(e);
            }

            try
            {
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
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            finally
            {
                if (logFile != null)
                {
                    logFile.Close();
                }
            }
        }
    }
#endif
}
