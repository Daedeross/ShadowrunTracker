using ShadowrunTracker.Contract.Model;

namespace ShadowrunTracker.Contract
{
    public interface IRoller
    {
        PoolResult RollDice(int count);
    }
}
