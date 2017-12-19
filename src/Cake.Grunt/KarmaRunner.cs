using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Karma
{
    public sealed class KarmaRunnerLocal<TSettings> : KarmaRunnerGlobal<TSettings>
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
            configure?.Invoke(settings);
            Execute(settings);
        }

        public override void Execute(TSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            ValidateSettings(settings);

            var args = new ProcessArgumentBuilder();
            args.AppendQuoted(settings.PathToKarmaCli.ToString());
            args.Append(settings.Command);
            settings.Evaluate(args);
            Run(settings, args);
        }

        protected override void ValidateSettings(TSettings settings)
        {
            base.ValidateSettings(settings);

            if (settings.PathToKarmaCli == null)
            {
                throw new ArgumentNullException("settings.PathToKarmaCli", "Path to Karma CLI is required when running in local mode.");
            }

            if (!FileSystem.Exist(settings.PathToKarmaCli))
            {
                var cliFile = settings.PathToKarmaCli.MakeAbsolute(Environment).ToString();
                throw new FileNotFoundException($"Cannot find the specified Karma CLI file: {cliFile}");
            }
        }
    }



    /// <summary>
    /// The base runner for global karma commands.
    /// </summary>
    public class KarmaRunnerGlobal<TSettings> : Tool<TSettings> where TSettings : KarmaSettings, new()
    {
        protected ICakeEnvironment Environment { get; }
        protected IFileSystem FileSystem { get; }


        public KarmaRunnerGlobal(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) 
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
            configure?.Invoke(settings);

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
