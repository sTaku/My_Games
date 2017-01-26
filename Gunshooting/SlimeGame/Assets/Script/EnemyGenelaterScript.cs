using UnityEngine;
using System.Collections;

/// <summary>
/// 敵のジェネレータクラス
/// </summary>

public class EnemyGenelaterScript : MonoBehaviour {

    [SerializeField]
    private int hp  = 2500;                 //ジェネレーターのHP
    private const int genelatcount = 5;       //生成回数
    private int[] EnemyCount;
    private float count = 0;
    [SerializeField]
    private float countTime;
    private const float genelateTime = 5;     //生成までの時間
    [SerializeField]
    private GameObject enemyobj;
    private GameObject enemy;
    private bool IsSleep = true;
    [SerializeField]
    private float rote;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (!IsSleep)
        {
            EnemyGenelate();
        }
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    private void EnemyGenelate()
    {
        if (count < genelatcount)
        {
            countTime += Time.deltaTime;

            if (countTime >= genelateTime)
            {
                countTime = 0.0f;
                Vector3 genelatePosition = new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z + Random.Range(-3, 3));
                enemy = (GameObject)Instantiate(enemyobj, genelatePosition, Quaternion.Euler(0, rote, 0));
                enemy.GetComponent<EnemyMove>().endPos.x = Random.Range(-1,2);
                enemy.transform.parent = transform;
                childWakeup();
                count++;
            }
        }
    }

    private void childWakeup()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<EnemyScript>().Wakeup();
        }
    }

    public void Wakeup()
    {
        IsSleep = false;
        transform.tag = "Enemy";
    }

    //Enemyが死ぬごとに-1
    public void damage()
    {
        hp-=500;
        if (hp <= 0)
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            if(parent.name == "BattleBox")
            {
                parent.GetComponent<BattleBoxScript>().Count();
            }
            Destroy(this.gameObject);
        }
    }
}
