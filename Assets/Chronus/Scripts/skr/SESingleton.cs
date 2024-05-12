using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SESingleton : MonoBehaviour
{
    float SEvolume;
    public static SESingleton instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("SEAudioSource")]
    [SerializeField] AudioSource SE;
    [Header("SEClip")]
    [SerializeField] AudioClip SE1;//�N���b�N��
    [SerializeField] AudioClip SE2;//�^�C���V�t�g
    [SerializeField] AudioClip SE3;//��
    private string beforeScene;
    // Start is called before the first frame update
    void Start()
    {
        beforeScene = "SKR_TestTitleUI";

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }
    public AudioSource GetAudioSource() => SE;
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnActiveSceneChanged(Scene preceScene, Scene nextScene)
    {
        if (beforeScene == "SKR_TestTitleUI" && nextScene.name == "MidtermPresentationStage")
        {
            //BGMvolume = ;//�X���C�_�[�̒l����
            SE.volume = SESlider.GetSliderVolume();
        }

        beforeScene = nextScene.name;
    }

    public void ClickSE()
    {
        SE.PlayOneShot(SE1);
    }
    public void TimeShiftSE()
    {
        SE.PlayOneShot(SE2);
    }
    public void DoorSE()
    {
        SE.PlayOneShot(SE3);
    }
}
