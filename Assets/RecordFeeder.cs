using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BAStudio.Unity.KineticMovementLab
{
    public class RecordFeeder<T> : MonoBehaviour where T : Record
    {
        public float tickPerSecond, initTick;
        float tick;

        void Start ()
        {
            tick = initTick;
        }

        void Update ()
        {
            tick += tickPerSecond * Time.deltaTime;
        }

        public class Session
        {
            List<T> Records { get; set; }
            float delayNoise;
            int playbackIndex;

            public void OnUpdate (float tick)
            {
                while (playbackIndex < Records.Count && Records[playbackIndex].Tick <= tick)
                {

                }
            }
        }
    }

}