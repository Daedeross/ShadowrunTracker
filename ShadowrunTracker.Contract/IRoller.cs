using ShadowrunTracker.Model;

namespace ShadowrunTracker
{
    public interface IRoller
    {
        PoolResult RollDice(int count);
    }
}
