using Chronus.StageSelect;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chronus.Scenes
{
    public static class SceneLoader
    {
        private static readonly string LoadingSceneName = "LoadingScene";
        private static bool IsLoading = false;

        public static async UniTaskVoid LoadSceneAsync(StageType stageType)
        {
            LoadSceneAsync("Stage0" + stageType.ToString("D")).Forget();
        }

        public static async UniTaskVoid LoadSceneAsync(string sceneName)
        {
            if (IsLoading)
                return;

            SceneManager.LoadScene(LoadingSceneName, LoadSceneMode.Additive);

            IsLoading = true;
            //必ず表示させるためにロードの開始を遅らせる
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            await SceneManager.LoadSceneAsync(sceneName);
            IsLoading = false;
        }

        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(LoadingSceneName, LoadSceneMode.Additive);
            SceneManager.LoadScene(sceneName);
            SceneManager.UnloadSceneAsync(LoadingSceneName);
        }

    }
}

