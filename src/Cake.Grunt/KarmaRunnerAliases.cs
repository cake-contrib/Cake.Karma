using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Karma
{
    /// <summary>
    /// contains functionality to interact with karma
    /// </summary>
    [CakeAliasCategory("Node")]
    public static class KarmaAliases
    {
        /// <summary>
        /// Runs karma start with a function for settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configureSettings">Function to supply for configuring settings.</param>
        /// <example>
        /// <para>Define the configuration file</para>
        /// <code>
        /// <![CDATA[
        ///     KarmaStart(settings => 
        ///     {
        ///         settings.ConfigFile = "karma.conf.js";
        ///     });
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void KarmaStart(this ICakeContext context, Action<KarmaStartSettings> configureSettings)
        {
            FromLambda(context, configureSettings);
        }

        /// <summary>
        /// Runs karma start with a function for settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The command settings.</param>
        /// <example>
        /// <para>Define the configuration file</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new KarmaStartSettings
        ///         {
        ///            ConfigFile = "karma.conf.js"
        ///         };
        ///     KarmaStart(settings);
        /// ]]>
        /// </code>
        /// <para>Run locally by specifying the run mode</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new KarmaStartSettings
        ///         {
        ///             RunMode = KarmaRunMode.Local,
        ///             ConfigFile = "karma.conf.js"
        ///         };
        ///     KarmaStart(settings);
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void KarmaStart(this ICakeContext context, KarmaStartSettings settings)
        {
            FromSettings(context, settings);
        }

        /// <summary>
        /// Runs karma run with a function for settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configureSettings">Function to supply for configuring settings.</param>
        /// <example>
        /// <para>Define the configuration file</para>
        /// <code>
        /// <![CDATA[
        ///     KarmaRun(settings => 
        ///     {
        ///         settings.ConfigFile = "karma.conf.js";
        ///     });
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void KarmaRun(this ICakeContext context, Action<KarmaRunSettings> configureSettings)
        {
            FromLambda(context, configureSettings);
        }

        /// <summary>
        /// Runs karma run with a function for settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The command settings.</param>
        /// <example>
        /// <para>Define the configuration file</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new KarmaRunSettings
        ///         {
        ///            ConfigFile = "karma.conf.js"
        ///         };
        ///     KarmaRun(settings);
        /// ]]>
        /// </code>
        /// <para>Run locally by specifying the run mode</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new KarmaRunSettings
        ///         {
        ///             RunMode = KarmaRunMode.Local,
        ///             ConfigFile = "karma.conf.js"
        ///         };
        ///     KarmaRun(settings);
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void KarmaRun(this ICakeContext context, KarmaRunSettings settings)
        {
            FromSettings(context, settings);
        }

        /// <summary>
        /// Runs karma init with a function for settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configureSettings">Function to supply for configuring settings.</param>
        /// <example>
        /// <para>Define the configuration file</para>
        /// <code>
        /// <![CDATA[
        ///     KarmaInit(settings => 
        ///     {
        ///         settings.ConfigFile = "karma.conf.js";
        ///     });
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void KarmaInit(this ICakeContext context, Action<KarmaSettings> configureSettings)
        {
            FromLambda(context, configureSettings);
        }

        /// <summary>
        /// Runs karma init with a function for settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The command settings.</param>
        /// <example>
        /// <para>Define the configuration file</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new KarmaSettings
        ///         {
        ///            ConfigFile = "karma.conf.js"
        ///         };
        ///     KarmaInit(settings);
        /// ]]>
        /// </code>
        /// <para>Run locally by specifying the run mode</para>
        /// <code>
        /// <![CDATA[
        ///     var settings =
        ///         new KarmaSettings
        ///         {
        ///             RunMode = KarmaRunMode.Local,
        ///             ConfigFile = "karma.conf.js"
        ///         };
        ///     KarmaInit(settings);
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void KarmaInit(this ICakeContext context, KarmaSettings settings)
        {
            FromSettings(context, settings);
        }

        private static void FromLambda<TSettings>(ICakeContext context, Action<TSettings> configureSettings) where TSettings : KarmaSettings, new()
        {
            var settings = new TSettings();
            configureSettings(settings);
            FromSettings(context, settings);
        }

        private static void FromSettings<TSettings>(ICakeContext context, TSettings settings) where TSettings : KarmaSettings, new()
        {
            var factory = new KarmaRunnerFactory(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            var runner = factory.CreateRunner<TSettings>(settings.RunMode);
            runner.Execute(settings);
        }
    }
}
