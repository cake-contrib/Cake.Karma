using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Karma
{
    /// <summary>
    /// Returns a gulp runner based on either a local or global gulp installation via npm
    /// </summary>
    public class KarmaLocalRunnerFactory : KarmaRunnerFactory
    {
        public KarmaRunnerLocal<KarmaStartSettings> Start => CreateRunner<KarmaStartSettings>();
        public KarmaRunnerLocal<KarmaRunSettings> Run => CreateRunner<KarmaRunSettings>();
        public KarmaRunnerLocal<KarmaSettings> Init => CreateRunner<KarmaSettings>();

        public KarmaLocalRunnerFactory(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
        }

        private KarmaRunnerLocal<TSettings> CreateRunner<TSettings>() where TSettings : KarmaSettings, new()
            => new KarmaRunnerLocal<TSettings>(FileSystem, Environment, ProcessRunner, Tools);
    }



    public class KarmaGlobalRunnerFactory : KarmaRunnerFactory
    {
        public KarmaRunnerGlobal<KarmaStartSettings> Start => CreateRunner<KarmaStartSettings>();
        public KarmaRunnerGlobal<KarmaRunSettings> Run => CreateRunner<KarmaRunSettings>();
        public KarmaRunnerGlobal<KarmaSettings> Init => CreateRunner<KarmaSettings>();

        public KarmaGlobalRunnerFactory(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) 
            : base(fileSystem, environment, processRunner, tools)
        {
        }

        private KarmaRunnerGlobal<TSettings> CreateRunner<TSettings>() where TSettings : KarmaSettings, new() 
            => new KarmaRunnerGlobal<TSettings>(FileSystem, Environment, ProcessRunner, Tools);
    }



    public abstract class KarmaRunnerFactory
    {
        protected IFileSystem FileSystem { get; }
        protected ICakeEnvironment Environment { get; }
        protected IProcessRunner ProcessRunner { get; }
        protected IToolLocator Tools { get; }


        protected KarmaRunnerFactory(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
        {
            FileSystem = fileSystem;
            Environment = environment;
            ProcessRunner = processRunner;
            Tools = tools;
        }
    }
}