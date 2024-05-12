using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Chronus.OrbGimmick;
using Chronus.StageSelect;
using Chronus.Scenes;

namespace Chronus.ChronusGimmick
{
    public class StageClearJudge : MonoBehaviour
    {
        [SerializeField] private List<OrbSwitch> _switchList;
        [SerializeField] private StageType _currentStageType;

        private async void Awake()
        {
            await UniTask.WaitUntil(() => _switchList.All(value => value.isOrbExist));
            Clear();
        }

        private void Clear()
        {
            if(_currentStageType != StageType.Stage4)
            {
                UnlockedStageManager.Unlock(_currentStageType + 1);
                SceneLoader.LoadSceneAsync(_currentStageType + 1).Forget();
            }
            else
            {
                SceneLoader.LoadSceneAsync("StageSelect").Forget();
            }
        }
    }
}
