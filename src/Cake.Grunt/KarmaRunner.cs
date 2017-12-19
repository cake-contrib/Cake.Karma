using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Karma
{
    public sealed class KarmaRunnerLocal<TSettings> : KarmaRunner<TSettings>
        where TSettings : KarmaSettings, new()
    {
        public KarmaRunnerLocal(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) 
            : base(fileSystem, environment, processRunner, tools)
        {
        }


        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "node.exe";
            yield return "node";
            yield return "nodejs";
        }

        /// <summary>
        /// Executes karma
        /// </summary>
        public override void Execute(Action<TSettings> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var settings = new TSettings();
            configure.Invoke(settings);
            Execute(settings);
        }

        public override void Execute(TSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            // Default the tool file if it is null.
            if (settings.LocalKarmaCli == null)
            {
                settings.LocalKarmaCli = KarmaSettings.DefaultCliFile;
            }

            ValidateSettings(settings);

            var args = new ProcessArgumentBuilder();
            args.AppendQuoted(settings.LocalKarmaCli.ToString());
            args.Append(settings.Command);
            settings.Evaluate(args);
            Run(settings, args);
        }

        protected override void ValidateSettings(TSettings settings)
        {
            base.ValidateSettings(settings);

            if (settings.RunMode != KarmaRunMode.Local)
            {
                throw new InvalidOperationException($"{nameof(KarmaRunnerLocal<TSettings>)} used, but the settings don't specify {nameof(KarmaRunMode.Local)}");
            }
            
            if (!FileSystem.Exist(settings.LocalKarmaCli))
            {
                var cliFile = settings.LocalKarmaCli.MakeAbsolute(Environment).ToString();
                throw new FileNotFoundException($"Cannot find the specified Karma CLI file for KarmaRunMode.Local: {cliFile}");
            }
        }
    }



    /// <summary>
    /// The base runner for global karma commands.
    /// </summary>
    public class KarmaRunner<TSettings> : Tool<TSettings> where TSettings : KarmaSettings, new()
    {
        protected ICakeEnvironment Environment { get; }
        protected IFileSystem FileSystem { get; }


        public KarmaRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) 
            : base(fileSystem, environment, processRunner, tools)
        {
            Environment = environment;
            FileSystem = fileSystem;
        }


        protected override string GetToolName()
        {
            return "Karma Runner";
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "karma.cmd";
            yield return "karma";
        }

        /// <summary>
        /// Executes karma
        /// </summary>
        public virtual void Execute(Action<TSettings> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var settings = new TSettings();
            configure.Invoke(settings);

            Execute(settings);
        }

        public virtual void Execute(TSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            ValidateSettings(settings);

            var args = new ProcessArgumentBuilder();
            args.Append(settings.Command);
            settings.Evaluate(args);
            Run(settings, args);
        }

        /// <summary>
        /// Validates settings
        /// </summary>
        /// <param name="settings">the settings class</param>
        /// <exception cref="FileNotFoundException">when config file does not exist</exception>
        protected virtual void ValidateSettings(TSettings settings)
        {
            if (settings.ConfigFile == null)
            {
                throw new ArgumentNullException("settings.ConfigFile", "A config file must be specified.");
            }

            if (!FileSystem.Exist(settings.ConfigFile))
            {
                var configFile = settings.ConfigFile.MakeAbsolute(Environment).ToString();
                throw new FileNotFoundException($"Cannot find the specified config file: {configFile}");
            }
        }
    }
}
