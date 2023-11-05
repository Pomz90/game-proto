using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solid : MonoBehaviour
{
    private Movement move;

    [HideInInspector]
    public SpriteRenderer sr;

    public int side = 1;
    private SpriteRenderer rend;
    private Shader mat;
    public Color col;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        mat = Shader.Find("GUI/Text Shader");
    }

    void colorSprite()
    {
        rend.material.shader = mat; 
        rend.color = col;  
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        colorSprite();
    }
   
}
