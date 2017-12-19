using System;
using System.IO;
using Cake.Testing;
using Shouldly;
using Xunit;

namespace Cake.Karma.Tests
{
    public class KarmaRunGlobalTests
    {
        private readonly KarmaGlobalFixture<KarmaRunSettings> _fixture;
        private readonly string _configFile;


        public KarmaRunGlobalTests()
        {
            _fixture = new KarmaGlobalFixture<KarmaRunSettings>();
            _configFile = "karma.conf.js";
            _fixture.FileSystem.CreateFile(_configFile);
        }


        [Fact]
        public void ConfigFileIsRequired()
        {
            _fixture.Settings = new KarmaRunSettings();

            Should.Throw<ArgumentNullException>(() => _fixture.Run());
        }

        [Fact]
        public void SettingsShouldAddConfFile()
        {
            _fixture.Settings = new KarmaRunSettings
            {
                ConfigFile = _configFile,
            };

            var result = _fixture.Run();

            result.Args.ShouldBe("run \"karma.conf.js\"");
        }
    }



    public class KarmaRunLocalTests
    {
        private readonly KarmaLocalFixture<KarmaRunSettings> _fixture;
        private readonly string _configFile;


        public KarmaRunLocalTests()
        {
            _fixture = new KarmaLocalFixture<KarmaRunSettings>();
            _configFile = "karma.conf.js";

            _fixture.FileSystem.CreateFile(_configFile);
        }


        [Fact]
        public void SettingsShouldDefaultCliFile()
        {
            _fixture.FileSystem.CreateFile(KarmaSettings.DefaultLocalKarmaCli);

            _fixture.Settings = new KarmaRunSettings
            {
                ConfigFile = _configFile,
                RunMode = KarmaRunMode.Local
            };

            var result = _fixture.Run();

            result.Args.ShouldBe($"\"{KarmaSettings.DefaultLocalKarmaCli}\" run \"karma.conf.js\"");
        }

        [Fact]
        public void SpecifiedCliFileShouldExist()
        {
            _fixture.Settings = new KarmaRunSettings
            {
                ConfigFile = _configFile,
                LocalKarmaCli = "karma-cli",
                RunMode = KarmaRunMode.Local
            };

            Should.Throw<FileNotFoundException>(() => _fixture.Run());
        }
    }
}
