// #addin "Cake.Grunt"
#r "artifacts\build\Cake.Grunt.dll"

Task("Default")
    .Does(() => 
    {
        try {
            Information("Running Global Karma");
            // Executes Karma from a global installation (npm install -g karma-cli)
            Karma.Global.Execute();
        } catch(Exception ex) {
            Error(ex.ToString());
        }
        
        try {
            Information("Running Local Karma");
            // Executes Karma from a local installation (npm install karma-cli)
            Karma.Local.Execute(settings => settings.WithGruntFile("gruntfile.js"));
        } catch(Exception ex) {
            Error(ex.ToString());
        }
    });
        
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

RunTarget(target);    