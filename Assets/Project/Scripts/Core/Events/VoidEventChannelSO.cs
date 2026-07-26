using System;
using UnityEngine;

namespace Core.Events
{
    /// <summary>
    /// An event that carries NO data - just "this thing happened".
    /// Examples: OnQueueEmpty, OnDayStarted, OnGameOver.
    ///
    /// WHAT THIS PATTERN IS (for context):
    /// This is a "ScriptableObject Event Channel". Instead of systems calling each other
    /// directly, they share a reference to an ASSET (this SO) - one side calls Raise() on
    /// it, the other side subscribes to it. Neither side needs to know the other exists.
    /// This is why the Verification, Queue, Economy, and Dialogue systems can all be built
    /// and tested independently without waiting on each other's code to exist.
    ///
    /// HOW TO CREATE ONE (in the Unity Editor, not in code):
    ///   Right-click in the Project window -> Create -> Events -> Void Event Channel
    ///   Name it after what happened, e.g. "OnQueueEmpty".
    ///   Store event assets in Assets/_Project/Events/ so they're easy to find.
    ///
    /// HOW TO RAISE IT (from your script):
    ///   [SerializeField] private VoidEventChannelSO onQueueEmpty;
    ///   ...
    ///   onQueueEmpty.Raise();
    ///   (Then drag the actual asset into that field in the Inspector.)
    ///
    /// HOW TO LISTEN WITHOUT WRITING CODE:
    ///   Add a "Void Event Listener" component (see VoidEventListener.cs) to any GameObject,
    ///   drag the SAME asset into its Channel field, and wire a response using the UnityEvent
    ///   in the Inspector - e.g. call a UI method to show "Day Complete". No code required.
    ///
    /// HOW TO LISTEN FROM CODE:
    ///   private void OnEnable()  { onQueueEmpty.OnEventRaised += HandleQueueEmpty; }
    ///   private void OnDisable() { onQueueEmpty.OnEventRaised -= HandleQueueEmpty; }
    ///   (Always unsubscribe in OnDisable - forgetting this is the #1 cause of weird bugs
    ///   with this pattern.)
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Void Event Channel", fileName = "New Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        /// <summary>C# code subscribes here directly.</summary>
        public event Action OnEventRaised;

        public void Raise()
        {
            OnEventRaised?.Invoke();
        }
    }
}
