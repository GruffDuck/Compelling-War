using UnityEngine;

namespace Worq.AEAI.Enemy
{
    public class TimedDestroy : MonoBehaviour
    {
        private readonly float _delay = AIData.delayB4Destroy;

        private void Start()
        {
            Destroy(gameObject, _delay);
        }
    }
}