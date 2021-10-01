using System;
using UnityEngine;

namespace BAStudio.Unity.KineticMovementLab
{
    [Flags]
    public enum CollisionFace
    {
        Above,
        Left,
        Right,
        Forward,
        Backward,
        Below
    }

    public delegate void HitDelegate(CollisionEvent e);

    public class RaycastCollider : MonoBehaviour
    {
        public struct RaycastSetup
        {
            public CollisionFace face;
            public Vector3 offset;
            public Vector3 direction;
            public float length;
            public LayerMask layerMask;
            public QueryTriggerInteraction queryTriggerInteraction;
        }

        public RaycastSetup[] setup;
        RaycastHit[] hitCache;
        RaycastHit[] hitBuffer;
        public event HitDelegate Hit;
        [SerializeField]
        int bufferSize = 8, maxHitPerRay = 1;
        [SerializeField]
        bool castOnFixedUpdate;
        void Awake ()
        {
            hitCache = new RaycastHit[maxHitPerRay];
            hitBuffer = new RaycastHit[bufferSize];
        }

        public CollisionEvent Cast (Vector3 offset)
        {
            CollisionFace hitFaces = 0;
            int hits = 0;
            foreach (var rs in setup)
            {
                Ray ray = new Ray(this.transform.position + rs.offset + offset, rs.direction);
                int hitCount = Physics.RaycastNonAlloc(ray, hitCache, rs.length, rs.layerMask, rs.queryTriggerInteraction);
                for (int i = 0; i < hitCount; i++)
                {
                    if (hitCache[i].collider != null)
                    {
                        hitFaces |= rs.face;
                        hitBuffer[hits] = hitCache[i];
                        hits++;
                    }
                }
            }
            return new CollisionEvent {
                faces = hitFaces,
                hits = new Span<RaycastHit>(hitBuffer, 0, hits)
            };
        }

        void FixedUpdate ()
        {
            if (castOnFixedUpdate)
            {
                var ce = Cast(Vector3.zero);
                if (ce.faces != 0)
                {
                    Hit(ce);
                }
            }
        }
    }

    public ref struct CollisionEvent
    {
        public CollisionFace faces;
        public Span<RaycastHit> hits;
    }

}
