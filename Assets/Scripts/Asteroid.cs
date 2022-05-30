using UnityEngine;
using System.Linq;
using IR.Common;

namespace IR
{
    public class Asteroid : MonoBehaviour, IDamageable
    {
        private Rigidbody2D body;
        public float baseHealth = 10;
        public float health = 10;

        [SerializeField]
        public AsteroidSize size;

        // Start is called before the first frame update
        void Start()
        {
            float scaleModifier = 0;

            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;

            scaleModifier = Utils.GetAsteroidScale(size);

            health = baseHealth * scaleModifier;
            

            body = GetComponent<Rigidbody2D>();
            transform.rotation = Quaternion.Euler(new Vector3() { z = Random.Range(0, 360) });
            gameObject.layer = Constants.Layers.Environment;
        }

        // Called when instantiated dynamically
        public void Setup()
        {
            if (body == null)
            {
                body = GetComponent<Rigidbody2D>();
            }

            float scaleModifier = 0;
            scaleModifier = Utils.GetAsteroidScale(size);
            transform.localScale = new Vector3(scaleModifier, scaleModifier, 1);

            body.AddTorque(Random.Range(-30 * scaleModifier, 30 * scaleModifier));
            body.AddForce(new Vector2(Random.Range(20 * scaleModifier, 100 * scaleModifier), Random.Range(20 * scaleModifier, 100 * scaleModifier)));

        }

        public void Damage(float damageValue)
        {
            health = (int)(health - damageValue);

            if (health <= 0)
            {
                gameObject.layer = Constants.Layers.DeadThings;
                GetComponent<IDropsLoot>()?.Drop();
                var boomThings = GetComponents<IExplodeable>();
                foreach (var item in boomThings)
                {
                    item.Explode();
                }

            }
            else
            {
                // We'd spawn some damage components here or something
                // Or shield twinkles if shield still up
            }
        }
    }
}