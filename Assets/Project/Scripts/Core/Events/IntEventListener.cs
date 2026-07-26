using UnityEngine;
using UnityEngine.Events;

namespace Core.Events
{
    /// <summary>
    /// UnityEvent&lt;int&gt; can't be dragged/configured in the Inspector unless we make a
    /// concrete, [Serializable] subclass like this one. This is the small bit of "plumbing"
    /// that lets an int payload show up as a real field in the Inspector's Response list -
    /// you don't need to touch this class, just know it exists.
    /// </summary>
    [System.Serializable]
    public class IntUnityEvent : UnityEvent<int> { }

    /// <summary>
    /// See VoidEventListener.cs for full usage notes - identical pattern, typed for int.
    /// Whatever method you hook up in "Response" (Inspector) will receive the int payload.
    /// </summary>
    public class IntEventListener : MonoBehaviour
    {
        [SerializeField] private IntEventChannelSO channel;
        [SerializeField] private IntUnityEvent response;

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

        private void Respond(int value)
        {
            response?.Invoke(value);
        }
    }
}
