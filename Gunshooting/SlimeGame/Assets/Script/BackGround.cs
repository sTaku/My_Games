using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour {

    public Vector2 m_ScrollSpeed;

    public Material m_Material;

    // Use this for initialization
    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        m_Material.SetTextureOffset("_MainTex", m_ScrollSpeed * Time.time);
    }
}
