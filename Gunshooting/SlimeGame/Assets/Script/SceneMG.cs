using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// シーンを管理する
/// シーン遷移と同時にFadeIn、FadeOut等のアクションを行う、コールバックを登録できる
/// </summary>
public class SceneMG : MonoBehaviour {

    //デリゲート（関数ポインタ）
    public delegate void Callback();
    Callback m_callbacks;

    Image m_image;
    GameObject m_obj;
    [SerializeField] public static bool f_isfade;

    [SerializeField] float m_fadeinTime = 1.5f;
    [SerializeField] float m_fadeoutTime = 1.5f;

    string m_currentSceneName; //現在のシーン名
    string m_previousSceneName; //一つ前のシーン名

    protected static SceneMG instance;
    public static SceneMG Instance {
        get {
            if (instance == null) {
                instance = (SceneMG)FindObjectOfType(typeof(SceneMG));
                if (instance == null) {
                    Debug.LogError("SceneManager Instance Error");
                }
            }
            return instance;
        }
    }

    void Awake() {

        GameObject[] obj = GameObject.FindGameObjectsWithTag("SceneManage");
        if (obj.Length > 1)
        {
            //既に存在しているなら削除
            Destroy(gameObject);
        }
        else
        {
            //シーン遷移では破棄させない
            DontDestroyOnLoad(this.gameObject);
        }

        m_currentSceneName = SceneManager.GetActiveScene().name;
        m_previousSceneName = "Course01";

        //マウスカーソルを表示しないようにする
        //Cursor.visible = false;
    }

    void Start() {
        m_obj = transform.GetChild(0).gameObject;
        m_image = m_obj.GetComponent<Image>();
        m_callbacks = null;
       
    }

    /// <summary>
    /// 一つ前のシーン名を取得する
    /// </summary>
    /// <returns></returns>
    public string GetPreviousSceneName()
    {
        return m_previousSceneName;
    }
    /// <summary>
    /// 現在のシーン名を取得する
    /// </summary>
    /// <returns></returns>
    public string GetCurrentSceneName()
    {
        return m_currentSceneName;
    }

    /// <summary>
    /// 第二引数をTrueにする フェードイン後にシーン遷移してフェードアウト
    /// 第二引数をFalseにする　フェードイン後にシーン遷移のみ
    /// </summary>
    /// <param name="NexSceneName"></param>
    /// <param name="AutoFadeOut"></param>
    public void FadeSceneChange(string NexSceneName, bool AutoFadeOut = true) {
        if (f_isfade) return;
        FadeIn(m_obj, m_image, AutoFadeOut, NexSceneName);
        m_previousSceneName = m_currentSceneName;
        m_currentSceneName = NexSceneName;
        f_isfade = true;
    }
    /// <summary>
    /// フェードアウト単体で呼び出す、コールバック（関数）を登録できる
    /// 登録したコールバック（関数）はフェード終了時に呼び出される
    /// </summary>
    /// <param name="callmethod"></param>
    public void SceneFadeOut(Callback callmethod = null, bool flyng = false)
    {
        if (f_isfade) return;
        //コールバック登録
        m_callbacks += callmethod;
        FadeOut(m_obj, m_image);
        if (flyng) {
            m_callbacks();
            m_callbacks = null;
        }
        f_isfade = true;
    }

    //キャンバスイメージのフェードイン処理
    void FadeIn(GameObject target, Image img, bool nextFadeOut, string nextSceneName)
    {
        string InOut = "";
        if (nextFadeOut)
        {
            InOut = "InOut";
        }

        iTween.ValueTo(target, iTween.Hash(
            "from", img.color.a,
            "to", 1,
            "time", m_fadeinTime,
            "onstart", "TestStart",
            "onstarttarget", this.gameObject,
            "onupdate", "UpdateAlpha",
            "onupdatetarget", this.gameObject,
            "oncomplete", "OnComplete" + InOut,
            "oncompleteparams", "" + nextSceneName,
            "oncompletetarget", this.gameObject));
    }
    //キャンバスイメージのフェードアウト処理
    void FadeOut(GameObject target, Image img)
    {
        iTween.ValueTo(target, iTween.Hash(
            "delay", 0.5f,
            "from", img.color.a,
            "to", 0,
            "time", m_fadeoutTime,
            "onupdate", "UpdateAlpha",
            "onupdatetarget", this.gameObject,
            "oncomplete", "OutComplete",
            "oncompleteparams", "testMessage",
            "oncompletetarget", this.gameObject));
    }

    //フェードイン終了時に呼び出される
    void OnCompleteInOut(string message)
    {
        SceneManager.LoadScene(message);
        FadeOut(m_obj, m_image);
    }

    //boolをfalseにするとこちらが呼ばれる、自動的にFadeOutしない
    void OnComplete(string message)
    {
        f_isfade = false;
        SceneManager.LoadScene(message);
        
    }
    
    /// <summary>
    /// フェードアウト終了時に呼び出す、コールバックが登録されていたら呼び出す
    /// </summary>
    /// <param name="message"></param>
    void OutComplete(string message)
    {
        f_isfade = false;
        if (m_callbacks != null) {
            m_callbacks();
            m_callbacks = null;
        }

    }

    //アルファ値を変更する
    void UpdateAlpha(float val)
    {
        m_image.color = new Color(0, 0, 0, val);
    }
}
