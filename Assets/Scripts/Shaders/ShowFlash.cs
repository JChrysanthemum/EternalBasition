using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class DrawMesh
{
    public Mesh mesh;
    public Matrix4x4 matrix;
    public Material material;
    public DrawMesh(Mesh me, Matrix4x4 ma, Material mat, Color color)
    {
        this.mesh = me;
        this.matrix = ma;
        this.material = new Material(mat);
        this.material.shader = Shader.Find("Transparent/VertexLit");
        this.material.SetColor("_Emission", color);
    }
}

/// <summary>
/// 
/// </summary>
public class ShowFlash : MonoBehaviour
{
    private SkinnedMeshRenderer[] render;

    private SkinnedMeshRenderer mBodyMesh;

    [SerializeField]
    private bool isStart;

    [SerializeField]
    private float durationMaxTime = 0.2f ;
    private float durationtimer;
    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private float rate;
    [SerializeField]
    private Color color;
    private List<DrawMesh> list;

    /// <summary>
    /// The max time of this flash action
    /// </summary>
    public float DurationTime
    {
        get
        {
            return durationMaxTime;
        }
        set
        {
            if (value > 0 && value < 5)
                durationMaxTime = value;
        }
    }

    /// <summary>
    /// If this flash action is showning 
    /// </summary>
    public bool Showning
    {
        get
        {
            return isStart;
        }
        set
        {
            isStart = value;
            if (isStart == true)
            {
                durationtimer = durationMaxTime;
            }
            else
            {
                durationtimer = 0;
            }
        }
    }

    void Awake()
    {
        render = GetComponentsInChildren<SkinnedMeshRenderer>();
        mBodyMesh = GetComponent<SkinnedMeshRenderer>();
        list = new List<DrawMesh>();
        isStart = false;
        durationtimer = durationMaxTime;
    }

    void Update()
    {
        UseSkill();
    }

    public void UseSkill()
    {
        if (isStart)
        {
            for (int i = 0; i < render.Length; i++)
            {
                Mesh mesh = new Mesh();
                render[i].BakeMesh(mesh);
                list.Add(new DrawMesh(mesh, render[i].transform.localToWorldMatrix, render[i].material, color));
            }
        }
    }

    public void showStart()
    {
        isStart = true;
        durationtimer = durationMaxTime;
        //mBodyMesh.enabled = false;
    }
    public void showEnd()
    {
        //mBodyMesh.enabled = true;
    }
    void FixedUpdate()
    {
        if (isStart)
        {
            if (durationtimer > 0) 
            {
                isStart = true;
                durationtimer -= Time.deltaTime;
            }
            else                                  
            {
                isStart = false;
            }
        }
    }


    void LateUpdate()
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i].material.SetColor("_Color", new Color(color.r, color.g, color.b, list[i].material.color.a - Time.deltaTime * fadeSpeed));
            if (list[i].material.color.a <= 0.05f)
            {
                Destroy(list[i].material);
                Destroy(list[i].mesh);
                list.RemoveAt(i);
            }
        }
        for (int i = list.Count - 1; i >= 0; i--)
        {
            Graphics.DrawMesh(list[i].mesh, list[i].matrix, list[i].material, gameObject.layer);
        }
    }
}