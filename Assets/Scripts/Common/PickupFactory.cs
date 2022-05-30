using UnityEngine;
using System.Collections;

namespace IR.Factories
{
    public class PickupFactory : MonoBehaviour
    {
        // TODO: add to global settings
        private static Transform group = GameObject.Find("/Pickup Container").transform;

        public static void SpawnMinerals(GameObject mineralSprayer, Vector2 position, int count, float quality)
        {
            var msObj = Instantiate(mineralSprayer, position, Quaternion.identity, group);
            var ms = msObj.GetComponent<MineralSprayer>();
            ms.NumberOfCoins = count;
            ms.Quality = quality;
            ms.transform.SetParent(group);
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