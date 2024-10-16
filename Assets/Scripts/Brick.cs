using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    
    public int PointValue;

    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", Color.white); //red
                break;
            case 2:
                block.SetColor("_BaseColor", Color.red);//orange
                break;
            case 3:
                block.SetColor("_BaseColor", Color.yellow);//yellow
                break;
            case 4:
                block.SetColor("_BaseColor", Color.green);//green
                break;
            case 5:
                block.SetColor("_BaseColor", Color.cyan);// blue
                break;
            case 6:
                block.SetColor("_BaseColor", Color.blue);// indigo
                break;
            case 7:
                block.SetColor("_BaseColor", Color.magenta);//purple
                break;
            default:
                block.SetColor("_BaseColor", Color.black);
                break;
        }
        renderer.SetPropertyBlock(block);
    }

    private void OnCollisionEnter(Collision other)
    {
        onDestroyed.Invoke(PointValue);
        
        //slight delay to be sure the ball have time to bounce
        Destroy(gameObject, 0.1f);
    }
}
