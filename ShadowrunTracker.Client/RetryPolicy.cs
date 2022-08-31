namespace ShadowrunTracker.Client
{
    using Microsoft.AspNetCore.SignalR.Client;
    using System;

    public class RetryPolicy : IRetryPolicy
    {
        private readonly Random _rand = new();
        private readonly int _maxRetries;
        private readonly int _maxWaitTime;

        public RetryPolicy(int maxRetries, int maxWaitTime)
        {
            _maxRetries = maxRetries;

            _maxWaitTime = (maxWaitTime <= 0 ? 64 : maxWaitTime) * 1000;
        }

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            if (_maxRetries >= 0 && retryContext.PreviousRetryCount >= _maxRetries)
            {
                return null;
            }

            var ms = Math.Max(Math.Pow(2, retryContext.PreviousRetryCount) * _rand.Next(1000, 2000), _maxWaitTime);

            return TimeSpan.FromMilliseconds(ms);
        }
    }
}
