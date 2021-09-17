using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BAStudio.Unity.KineticMovementLab
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField]
        long startTick;
        double tick;
        [SerializeField]
        public Movement[] targets;
        
        void Start ()
        {
            tick = startTick;
        }
        void Update()
        {
            tick += Time.unscaledDeltaTime * 1000f;
        }
    }
}