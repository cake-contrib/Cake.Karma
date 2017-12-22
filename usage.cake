#addin "Cake.Karma"

Task("Default")
    .Does(() => 
    {
        Information("Running Global Karma");

        var settings = new KarmaStartSettings
        {
           ConfigFile = "karma.conf.js"
        };

        KarmaStart(settings);
    });
        
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

RunTarget(target);    