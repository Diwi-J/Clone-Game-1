using UnityEngine;
using UnityEngine.Events;

namespace Core.Events
{
    /// <summary>
    /// Drop this on any GameObject to react to a VoidEventChannelSO WITHOUT writing code.
    /// Drag the event asset into "Channel", then wire up "Response" in the Inspector exactly
    /// like a Button's OnClick - drag a target object in, pick a public method.
    ///
    /// Automatically subscribes in OnEnable and unsubscribes in OnDisable, so nobody has to
    /// remember to do that manually or worry about memory leaks / double-firing.
    /// </summary>
    public class VoidEventListener : MonoBehaviour
    {
        [Tooltip("The event asset this listener is watching. Must be the same asset whoever raises the event is using.")]
        [SerializeField] private VoidEventChannelSO channel;

        [Tooltip("What should happen when the event fires. Set this up in the Inspector - no code required.")]
        [SerializeField] private UnityEvent response;

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

        private void Respond()
        {
            response?.Invoke();
        }
    }
}
