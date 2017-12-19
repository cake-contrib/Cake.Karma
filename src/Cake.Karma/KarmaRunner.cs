using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Karma
{
    /// <summary>
    /// The karma runner for local mode execution.
    /// </summary>
    /// <typeparam name="TSettings">The type of settings to provide for the relevant karma command (start, run, init).</typeparam>
    public sealed class KarmaRunnerLocal<TSettings> : KarmaRunner<TSettings>
        where TSettings : KarmaSettings, new()
    {
        /// <summary>
        /// Default constructor for <see cref="KarmaRunnerLocal{TSettings}" />.
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="environment"></param>
        /// <param name="processRunner"></param>
        /// <param name="tools"></param>
        public KarmaRunnerLocal(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) 
            : base(fileSystem, environment, processRunner, tools)
        {
        }


        /// <summary>
        /// The name of the tool, used during logging.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Karma Runner (Local)";
        }

        /// <summary>
        /// Available tool executable names for local karma runs.
        /// </summary>
        /// <returns>The available tool execution names.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "node.exe";
            yield return "node";
            yield return "nodejs";
        }

        /// <summary>
        /// Execute the runner with the specified settings. If LocalKarmaCli is not set, it defaults to <see cref="KarmaSettings.DefaultCliFile" />.
        /// </summary>
        /// <param name="settings">Command settings.</param>
        /// <exception cref="ArgumentNullException">Thrown if settings is null.</exception>
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

        /// <summary>
        /// Validates the provides settings for local mode.
        /// </summary>
        /// <param name="settings">The settings to validate.</param>
        /// <exception cref="InvalidOperationException">Thrown when the runner is used when settings.RunMode is not Local.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the Karma CLI file cannot be located.</exception>
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
    /// The karma runner for global mode execution.
    /// </summary>
    /// <typeparam name="TSettings">The type of settings to provide for the relevant karma command (start, run, init).</typeparam>
    public class KarmaRunner<TSettings> : Tool<TSettings> where TSettings : KarmaSettings, new()
    {
        /// <summary>
        /// A reference to the supplied <see cref="ICakeEnvironment" />.
        /// </summary>
        protected ICakeEnvironment Environment { get; }
        /// <summary>
        /// A reference to the supplied <see cref="IFileSystem" />.
        /// </summary>
        protected IFileSystem FileSystem { get; }


        /// <summary>
        /// Default constructor for <see cref="KarmaRunner{TSettings}" />.
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="environment"></param>
        /// <param name="processRunner"></param>
        /// <param name="tools"></param>
        public KarmaRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) 
            : base(fileSystem, environment, processRunner, tools)
        {
            Environment = environment;
            FileSystem = fileSystem;
        }


        /// <summary>
        /// The name of the tool, used during logging.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Karma Runner (Global)";
        }

        /// <summary>
        /// Available tool executable names for local karma runs.
        /// </summary>
        /// <returns>The available tool execution names.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "karma.cmd";
            yield return "karma";
        }

        /// <summary>
        /// Execute the runner with the specified settings.
        /// </summary>
        /// <param name="settings">Command settings.</param>
        /// <exception cref="ArgumentNullException">Thrown if settings is null.</exception>
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
        /// Validates the provides settings for global mode.
        /// </summary>
        /// <param name="settings">The settings to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown if the config file setting value is null.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the config file cannot be located.</exception>
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
