using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 視点変更バトル用のスクリプト
/// </summary>
public class BattleBoxScript : MonoBehaviour
{

    [SerializeField]
    private Image RightButton;
    [SerializeField]
    private Image LeftButton;
    private int objectCount = 3;
    private GameObject player;
    public GameObject[] WakeUpEnemys;


    // Use this for initialization
    void Start()
    {
        RightButton.gameObject.SetActive(false);
        LeftButton.gameObject.SetActive(false);
        this.GetComponent<Renderer>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Count()
    {
        objectCount--;
        if(objectCount == 0)
        {
            player.GetComponent<PlayerMoveScript>().setIsGOGOGO(true);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //ボタン表示
        if (collider.gameObject.tag == "Player")
        {
            collider.GetComponent<PlayerMoveScript>().setIsGOGOGO(false);
            RightButton.gameObject.SetActive(true);
            LeftButton.gameObject.SetActive(true);
            RightButton.GetComponent<RotateButton>().PlayerChack(collider);
            LeftButton.GetComponent<RotateButton>().PlayerChack(collider);
            for (int i = 0; i < WakeUpEnemys.Length; i++)
            {
                WakeUpEnemys[i].GetComponent<EnemyGenelaterScript>().Wakeup();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            RightButton.gameObject.SetActive(false);
            LeftButton.gameObject.SetActive(false);
            player.GetComponent<PlayerScript>().MouseCheck(false);
        }
    }
}
