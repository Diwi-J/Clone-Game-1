using System;
using UnityEngine;

namespace Core.Events
{
    /// <summary>
    /// Same idea as VoidEventChannelSO, but carries a single string payload.
    /// Likely useful for the Dialogue System (e.g. OnDialogueLineChanged) or UI messages
    /// (e.g. OnBorderMessageShown). See VoidEventChannelSO.cs for full usage instructions.
    ///
    /// Create via: Project window -> Create -> Events -> String Event Channel
    /// </summary>
    [CreateAssetMenu(menuName = "Events/String Event Channel", fileName = "New String Event Channel")]
    public class StringEventChannelSO : ScriptableObject
    {
        public event Action<string> OnEventRaised;

        public void Raise(string value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
