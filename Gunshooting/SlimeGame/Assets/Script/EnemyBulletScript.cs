using UnityEngine;
using System.Collections;

/// <summary>
/// 敵の弾のクラス
/// </summary>

public class EnemyBulletScript : MonoBehaviour {

    private float hp = 1;
    public float m_fSpeed;
    private GameObject player;
    private PlayerScript pscript;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        pscript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);
        this.gameObject.transform.Translate(0.0f, 0.0f, m_fSpeed * Time.deltaTime);
        Destroy(this.gameObject, 10f);
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            pscript.Damage(100);
        }
        Destroy(this.gameObject);
    }
    public void GetDamage(float getdamage)
    {
        hp -= getdamage;
    }
}
