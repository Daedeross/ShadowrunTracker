using ShadowrunTracker.Model;

namespace ShadowrunTracker
{
    public interface IRoller
    {
        PoolResult RollDice(int count);

        /// <summary>
        /// Returns a random integer.
        /// Specifics of the distribution are dependent on the implementation.
        /// </summary>
        /// <remarks>
        /// Using .NET's <see cref="System.Random"/> the result will be a non-negatieve 32-bit integer.
        /// </remarks>
        /// <returns>The random integer.</returns>
        int Next();
    }
}
