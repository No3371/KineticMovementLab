using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace BAStudio.Unity.KineticMovementLab
{
    public class FollowSelection : MonoBehaviour
    {
        public Canvas canvas;
        public RectTransform buttonContainer;
        public Button prefabSelection;

        // Disabled by default
        void Awake ()
        {
            this.enabled = false;
        }

        public void Setup (GameObject[] subjects, CinemachineVirtualCamera camera)
        {
            this.enabled = true;
            foreach (var t in subjects)
            {
                var b = GameObject.Instantiate<Button>(prefabSelection, buttonContainer);
                b.gameObject.SetActive(true);
                b.onClick.AddListener(() => {
                    if (camera.Follow == t) return;
                    // if (camera.Follow != null)
                    // {
                    //     var offset = (camera.Follow.position - t.position);
                    //     camera.
                    // }
                    camera.Follow = t.transform;
                    camera.LookAt = t.transform;
                });
                b.GetComponentInChildren<Text>().text = t.name;
            }
        }
    }
}
