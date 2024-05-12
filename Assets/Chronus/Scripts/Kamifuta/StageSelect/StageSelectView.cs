using Chronus.Scenes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Chronus.StageSelect
{
    public class StageSelectView : MonoBehaviour
    {
        [Serializable]
        private sealed class LoadStage
        {
            public StageType stageType;
            public StageLoadButton button;
            public Sprite backGroundSprite;
        }

        [SerializeField] private LoadStage[] loadStages;
        [SerializeField] private EventSystem eventSystem;

        [SerializeField] private Image backGroundImage;

        private void Start()
        {
            foreach(var loadStage in loadStages)
            {
                loadStage.button.interactable = UnlockedStageManager.UnlockedStages.Any(x => x == loadStage.stageType);
                loadStage.button.onClick.AddListener(() => SceneLoader.LoadSceneAsync(loadStage.stageType).Forget());

                loadStage.button.onPointerEnterCallback = () => SetSelectedButton(loadStage.button);
            }

            this.ObserveEveryValueChanged(x => x.eventSystem.currentSelectedGameObject)
                .Where(x => x != null)
                .Subscribe(obj =>
                {
                    var loadStage = loadStages.FirstOrDefault(button => button.button.gameObject == obj);
                    if (loadStage == null)
                        return;

                    backGroundImage.sprite = loadStage.backGroundSprite;
                })
                .AddTo(this);

            eventSystem.SetSelectedGameObject(loadStages.First(x => x.button.interactable).button.gameObject);
        }

        private void SetSelectedButton(Button button)
        {
            if (!button.interactable)
                return;

            eventSystem.SetSelectedGameObject(button.gameObject);
        }
    }
}

