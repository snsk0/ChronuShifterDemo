using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chronus.Utils
{
    public static class MouseCusorController
    {
        private static readonly List<string> CursolDisableSceneNames = new List<string>()
        {
            "CharacterDemo",
            "Stage01",
            "Stage02",
            "Stage03",
            "Stage04"
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene nextScene, LoadSceneMode mode)
        {
            SetCursolActive(!CursolDisableSceneNames.Contains(nextScene.name));
        }

        public static void SetCursolActive(bool active)
        {
            Cursor.visible = active;
            
            if(active)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
