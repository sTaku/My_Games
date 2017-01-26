using UnityEngine;
using System.Collections;

/// <summary>
/// 銃(ハンドガン)のクラス
/// </summary>
public class HandGunScript : MonoBehaviour
{

    private PlayerScript playerScript;
    private GameObject player;

    //ハンドガン弾描画用オブジェクト
    public GameObject handGunBullet;
    [SerializeField]
    private Transform bulletCamera;

    //カーソル
    [SerializeField]
    private CursolScript cursolScript;

    //弾関連
    private int bulletNum;
    public int maxBulletNum;
    public GameObject Efe_bulletParticle;

    //弾丸描画関連.
    public GameObject bullet_3D;
    private GameObject[] bulletObj_3D = new GameObject[15];
    public GameObject karaObj;

    //リロード関連.
    public float reloadTime;
    private bool reloadFlg = false;
    private float reloadTimer;
    public GameObject reloadAlert;

    [SerializeField]
    private AudioClip audioClip;
    AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        bulletCamera = GameObject.FindGameObjectWithTag("HBCamera").transform;
        playerScript = player.GetComponent<PlayerScript>();
        if (playerScript.getGunType() != PlayerScript.GunType.HandGun)
        {
            transform.localPosition = new Vector3(1.0f, -10.0f, 0.5f);
        }

        cursolScript = Camera.main.GetComponent<CursolScript>();
        
        bulletNum = maxBulletNum;

        reloadTimer = 0.0f;

        //初期化時、最大装填数に合わせて弾画像生成
        for (var i = 0; i < 15; i++)
        {

            Vector3 euler = new Vector3(0.0f, 20.0f, 90.0f);
            bulletObj_3D[i] = (GameObject)Instantiate(bullet_3D,Vector3.zero, Quaternion.Euler(0,0,90));
            bulletObj_3D[i].transform.parent = bulletCamera.transform;
            bulletObj_3D[i].transform.localPosition = new Vector3(2f, 3.4f -0.4f * i, 1.0f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playerScript.getGunType() == PlayerScript.GunType.HandGun && playerScript.IsChangeGun())
        {
            transform.localPosition = new Vector3(1.0f, -0.8f, 0.5f);
        }
        else if (playerScript.getGunType() != PlayerScript.GunType.HandGun && playerScript.IsChangeGun())
        {
            transform.localPosition = new Vector3(1.0f, -5.0f, 0.5f);
        }
    }

    void LateUpdate()
    {
        playReload();
    }

    void Shot()
    {
        //弾がある　又は　リロードをしていない.
        if (bulletNum > 0 && !reloadFlg)
        {
            audioSource.PlayOneShot(audioClip);
            cursolScript.Shotdamage(100);
            Instantiate(Efe_bulletParticle, cursolScript.cursolPosition, Quaternion.Euler(0, 0, 0));
            bulletNum--;
            BulletRem(bulletNum);
        }
        if (bulletNum == 0)
        {
            Vector3 pPos = player.transform.position;
            Instantiate(reloadAlert, new Vector3(pPos.x, pPos.y, pPos.z + 1),Quaternion.Euler(0,0,0));
        }

    }

    // 現在の装填数に合わせて弾オブジェクトを消す.
    public void BulletRem(int nowBulletCount)
    {
        var nowNo = maxBulletNum - nowBulletCount - 1;
        if (nowNo < 0) return;
        Instantiate(karaObj, bulletObj_3D[bulletObj_3D.Length - 1].transform.position, Quaternion.identity);
        Destroy(bulletObj_3D[nowNo]);
    }

    // リロード時、現在の装填数に合わせて弾オブジェクトを生成.
    public void BulletReload(int nowBulletCount)
    {
        var nowNo = 15 - nowBulletCount; 
        Vector3 euler = new Vector3(0.0f, 0.0f, 90.0f);
        bulletObj_3D[nowNo] = (GameObject)Instantiate(bullet_3D, Vector3.zero, Quaternion.Euler(euler));
        bulletObj_3D[nowNo].transform.parent = bulletCamera.transform;
        bulletObj_3D[nowNo].transform.localPosition = new Vector3(2f, 3.4f -0.4f * nowNo, 1.0f);
    }

    void Reload()
    {
        //弾が一発でも減っていたら.
        if (bulletNum < maxBulletNum)
        {
            //リロードフラグON
            reloadFlg = true;
        }
    }

    void playReload()
    {
        if (!reloadFlg) return;

        //リロード時装填可能数に現在の装填数に達していない:毎フレームに1発
        if (bulletNum < maxBulletNum)
        {
            bulletNum++;
            reloadTimer += Time.deltaTime;
            BulletReload(bulletNum);
        }
        //装填可能数に現在の装填数に達している　かつ　リロード時間終了
        else if (reloadTimer >= reloadTime)
        {
            //リロードフラグOFF
            reloadFlg = false;
            //リロード時間を初期化
            reloadTimer = 0.0f;
        }
        //リロード時間中　かつ　装填数MAX
        else
        {
            reloadTimer += Time.deltaTime;
        }

    }
}
