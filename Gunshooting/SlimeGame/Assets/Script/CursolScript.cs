using UnityEngine;
using System.Collections;

/// <summary>
/// カーソル用のスクリプト
/// </summary>
public class CursolScript : MonoBehaviour {

    public Vector3 cursolPosition;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    //着弾地点を
    public void Shotdamage(float bulletdamage)
    {
        //スクリーン座標をrayに変換
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Rayの当たったオブジェクトの情報を格納する
        RaycastHit hit = new RaycastHit();
        // オブジェクトにrayが当たった時
        if (Physics.Raycast(ray, out hit))
        {
            //マウスカーソルのポジション
            cursolPosition = hit.point;
            // rayが当たったオブジェクトのタグを取得
            string objecttag = hit.collider.gameObject.tag;
            if (objecttag == "Enemy")
            {
                var enemyobject = hit.collider.gameObject.GetComponent<EnemyScript>();
                enemyobject.GetDamage(bulletdamage);
            } 
            else if(objecttag == "EnemyBullet")
            {
                var bulletobject = hit.collider.gameObject.GetComponent<EnemyBulletScript>();
                bulletobject.GetDamage(bulletdamage);
            }
        }
    }
}
