using Chronus.Direction;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Chronus.ChronuShift
{
    public class ChronusStateManager : MonoBehaviour
    {
        private static ChronusStateManager instance;
        public static ChronusStateManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<ChronusStateManager>();
                }

                return instance;
            }
        }

        [SerializeField] private ChronusState initialState = ChronusState.Current;
        public ReactiveProperty<ChronusState> chronusState { private set; get; } = new ReactiveProperty<ChronusState>();

        [SerializeField] private float _shiftDuration = 2;
        public float shiftDuration => _shiftDuration;

        private CancellationToken token;

        private void Awake()
        {
            if(this != Instance)
            {
                Destroy(this);
            }

            Instance.chronusState.Value = initialState;
        }

        // ëJà⁄íÜÇ≈Ç†ÇÍÇŒbool,ëJà⁄íÜÇ≈Ç»ÇØÇÍÇŒéûä‘ÇêÿÇËë÷Ç¶ÇƒtrueÇï‘Ç∑
        public bool ChronuShift()
        {
            if(token == null)
            {
                token = this.GetCancellationTokenOnDestroy();
            }

            if(Instance.chronusState.Value == ChronusState.Past)
            {
                StartShift(ChronusState.Forward, token).Forget();
            }
            else if(Instance.chronusState.Value == ChronusState.Current)
            {
                StartShift(ChronusState.Backward, token).Forget();
            }
            else return false;

            return true;
        }

        public bool ChronuShift(ChronusState state)
        {
            if (token == null)
            {
                token = this.GetCancellationTokenOnDestroy();
            }

            if (state == ChronusState.Past)
            {
                StartShift(ChronusState.Backward, token).Forget();
            }
            else if (state == ChronusState.Current)
            {
                StartShift(ChronusState.Forward, token).Forget();
            }
            else return false;

            return true;
        }

        // ëJà⁄íÜÇ©î€Ç©Çï‘Ç∑
        public bool isShifting()
        {
            if(Instance.chronusState.Value == ChronusState.Past || Instance.chronusState.Value == ChronusState.Current)
            {
                return false;
            } else
            {
                return true;
            }
        }

        private async UniTaskVoid StartShift(ChronusState state, CancellationToken token)
        {
            ChronusState nextState = new ChronusState();

            if (state == ChronusState.Forward)
            {
                if (Instance.chronusState.Value == ChronusState.Past)
                {
                    nextState = ChronusState.Current;
                }
            }
            else if (state == ChronusState.Backward)
            {
                if (Instance.chronusState.Value == ChronusState.Current)
                {
                    nextState = ChronusState.Past;
                }
            }
            else
            { return; }

            Instance.chronusState.Value = state;

            await TimelinePlayer.PlayTimelineAll(UnityEngine.Playables.DirectorWrapMode.None);
            //ãå await UniTask.Delay((int)shiftDuration * 1000, cancellationToken: token);

            Instance.chronusState.Value = nextState;
        }
    }
}