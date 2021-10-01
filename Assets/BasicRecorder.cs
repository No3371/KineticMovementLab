namespace BAStudio.Unity.KineticMovementLab
{
    public class BasicRecorder : Recorder<Record>
    {
        protected override void Update ()
        {
            base.Update();
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].ShouldRecord())
                {
                    Record item = targets[i].Record();
                    item.tickMS = tick;
                    records[i].Add(item);
                }
            }
        }
    }
}