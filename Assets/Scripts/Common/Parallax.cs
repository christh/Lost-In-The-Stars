using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Material material;
    public GameObject player;
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }


    void FixedUpdate()
    {
        Debug.Log(material.mainTextureOffset);
        material.mainTextureOffset = new Vector2(player.transform.position.x / 10, player.transform.position.y / 10);
    }
}
