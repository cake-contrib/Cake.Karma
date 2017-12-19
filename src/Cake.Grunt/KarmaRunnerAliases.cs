using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Karma
{
    /// <summary>
    /// contains functionality to interact with karma
    /// </summary>
    [CakeAliasCategory("Node")]
    public static class KarmaRunnerAliases
    {
        /// <summary>
        /// Allows access to the karma test runner for the local installation.
        /// </summary>
        /// <param name="context">The cake context</param>
        /// <returns></returns>
        /// <example>
        /// <para>Run 'karma' from your local karma installation</para>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("Karma")
        ///     .Does(() =>
        /// {
        ///     KarmaLocal.Start(new KarmaStartSettings());
        /// });
        /// ]]>
        /// </code>
        /// </example>
        [CakePropertyAlias(Cache = true)]
        public static KarmaLocalRunnerFactory KarmaLocal(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new KarmaLocalRunnerFactory(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        }

        /// <summary>
        /// Allows access to the karma test runner for the global installation.
        /// </summary>
        /// <param name="context">The cake context</param>
        /// <returns></returns>
        /// <example>
        /// <para>Run 'karma' from your global karma installation</para>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("Karma")
        ///     .Does(() =>
        /// {
        ///     KarmaGlobal.Start(new KarmaStartSettings());
        /// });
        /// ]]>
        /// </code>
        /// </example>
        [CakePropertyAlias(Cache = true)]
        public static KarmaGlobalRunnerFactory KarmaGlobal(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new KarmaGlobalRunnerFactory(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        }
    }
}
