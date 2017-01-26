using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


/// <summary>
/// Battle時のマウスポインタの確認
/// </summary>
[RequireComponent(typeof(Image))]

public class MouseCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image { get { return GetComponent<Image>(); } }

    private GameObject player;


    // オブジェクトの範囲内にマウスポインタが入った際に呼び出されまる
    public void OnPointerEnter(PointerEventData eventData)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //trueの時は撃てないように
        player.GetComponent<PlayerScript>().MouseCheck(true);
    }

    // オブジェクトの範囲内からマウスポインタが出た際に呼び出される
    public void OnPointerExit(PointerEventData eventData)
    {
        //falseのときは撃てるように
        player.GetComponent<PlayerScript>().MouseCheck(false);
    }
}