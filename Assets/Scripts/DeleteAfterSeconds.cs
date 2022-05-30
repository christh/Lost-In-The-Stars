using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DeleteAfterSeconds : MonoBehaviour
{
    [SerializeField] float seconds = 3;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, seconds);
    }
}
