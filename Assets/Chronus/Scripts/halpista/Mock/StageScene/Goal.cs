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
        // ������
        // �S�[�����oTimeline���Đ�
        // �V�[���J�ڂ�Timeline����Ăяo��
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