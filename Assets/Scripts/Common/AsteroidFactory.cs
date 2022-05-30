using UnityEngine;
using System.Collections;

namespace IR.Factories
{
    public class AsteroidFactory : MonoBehaviour
    {
        // TODO: add to global settings
        private static Transform containerGroup = GameObject.Find("/Asteroid Container").transform;

        public static void SpawnAsteroids(GameObject sprayer, Vector2 position, int count, AsteroidSize size, GameObject type)
        {
            var msObj = Instantiate(sprayer, position, Quaternion.identity, containerGroup);
            var ms = msObj.GetComponent<AsteroidSprayer>();
            ms.NumberOfAsteroids = count;
            ms.size = size;
            ms.AsteroidType = type;
            msObj.SetActive(true);
        }

        public static void PlaySpriteAnimation(GameObject animation, Vector2 position)
        {
            if (animation != null)
            {
                var obj = Instantiate(animation, position, Quaternion.identity);
                //var ms = obj.GetComponent<Animator>();
                Destroy(obj, 0.5f); // TODO: Should be animation.length...
            }
        }
    }
}