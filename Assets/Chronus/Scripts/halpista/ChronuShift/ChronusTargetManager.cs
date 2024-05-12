using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Chronus.Direction;

namespace Chronus.ChronuShift
{
    public class ChronusTargetManager : MonoBehaviour
    {
        List<IChronusTarget> chronusTargets = new List<IChronusTarget>();

        private void Start()
        {
            FindChronusTarget<IChronusTarget>(ref chronusTargets);
            ChronusStateManager.Instance.chronusState.Subscribe(chronusState => OnShift(chronusState)).AddTo(this);
        }

        private void OnShift(ChronusState state)
        { 
            foreach (var target in chronusTargets)
            {
                target.OnShift(state);
            }

            // TimelinePlayer.PlayTimelineAll().Forget();
        }

        private void FindChronusTarget<T>(ref List<T> values) where T : class
        {
            values.Clear();

            foreach(var n in GameObject.FindObjectsOfType<Component>())
            {
                var component = n as T;
                if (component != null) 
                { 
                    values.Add(component);
                }
            }
        }

        public void AddChronusTarget(IChronusTarget target)
        {
            chronusTargets.Add(target);
        }
    }
}