using System;
using UnityEngine;


namespace BAStudio.Unity.KineticMovementLab
{
    public abstract class RecorderBase : MonoBehaviour
    {
        public abstract void Setup (GameObject[] targets);
    }
}