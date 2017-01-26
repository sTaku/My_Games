using UnityEngine;
using System.Collections;

/// <summary>
/// ゴールオブジェクトクラス
/// </summary>
public class GoalScript : MonoBehaviour {

    [SerializeField]
    private GameObject missionobj;
    [SerializeField]
    private GameObject completeobj;
    [SerializeField]
    private GameObject titlebackbutton;
    // Use this for initialization
    void Start () {
        this.GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        titlebackbutton.SetActive(true);
        missionobj.GetComponent<ClearEffect>().Clear();
        completeobj.GetComponent<ClearEffect>().Clear();
    }
}
