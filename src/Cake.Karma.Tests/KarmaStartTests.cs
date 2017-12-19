using System;
using System.IO;
using Cake.Testing;
using Shouldly;
using Xunit;

namespace Cake.Karma.Tests
{
    public class KarmaStartGlobalTests
    {
        private readonly KarmaGlobalFixture<KarmaStartSettings> _fixture;
        private readonly string _configFile;


        public KarmaStartGlobalTests()
        {
            _fixture = new KarmaGlobalFixture<KarmaStartSettings>();
            _configFile = "karma.conf.js";
            _fixture.FileSystem.CreateFile(_configFile);
        }


        [Fact]
        public void ConfigFileIsRequired()
        {
            _fixture.Settings = new KarmaStartSettings();

            Should.Throw<ArgumentNullException>(() => _fixture.Run());
        }

        [Fact]
        public void SettingsShouldAddConfFile()
        {
            _fixture.Settings = new KarmaStartSettings
            {
                ConfigFile = _configFile,
            };

            var result = _fixture.Run();

            result.Args.ShouldBe("start \"karma.conf.js\"");
        }
    }



    public class KarmaStartLocalTests
    {
        private readonly KarmaLocalFixture<KarmaStartSettings> _fixture;
        private readonly string _configFile;


        public KarmaStartLocalTests()
        {
            _fixture = new KarmaLocalFixture<KarmaStartSettings>();
            _configFile = "karma.conf.js";

            _fixture.FileSystem.CreateFile(_configFile);
        }


        [Fact]
        public void SettingsShouldDefaultCliFile()
        {
            _fixture.FileSystem.CreateFile(KarmaSettings.DefaultCliFile);

            _fixture.Settings = new KarmaStartSettings
            {
                ConfigFile = _configFile,
                RunMode = KarmaRunMode.Local
            };

            var result = _fixture.Run();

            result.Args.ShouldBe($"\"{KarmaSettings.DefaultCliFile}\" start \"karma.conf.js\"");
        }

        [Fact]
        public void SpecifiedCliFileShouldExist()
        {
            _fixture.Settings = new KarmaStartSettings
            {
                ConfigFile = _configFile,
                LocalKarmaCli = "karma-cli",
                RunMode = KarmaRunMode.Local
            };

            Should.Throw<FileNotFoundException>(() => _fixture.Run());
        }
    }
}
