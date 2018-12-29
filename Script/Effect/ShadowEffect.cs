using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ShadowEffect : MonoBehaviour
{
 
    private SkinnedMeshRenderer[] skinMesh;
 
    void Start()
    {
        skinMesh = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void Plays()
    {
        for (int i = 0; i < skinMesh.Length; i++)
        {
            Mesh mesh = new Mesh();
            skinMesh[i].BakeMesh(mesh);
            GameObject t = ObjPool.Instance.TackOut("SkinMesh");
            MeshFilter mesh_ = null;
            MeshRenderer meshRenderer = null;
            if (t == null)
            {
                t = new GameObject();
                t.hideFlags = HideFlags.HideAndDontSave;
                mesh_ = t.AddComponent<MeshFilter>();
                meshRenderer = t.AddComponent<MeshRenderer>();
                meshRenderer.material = skinMesh[i].material;
                t.AddComponent<DesT>();
            }
            else
            {
                t.SetActive(true);
                mesh_ = t.GetComponent<MeshFilter>();
            }
            mesh_.mesh = mesh;
            t.transform.position = skinMesh[i].transform.position;
            t.transform.rotation = skinMesh[i].transform.rotation;
        }
    }
}
 
public class ObjPool
{
    private Dictionary<string, List<GameObject>> pool;
    private static ObjPool instance;
    public static ObjPool Instance
    {
        get
        {
            if (instance == null)
                instance = new ObjPool();
            return instance;
        }
    }
    private ObjPool()
    {
        pool = new Dictionary<string, List<GameObject>>();
    }
 
    public void Add(string name, GameObject o)
    {
        if (pool.ContainsKey(name))
        {
            pool[name].Add(o);
            return;
        }
        pool.Add(name, new List<GameObject>());
        pool[name].Add(o);
    }
 
    public GameObject TackOut(string name)
    {
        GameObject o = null;
        if (!pool.ContainsKey(name))
        {
            pool.Add(name, new List<GameObject>());
            return o;
        }
        if (pool[name].Count == 0)
        {
            return o;
        }
        o = pool[name][0];
        pool[name].RemoveAt(0);
        return o;
    }
 
}