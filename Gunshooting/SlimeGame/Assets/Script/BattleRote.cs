using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// パトルポイントでのプレイヤーの視点変更
/// </summary>
public class BattleRote : MonoBehaviour
{
    [SerializeField]
    private Vector3 degree;
    [SerializeField]
    private float time;
    public float rotetime;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRote(time);
    }
    void PlayerRote(float timer)
    {
        if (rotetime < timer)
        {
            rotetime += Time.deltaTime;
            transform.Rotate(degree * Time.deltaTime);
        }
    }
}
