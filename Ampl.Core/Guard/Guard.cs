namespace Ampl.Core
{
    /// <summary>
    /// Provides an entry point to a set of Guard clauses defined as extension methods on <see cref="IGuardClause"/>.
    /// </summary>
    /// <remarks>This is a port of Ardalis GuardClauses without extra dependencies.</remarks>
    public class Guard : IGuardClause
    {
        private Guard()
        { }

        /// <summary>
        /// An entry point to a set of Guard Clauses.
        /// </summary>
        public static IGuardClause Against = new Guard();
    }
}
