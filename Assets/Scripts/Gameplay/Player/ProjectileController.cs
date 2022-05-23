using UnityEngine;

namespace MultiplayerGame.Gameplay.Player
{
    public class ProjectileController : MonoBehaviour
    {
        [Header("Set Values")]
        public Transform shooter;
        [SerializeField] Collider2D col;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] LayerMask hittableLayers;
        [SerializeField] float speed;
        [SerializeField] float lifeTime;
        [Header("Runtime Values")]
        public Vector2 movement;
        [SerializeField] float shooterCheckRadius;
        [SerializeField] float lifeTimer;

        public System.Action Destroyed;

        //Unity Events
        private void Start()
        {
            if (!col)
            {
                col = GetComponent<Collider2D>();
            }
            if (!rb)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            shooterCheckRadius = transform.lossyScale.x * 0.6f;
        }
        private void Update()
        {
            //If disc runned out of time, destroy
            if (lifeTimer > lifeTime)
            {
                Destroyed?.Invoke();
                Destroy(gameObject);
            }
            else
            {
                lifeTimer += Time.deltaTime;
            }

            //activate collider only if shooter is not here
            if (col.enabled == ShooterIsHit())
            {
                col.enabled = !col.enabled;
            }

            Move();
        }

        //Methods
        void Move()
        {
            if (!(movement.sqrMagnitude > 0)) return;

            rb.AddForce(movement.normalized * speed);
            movement = Vector2.zero;
        }
        bool ShooterIsHit()
        {
            //Get all Players
            Collider2D[] hits;
            hits = Physics2D.OverlapCircleAll(transform.position, shooterCheckRadius, hittableLayers);

            //If didn't get any players, return false
            if (hits.Length < 1) return false;

            //If some player is the shooter, return true
            foreach (var hit in hits)
            {
                if (hit.transform == shooter)
                {
                    return true;
                }
            }

            //If there was no shooter, return false
            return false;
        }
    }
}
