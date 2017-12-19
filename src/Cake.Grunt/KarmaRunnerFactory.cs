using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Karma
{
    public sealed class KarmaRunnerFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly IToolLocator _tools;


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


        public KarmaRunner<TSettings> CreateRunner<TSettings>(KarmaRunMode runMode) where TSettings : KarmaSettings, new()
        {
            return runMode == KarmaRunMode.Global 
                ? new KarmaRunner<TSettings>(_fileSystem, _environment, _processRunner, _tools) 
                : new KarmaRunnerLocal<TSettings>(_fileSystem, _environment, _processRunner, _tools);
        }
    }
}
