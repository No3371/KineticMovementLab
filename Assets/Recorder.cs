using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiscUtil.Collections;
using Unity.VisualScripting;
using UnityEngine;


namespace BAStudio.Unity.KineticMovementLab
{

    public abstract class Recorder<R> : RecorderBase where R : Record
    {
        [SerializeField]
        long startTick;
        protected double tick;
        [NonSerialized]
        internal MovementBase<R>[] targets;
        public List<R>[] records;

        public override void Setup(GameObject[] targets)
        {
            this.targets = new MovementBase<R>[targets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                this.targets[i] = targets[i].GetComponent<MovementBase<R>>();
            }

            records = new List<R>[targets.Length];
            for (int i = 0; i < records.Length; i++)
            {
                records[i] = new List<R>();
            }
        }

        protected virtual void Start ()
        {
            tick = startTick;
        }
        
        protected virtual void Update()
        {
            tick += Time.unscaledDeltaTime * 1000f;
        }
    }
}