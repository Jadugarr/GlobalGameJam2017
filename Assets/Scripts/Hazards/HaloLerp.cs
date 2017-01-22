using UnityEngine;

namespace Assets.Scripts.Hazards
{
    public class HaloLerp : MonoBehaviour
    {
        [SerializeField]
        private Vector3 targetSize;

        [SerializeField]
        private float lerpTime;

        private Vector3 startingSize;
        private Behaviour halo;
        private bool makeBigger = true;

        void Awake()
        {
            halo = (Behaviour) GetComponent("Halo");
            startingSize = halo.transform.localScale;
        }

        void Update()
        {
            Vector3 velocity = Vector3.zero;
            if (makeBigger)
            {
                Vector3.SmoothDamp(startingSize, targetSize, ref velocity, lerpTime);
                if (halo.transform.localScale.magnitude == targetSize.magnitude)
                {
                    makeBigger = false;
                }
            }
            else
            {
                Vector3.SmoothDamp(targetSize, startingSize, ref velocity, lerpTime);
                if (halo.transform.localScale.magnitude == startingSize.magnitude)
                {
                    makeBigger = false;
                }
            }

            
        }
    }
}
