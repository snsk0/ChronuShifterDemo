using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMSingleton : MonoBehaviour
{
    float BGMvolume;
    public static BGMSingleton instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("BGMList")]
    [SerializeField] AudioSource Title;
    [SerializeField] AudioSource GameBGM1;
    private string beforeScene;
    // Start is called before the first frame update
    void Start()
    {
        beforeScene = "SKR_TestTitleUI";
        Title.Play();

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioSource GetAudioSource() => Title;

    void OnActiveSceneChanged(Scene preceScene,Scene nextScene)
    {
        if(beforeScene == "StageSelect" && nextScene.name != "SKR_TestTitleUI")
        {
            Title.Stop();
            //BGMvolume = ;//�X���C�_�[�̒l����
            GameBGM1.volume = BGMSlider.GetSliderVolume();
            GameBGM1.Play();
        }
        else if(beforeScene != "SKR_TestTitleUI" && nextScene.name == "StageSelect")
        {
            GameBGM1.Stop();
            Title.Play();
        }

        beforeScene = nextScene.name;
    }
}
