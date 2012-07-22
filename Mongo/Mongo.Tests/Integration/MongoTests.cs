using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Mongo.Tests.Integration
{
    public class MongoTests
    {
     
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            AssertMongoOnPath();
            RunMongo("db.dropDatabase();");            
        }

        [TearDown]
        public void TestTearDown()
        {
            RunMongo("db.dropDatabase();");                        
        }


        public static void AssertMongoOnPath()
        {
            var path = Environment.GetEnvironmentVariable("Path") ?? String.Empty;
            foreach (var folder in path.Split(';'))
            {
                if (!Directory.Exists(folder)) continue;
                if (Directory.GetFiles(folder).Any(file => file.EndsWith(@"\mongo.exe", StringComparison.InvariantCultureIgnoreCase)))
                {
                    return;
                }
            }
            Assert.Fail("Could not find mongo.exe on your environment's path.  Make sure you've got it installed and on the path.  Check that you've got mongod running while you're at it.");
        }

        private static void RunMongo(string script)
        {
            var psi = new ProcessStartInfo("mongo", string.Format("--norc --eval \"{0}\" test", script))
                          {
                              UseShellExecute = false,
                              CreateNoWindow = true,
                              RedirectStandardOutput = true,
                              RedirectStandardError = true
                          };

            var process = new Process
                              {
                                  StartInfo = psi,
                                  EnableRaisingEvents = true
                              };

            var stdOut = new StringBuilder();
            var stdErr = new StringBuilder();

            Action<StringBuilder, string> handleLine = (builder, line) =>
            {
                if (line == null)
                    return;
                builder.AppendLine(line);
            };

            process.OutputDataReceived += (obj, e) => handleLine(stdOut, e.Data);
            process.ErrorDataReceived += (obj, e) => handleLine(stdErr, e.Data);

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            do
            {
                process.WaitForExit(100);
                process.Refresh();
            } while (!process.HasExited);

            process.WaitForExit();
            var infoOutput = stdOut.ToString();
            var errorOutput = stdErr.ToString();
            var exitCode = process.ExitCode;

            if (exitCode > 0)
            {
                throw new Exception(string.Format("Mongo exited with code {0}", exitCode));
            }
        }
    }
}