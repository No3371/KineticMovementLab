using MessagePack;
using UnityEngine;


namespace BAStudio.Unity.KineticMovementLab
{
    public class Record
    {
        public long Tick { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}