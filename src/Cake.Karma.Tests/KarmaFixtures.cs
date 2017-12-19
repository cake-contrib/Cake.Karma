using Cake.Testing.Fixtures;

namespace Cake.Karma.Tests
{
    public class KarmaGlobalFixture<TSettings> : ToolFixture<TSettings> where TSettings : KarmaSettings, new()
    {
        public TSettings Settings { get; set; }

        public KarmaGlobalFixture() 
            : base("karma")
        {
        }

        protected override void RunTool()
        {
            var tool = new KarmaRunner<TSettings>(FileSystem, Environment, ProcessRunner, Tools);
            tool.Execute(Settings);
        }
    }



    public class KarmaLocalFixture<TSettings> : ToolFixture<TSettings> where TSettings : KarmaSettings, new()
    {
        public TSettings Settings { get; set; }

        public KarmaLocalFixture()
            : base("node")
        {
        }

        protected override void RunTool()
        {
            var tool = new KarmaRunnerLocal<TSettings>(FileSystem, Environment, ProcessRunner, Tools);
            tool.Execute(Settings);
        }
    }
}
