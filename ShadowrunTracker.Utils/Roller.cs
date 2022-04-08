using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowrunTracker.Utils
{
    public class Roller : IRoller
    {
        private const int TargetNumber = 5;

        public static IRoller Default { get; } = new Roller();

        private readonly Random _rand;

        public Roller()
        {
            _rand = new Random();
        }

        public Roller(int seed)
        {
            _rand = new Random(seed);
        }

        private int D6() => _rand.Next(1, 6);

        public PoolResult RollDice(int count)
        {
            int[] rolls = new int[count];

            for (int i = 0; i < count; i++)
            {
                rolls[i] = D6();
            }

            return new PoolResult
            {
                Pool = count,
                Hits = rolls.Count(r => r >= TargetNumber),
                Ones = rolls.Count(r => r == 1),
                Sum = rolls.Sum()
            };
        }

        public int Next()
        {
            return _rand.Next();
        }
    }
}
