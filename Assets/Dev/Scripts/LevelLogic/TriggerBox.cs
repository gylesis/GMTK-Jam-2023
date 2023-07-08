using UniRx;
using UnityEngine;

namespace Dev.Scripts
{
    public class TriggerBox : MonoBehaviour
    {
        public Subject<Collider> TriggerEntered { get; } = new Subject<Collider>();

        protected virtual void OnTriggerEnter(Collider other)
        {
            TriggerEntered.OnNext(other);
        }
    }
}