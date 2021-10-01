using System;
using System.Collections.Generic;
using UnityEngine;

namespace BAStudio.Unity.KineticMovementLab
{
    public class BasicLinearInterpolation : Algorithm<Record, BasicSimulatedMovement>
    {
        [NonSerialized] Record _previous;
        [NonSerialized] Record _latest;
        protected override void OnNewRecord(BasicSimulatedMovement movement, Record record, int index, IReadOnlyList<Record> history, long simMS)
        {
            _previous = _latest;
            _latest = record;
        }
        protected override void OnUpdate(BasicSimulatedMovement movement, long simMS)
        {
            var lerpProgress = (float) ((simMS - _previous.tickMS) / (_latest.tickMS - _previous.tickMS));
            movement.transform.position = Vector3.Lerp(_previous.Position, _latest.Position, lerpProgress);
        }
    }
}
