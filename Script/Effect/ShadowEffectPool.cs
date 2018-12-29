using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ShadowEffectPool : MonoBehaviour
{
    private float alphaValue = 150;
    void Start()
    {
        
    }
 
    private void OnEnable()
    {
        GetComponent<MeshRenderer>().material.shader = Shader.Find("First Fantasy/Water/Water Diffuse");
        GetComponent<MeshRenderer>().material.SetFloat("_IsPlay", 1);
        GetComponent<MeshRenderer>().material.SetColor("_MainTexColor", new Color(1, 0, 0, alphaValue / 255));
        GetComponent<MeshRenderer>().material.SetFloat("_MainTexMultiply", 3.21f);
        gameObject.layer = LayerMask.NameToLayer("Temp");
        alphaValue = 150;
        timer = 0;
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>("Mask"));
    }
 
    private float timer, rate = 0.50f;
    void Update()
    {
        timer += Time.deltaTime;
        alphaValue -= Time.deltaTime * 300;
        GetComponent<MeshRenderer>().material.SetColor("_MainTexColor", new Color(1, 0, 0,  alphaValue/ 255));
        if (timer > rate)
        {
            GetComponent<MeshRenderer>().material.SetFloat("_MainTexMultiply", 0);
            GetComponent<MeshRenderer>().material.SetColor("_MainTexColor", new Color(0, 0, 0, 0));
            gameObject.SetActive(false);
            ObjPool.Instance.Add("SkinMesh",this.gameObject);
            return;
        }
    }
}