using UnityEngine;
using System.Collections;

/// <summary>
/// 敵のスクリプト
/// </summary>
public class EnemyScript : MonoBehaviour {

    private GameObject Player;
    private GameObject my;
    [SerializeField]
    private GameObject enemybullet;
    private GameObject parent;
    public float hp;
    public float maxhp;
    private float damagetime = 1f;//アニメーションが終わる時間
    private float walkEndtime = 0f;
    private float atime;
    public float attackTime; //攻撃するまでの時間.
    public float attackPower = 150;  //プレイヤーにどれだけのダメージを与えられるか.

    private bool IsSleep = true;

    private EnemyMove eMove;
    private EnemyAnimation eAnime;
    private BoxCollider boxCol;


	// Use this for initialization
	void Start ()
    {
        hp = maxhp;
        atime = attackTime;
        Player = GameObject.FindGameObjectWithTag("Player");
        eAnime = GetComponent<EnemyAnimation>();
        eMove = GetComponent<EnemyMove>();
        boxCol = GetComponent<BoxCollider>();
        parent = gameObject.transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        EnemyMove();

	}

    void EnemyMove()
    {
        if (!IsSleep)
        {
            if(enemybullet == null)
            {
                Attack();
            }
            else if (enemybullet == true)
            {
                BulletAttack();
            }
            walkEndtime += Time.deltaTime;
            if(walkEndtime >= eMove.mToa)
            {
                eAnime.IdleAnim();
            }
            if (hp <= 0)
            {
                hp = 0;
                StartCoroutine("IsDead");
            }
        }
    }

    /// <summary>
    /// 敵が起動するときに呼び出される
    /// </summary>
    public void Wakeup()
    {
        IsSleep = false;
        transform.tag = "Enemy";
    }


    /// <summary>
    /// 敵が起きているか
    /// </summary>
    /// <returns></returns>
    public bool IsWakeUp()
    {
        return !IsSleep;
    }

    /// <summary>
    /// 弾を持っていない敵の攻撃
    /// </summary>
    void Attack()
    {
        attackTime -= Time.deltaTime;

        if (attackTime <= 0)
        {
            eAnime.AttackAnim();
            Invoke("Attackdamage", damagetime);
            attackTime = atime;
        }
    }

    /// <summary>
    /// 弾を持っている敵の攻撃
    /// </summary>
    void BulletAttack()
    {
        attackTime -= Time.deltaTime;

        if (attackTime <= 0)
        {
            eAnime.AttackAnim();
            Instantiate(enemybullet, new Vector3(this.transform.position.x, this.transform.position.y + 3.0f , this.transform.position.z), Quaternion.Euler(0, 0, 0));
            attackTime = atime;
        }
    }
    /// <summary>
    /// ダメージを与える
    /// </summary>
    void Attackdamage()
    {
        Player.GetComponent<PlayerScript>().Damage(attackPower);
    }
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="getdamage"></param>
    public void GetDamage(float getdamage)
    {
        hp -= getdamage;
        eAnime.GethitAnim();
    }

    /// <summary>
    /// 死亡クラス
    /// </summary>
    /// <returns></returns>
    public IEnumerator IsDead()
    {
        IsSleep = true;

        boxCol.enabled = false;
        eAnime.IsDeadAnim();
        this.transform.tag = "Untagged";
        yield return new WaitForSeconds(3);
        if(parent.gameObject.name == "EnemyGenerater")
        {
            parent.GetComponent<EnemyGenelaterScript>().damage();
        }
        Destroy(gameObject);
    }
}
