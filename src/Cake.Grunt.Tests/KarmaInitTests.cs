using System;
using System.IO;
using Cake.Testing;
using Shouldly;
using Xunit;

namespace Cake.Karma.Tests
{
    public class KarmaInitGlobalTests
    {
        private readonly KarmaGlobalFixture<KarmaSettings> _fixture;
        private readonly string _configFile;


        public KarmaInitGlobalTests()
        {
            _fixture = new KarmaGlobalFixture<KarmaSettings>();
            _configFile = "karma.conf.js";
            _fixture.FileSystem.CreateFile(_configFile);
        }


        [Fact]
        public void ConfigFileIsRequired()
        {
            _fixture.Settings = new KarmaSettings();

            Should.Throw<ArgumentNullException>(() => _fixture.Run());
        }

        [Fact]
        public void SettingsShouldAddConfFile()
        {
            _fixture.Settings = new KarmaSettings
            {
                ConfigFile = _configFile,
            };

            var result = _fixture.Run();

            result.Args.ShouldBe("init \"karma.conf.js\"");
        }
    }



    public class KarmaInitLocalTests
    {
        private readonly KarmaLocalFixture<KarmaSettings> _fixture;
        private readonly string _configFile;


        public KarmaInitLocalTests()
        {
            _fixture = new KarmaLocalFixture<KarmaSettings>();
            _configFile = "karma.conf.js";

            _fixture.FileSystem.CreateFile(_configFile);
        }


        [Fact]
        public void SettingsShouldDefaultCliFile()
        {
            _fixture.FileSystem.CreateFile(KarmaSettings.DefaultCliFile);

            _fixture.Settings = new KarmaSettings
            {
                ConfigFile = _configFile,
            };

            var result = _fixture.Run();

            result.Args.ShouldBe($"\"{KarmaSettings.DefaultCliFile}\" init \"karma.conf.js\"");
        }

        [Fact]
        public void SpecifiedCliFileShouldExist()
        {
            _fixture.Settings = new KarmaStartSettings
            {
                ConfigFile = _configFile,
                PathToKarmaCli = "karma-cli"
            };

            Should.Throw<FileNotFoundException>(() => _fixture.Run());
        }
    }
}
