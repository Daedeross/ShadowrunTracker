using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public interface IRecordViewModel<TRecord> : IViewModel
    {
        TRecord ToRecord();
    }
}
