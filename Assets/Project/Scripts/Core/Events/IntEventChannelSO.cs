using System;
using UnityEngine;

namespace Core.Events
{
    /// <summary>
    /// Same idea as VoidEventChannelSO, but carries a single int payload.
    /// Use for things like: OnMoneyChanged(newTotal), OnDayNumberChanged(dayNumber).
    /// See VoidEventChannelSO.cs for full usage instructions - identical workflow, just typed.
    ///
    /// Create via: Project window -> Create -> Events -> Int Event Channel
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Int Event Channel", fileName = "New Int Event Channel")]
    public class IntEventChannelSO : ScriptableObject
    {
        public event Action<int> OnEventRaised;

        public void Raise(int value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
