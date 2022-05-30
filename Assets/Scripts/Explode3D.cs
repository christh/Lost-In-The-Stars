using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IR.Factories;

public class Explode3D : MonoBehaviour
{
    public GameObject ExplosionPreFab;
    public AudioClip ExplosionAudio;
    public string[] TriggerableTags;

    private bool Triggered;

    void OnCollisionEnter(Collision other)
    {
        if (TriggerableTags.Length == 0)
        {
            // Explodes on everything!
            GoBoom();
        }

        // Otherwise see if the tag exists in triggerable array
        foreach (var item in TriggerableTags)
        {
            if ((other.gameObject.CompareTag(item)))
            {
                GoBoom();
                return;
            }
        }
    }

    void GoBoom()
    {
        Triggered = true;


        // TODO: Create and play fade out animation..?
        ExplosionFactory.SpawnExplosion3D(ExplosionPreFab, ExplosionAudio, transform.position);

        // TODO: Disable collision / remove tag ..?
        //GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        Destroy(gameObject, 2);
    }
}
