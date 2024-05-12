using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Chronus.LegacyChronuShift
{
    public class Goal : MonoBehaviour
    {
        // 仮実装
        // ゴール演出Timelineを再生
        // シーン遷移をTimelineから呼び出し
        [SerializeField] string nextSceneName;
        [SerializeField] PlayableDirector director;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                director.Play();
            }
        }

        public void GoNextScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}