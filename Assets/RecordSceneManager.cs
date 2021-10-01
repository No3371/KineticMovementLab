using UnityEngine;
using UnityEngine.Profiling;

namespace BAStudio.Unity.KineticMovementLab
{
    public class RecordSceneManager : MonoBehaviour
    {
        public GameObject[] subjects;
        public Cinemachine.CinemachineVirtualCamera followCamera;
        public FollowSelection followSelection;
        public RecorderBase recorder;

        void Start ()
        {
            Setup();
        }

        public virtual void Setup ()
        {
            recorder.Setup(subjects);
            followCamera.Follow = subjects[0].transform;
            followCamera.LookAt = subjects[0].transform;
            followSelection.Setup(subjects, followCamera);
        }
    }
}