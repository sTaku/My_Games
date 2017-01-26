using UnityEngine;
using System.Collections;

public class ClearEffect : MonoBehaviour {

    private RectTransform m_RectTransform;

    void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clear()
    {
        LeanTween.scale(m_RectTransform, new Vector3(4f, 3f, 4f), 0.8f)
                   .setDelay(1)
                   .setEase(LeanTweenType.easeInOutExpo)
                   .setOnComplete(() => {
                       LeanTween.scale(m_RectTransform, new Vector3(2f, 1.5f, 2f), 1.5f).setDelay(0.5f)
                       .setLoopType(LeanTweenType.pingPong);
                   });
    }
}
