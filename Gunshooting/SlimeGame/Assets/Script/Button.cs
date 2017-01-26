using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//ボタンUIのスクリプト
public class Button : MonoBehaviour {

    [SerializeField]
    private AudioClip audioClip;
    AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }


    public void StartButton()
    {

        audioSource.PlayOneShot(audioClip);
        SceneMG.Instance.FadeSceneChange("GamePlayScene",true);
    }

    public void ExitButton()
    {

        audioSource.PlayOneShot(audioClip);
        Application.Quit();
    }

    public void TitleBackButton()
    {
        audioSource.PlayOneShot(audioClip);
        SceneMG.Instance.FadeSceneChange("Title", true);
    }
}
