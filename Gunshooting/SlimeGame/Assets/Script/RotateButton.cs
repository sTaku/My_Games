using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// プレイヤーが回転する用のボタン
/// </summary>
public class RotateButton : MonoBehaviour
{
    //ボタンの状態
    private enum ButtonState
    {
        RightRote,
        LeftRote
    }

    [SerializeField]
    private ButtonState buttonState;
    private Vector3 degree;         //プレイヤーを回転させる数値
    [SerializeField]
    private float time;           //回転するまでの時間
    private GameObject player;
    [SerializeField]
    private bool cooltime;        //クールタイムフラグ　オフの時にしか回転できない

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlayerChack(Collider col)
    {
        player = col.gameObject;
    }

    //ボタンをクリックした時.に視点を変更する
    public void GetButtonDown()
    {
        if (cooltime == false)
        {
            switch (buttonState)
            {
                case ButtonState.RightRote:
                    if (player.GetComponent<PlayerMoveScript>().count < 1)
                    {
                        degree = new Vector3(0, 90, 0);
                        StartCoroutine(PlayerRote(1, degree));
                        player.GetComponent<PlayerMoveScript>().count++;
                    }
                    break;
                case ButtonState.LeftRote:
                    if (player.GetComponent<PlayerMoveScript>().count > -1)
                    {
                        degree = new Vector3(0, -90, 0);
                        StartCoroutine(PlayerRote(1, degree));
                        player.GetComponent<PlayerMoveScript>().count--;
                    }
                    break;
            }
        }
    }
    public IEnumerator PlayerRote(float endtime, Vector3 rotate)
    {
        cooltime = true;
        time = 0;
        while (time < endtime)
        {
            yield return null;
            time += Time.deltaTime;
            player.transform.Rotate(rotate * Time.deltaTime);
        }
        cooltime = false;
    }
}
