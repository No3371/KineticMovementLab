using MessagePack;
using UnityEngine;


namespace BAStudio.Unity.KineticMovementLab
{
    public class Record
    {
        public double tickMS { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
    }
}