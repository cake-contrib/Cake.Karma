using System;
using Cake.Testing;
using Shouldly;
using Xunit;

namespace Cake.Karma.Tests
{
    public class GeneralSettingsTests
    {
        private readonly KarmaLocalFixture<KarmaSettings> _localFixture;
        private readonly KarmaGlobalFixture<KarmaSettings> _globalFixture;
        private readonly string _configFile;


        public GeneralSettingsTests()
        {
            _localFixture = new KarmaLocalFixture<KarmaSettings>();
            _globalFixture = new KarmaGlobalFixture<KarmaSettings>();
            _configFile = "karma.conf.js";

            _localFixture.FileSystem.CreateFile(_configFile);
            _globalFixture.FileSystem.CreateFile(_configFile);
        }


        [Fact]
        public void LocalRunnerRequiresCorrectLocalRunMode()
        {
            _localFixture.Settings = new KarmaStartSettings
            {
                ConfigFile = _configFile
            };

            Should.Throw<InvalidOperationException>(() => _localFixture.Run());
        }
    }
}
