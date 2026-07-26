using UnityEngine;
using UnityEngine.Events;

namespace Core.Events
{
    /// <summary>
    /// See IntEventListener.cs's comment on IntUnityEvent - same reasoning applies here.
    /// This concrete subclass is what lets a string payload appear in the Inspector.
    /// </summary>
    [System.Serializable]
    public class StringUnityEvent : UnityEvent<string> { }

    /// <summary>
    /// See VoidEventListener.cs for full usage notes - identical pattern, typed for string.
    /// </summary>
    public class StringEventListener : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO channel;
        [SerializeField] private StringUnityEvent response;

        private void OnEnable()
        {
            if (channel != null)
                channel.OnEventRaised += Respond;
        }

        private void OnDisable()
        {
            if (channel != null)
                channel.OnEventRaised -= Respond;
        }

        private void Respond(string value)
        {
            response?.Invoke(value);
        }
    }
}
