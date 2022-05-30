using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCriteria : MonoBehaviour
{
    [SerializeField] GameObject[] unlockAfterDestroyed;
    [SerializeField] AudioClip unlockSound;

    // Update is called once per frame
    void Update()
    {
        foreach (var item in unlockAfterDestroyed)
        {
            if (item != null)
            {
                return;
            }
        }

        AudioSource.PlayClipAtPoint(unlockSound, (Vector2)transform.position);
        Destroy(gameObject, 3);
    }
}
