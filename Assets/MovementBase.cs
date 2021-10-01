using UnityEngine;

namespace BAStudio.Unity.KineticMovementLab
{
    public abstract class MovementBase<R> : MonoBehaviour where R : Record
    {
        public abstract bool ShouldRecord ();
        public abstract R Record ();
    }

}
