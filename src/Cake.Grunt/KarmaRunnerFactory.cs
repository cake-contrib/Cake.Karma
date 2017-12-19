using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Karma
{
    /// <summary>
    /// The karma runner factory, determines which runner to use based on run mode.
    /// </summary>
    public sealed class KarmaRunnerFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly IToolLocator _tools;


        /// <summary>
        /// Default constructor for <see cref="KarmaRunnerFactory" />.
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="environment"></param>
        /// <param name="processRunner"></param>
        /// <param name="tools"></param>
        public KarmaRunnerFactory(
            IFileSystem fileSystem, 
            ICakeEnvironment environment, 
            IProcessRunner processRunner,
            IToolLocator tools)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _processRunner = processRunner;
            _tools = tools;
        }


        /// <summary>
        /// Creates a new runner based on run mode.
        /// </summary>
        /// <typeparam name="TSettings">The type of settings to use for the runner.</typeparam>
        /// <param name="runMode">The run mode of the command.</param>
        /// <returns>A fully constructed <see cref="KarmaRunner{TSettings}"/>.</returns>
        public KarmaRunner<TSettings> CreateRunner<TSettings>(KarmaRunMode runMode) where TSettings : KarmaSettings, new()
        {
            return runMode == KarmaRunMode.Global 
                ? new KarmaRunner<TSettings>(_fileSystem, _environment, _processRunner, _tools) 
                : new KarmaRunnerLocal<TSettings>(_fileSystem, _environment, _processRunner, _tools);
        }
    }
}
