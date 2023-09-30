using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Goal : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] PlayableDirector director;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            director.Play();
        }
    }

    public void GoNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}