using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShadowrunTracker.Communication
{
    public interface IEncounterHubClient
    {
        Task RecieveState();
    }
}
