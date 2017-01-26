using UnityEngine;
using System.Collections;

/// <summary>
/// 銃(マシンガン)クラス
/// </summary>
public class MachineGunScript : MonoBehaviour
{
    //ガン切り替え用
    private PlayerScript playerScr;

    private GameObject player;

    //マシンガン弾描画用オブジェクト
    [SerializeField]
    private GameObject machineGunBullet;

    [SerializeField]
    private Transform bulletCamera;
    //カーソルオブジェクト
    //private GameObject cursolObject;
    [SerializeField]
    private CursolScript cursolScript;

    //弾関連
    private int bulletNum; //現在の装填数
    public int maxBulletNum; //装填可能弾数
    public float shotWait;
    private float shotWaitTimer;
    public int nowRemenberBullet; //現在所持弾数
    public int maxRemenberBullet; //最大所持弾数
    private int bulletCount = 0;
    public GameObject Efe_bulletParticle;

    //弾丸描画関連
    public GameObject bullet;
    private GameObject[] bulletObj;
    public GameObject bullet_3D;
    private GameObject[] bulletObj_3D;
    public GameObject karaObj;

    //リロード関連.
    public float reloadTime;
    private bool reloadFlg = false;
    private float reloadTimer;
    private Vector3 gunRote;
    public GameObject reloadAlert;

    [SerializeField]
    private AudioClip audioClip;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    { 
        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        playerScr = player.GetComponent<PlayerScript>();
        bulletCamera = GameObject.FindGameObjectWithTag("MBCamera").transform;
        if (playerScr.getGunType() != PlayerScript.GunType.MachinGun)
        {
            transform.localPosition = new Vector3(1.0f, -10.0f, 0.3f);
        }

        cursolScript = Camera.main.GetComponent<CursolScript>();

        bulletNum = maxBulletNum;
        shotWaitTimer = 0.0f;

        reloadTimer = 0.0f;

        bulletObj = new GameObject[maxBulletNum];
        bulletObj_3D = new GameObject[maxBulletNum];
        //初期化時、最大装填数に合わせて弾画像生成
        for (var i = 0; i < maxBulletNum; i++)
        {
            Vector3 euler = new Vector3(0.0f,30.0f,90.0f);
            bulletObj_3D[i] = (GameObject)Instantiate(bullet_3D, Vector3.zero, Quaternion.Euler(0,0,90));
            bulletObj_3D[i].transform.parent = bulletCamera.transform;
            bulletObj_3D[i].transform.localPosition = new Vector3((float)(4f - (i / 30 * 1f)), (float)(-0.5f + 0.2f * (i % 30) + (i / 30 * 0.2)), (float)(3.0f +(i / 30 * 0.5)));
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playerScr.getGunType() == PlayerScript.GunType.MachinGun && playerScr.IsChangeGun())
        {
            transform.localPosition = new Vector3(1.0f, -1.5f, 0.3f);
        }
        else if (playerScr.getGunType() != PlayerScript.GunType.MachinGun && playerScr.IsChangeGun())
        {
            transform.localPosition = new Vector3(1.0f, -5.0f, 0.3f);
        }
    }

    void LateUpdate()
    {
        playReload();
    }

    void Shot()
    {
        shotWaitTimer += Time.deltaTime;
        //弾がある　又は　リロードをしていない.
        if (bulletNum > 0 && !reloadFlg && shotWaitTimer >= shotWait)
        {
            audioSource.PlayOneShot(audioClip);
            cursolScript.Shotdamage(50);
            Instantiate(Efe_bulletParticle, cursolScript.cursolPosition, Quaternion.Euler(0, 0, 0));
            bulletNum--;
            BulletRem(bulletNum);
            shotWaitTimer = 0.0f;
        }
        if (bulletNum == 0)
        {
            Instantiate(reloadAlert);
        }
    }

    // 現在の装填数に合わせて弾画像を消す.
    public void BulletRem(int nowBulletCount)
    {
        var nowNo = maxBulletNum - nowBulletCount - 1;
        if (nowNo < 0) return;
        Instantiate(karaObj, bulletObj_3D[bulletObj_3D.Length - 1].transform.position, Quaternion.identity);
        Destroy(bulletObj_3D[nowNo]);
    }

    // リロード時、現在の装填数に合わせて弾画像を生成.
    public void BulletReload(int nowBulletCount)
    {
        var nowNo = maxBulletNum - nowBulletCount;

        Vector3 euler = new Vector3(0.0f, -5.0f, 90.0f);
        bulletObj_3D[nowNo] = (GameObject)Instantiate(bullet_3D, Vector3.zero, Quaternion.Euler(euler));
        bulletObj_3D[nowNo].transform.parent = bulletCamera.transform;
        bulletObj_3D[nowNo].transform.localPosition = new Vector3((float)(4 - (nowNo / 30 * 1f)), (float)(-0.5f + 0.2f * (nowNo % 30) + (nowNo / 30 * 0.2)), (float)(3.0f + (nowNo / 30 * 0.5)));
    }

    void Reload()
    {
        //弾が一発でも減っていたら.
        if (bulletNum < maxBulletNum && nowRemenberBullet > 0)
        {
            //リロードフラグON
            reloadFlg = true;
            bulletCount = nowRemenberBullet;
        }
    }

    void playReload()
    {
        if (!reloadFlg) return;

        //リロード時装填可能数に現在の装填数に達していない:毎フレームに1発
        if (bulletNum < maxBulletNum && bulletCount > 0)
        {
            bulletNum++;
            bulletCount--;
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
            nowRemenberBullet = bulletCount;
        }
        //リロード時間中　かつ　装填数MAX
        else
        {
            reloadTimer += Time.deltaTime;
        }

        //Quaternion gunQuant = transform.rotation;
        //if (reloadTimer <= 0.5f)
        //{
        //    gunRote = Vector3.Lerp(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.2f, 0.0f, 0.0f), reloadTimer * 2);
        //    gunQuant.Set(
        //        gunQuant.x + gunRote.x,
        //        gunQuant.y,
        //        gunQuant.z,
        //        gunQuant.w
        //    );
        //    this.gameObject.transform.localRotation = gunQuant;
        //}
        //else if (reloadTimer <= 2.0f)
        //{
        //    gunQuant.Set(
        //        gunQuant.x + 1.2f,
        //        gunQuant.y,
        //        gunQuant.z,
        //        gunQuant.w
        //    );
        //    this.gameObject.transform.localRotation = gunQuant;
        //}
        //else
        //{
        //    gunRote = Vector3.Lerp(new Vector3(0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), (reloadTimer - 2.0f) * 2);
        //    gunQuant.Set(
        //        gunQuant.x + gunRote.x,
        //        gunQuant.y,
        //        gunQuant.z,
        //        gunQuant.w
        //    );
        //    this.gameObject.transform.localRotation = gunQuant;
        //}
    }
}
