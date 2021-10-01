using System.Collections.Generic;
using UnityEngine;

namespace BAStudio.Unity.KineticMovementLab
{
    public abstract class Algorithm<T, M> : ScriptableObject where T : Record where M : SimulatedMovementBase<T, M>
    {
        protected abstract void OnNewRecord (M movement, T record, int index, IReadOnlyList<T> history, long simMS);
        protected abstract void OnUpdate (M Movement, long simMS);
    }

}
