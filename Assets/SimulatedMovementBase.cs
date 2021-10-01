using UnityEngine;

namespace BAStudio.Unity.KineticMovementLab
{
    public abstract class SimulatedMovementBase<R, M> : MonoBehaviour where R : Record where M : SimulatedMovementBase<R, M>
    {
        public Algorithm<R, M> algorithm;
    }
}
