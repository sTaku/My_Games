using UnityEngine;
using System.Collections;

/// <summary>
/// 空薬莢(ハンドガン)クラス
/// </summary>
public class karaScr : MonoBehaviour {

    private float dirY,dirZ;

    //追加-------------------------------------------
    private float angleZ;

	// Use this for initialization
	void Start () {
        dirY = Random.Range(-0.01f, 0.1f);
        dirZ = Random.Range(-0.01f, -0.1f);

        //追加-----------------------------------------------
        angleZ = Random.Range(-0.1f, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.Rotate(0.0f, 0.0f, 90.0f);
        var rote = this.gameObject.transform.localRotation;
        rote.Set(
            rote.x,
            rote.y,
            rote.z + angleZ,
            rote.w
        );
        this.gameObject.transform.localRotation = rote;

        this.transform.Translate(0.0f, dirY, dirZ);
        Destroy(gameObject, 1f);
	}
}
