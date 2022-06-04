using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerGame.Gameplay
{
    public class AreaController : MonoBehaviour
    {
        [Header("Set Values")]
        [SerializeField] LayerMask hittableLayers;
        [SerializeField] public int maxEntities;
        [Header("Runtime Values")]
        [SerializeField] List<Transform> currentEntities;
        [SerializeField] Vector2 leftDownCorner;
        [SerializeField] Vector2 rightUpCorner;
        [SerializeField] bool poolMaxed;
        List<Transform> newEntities;

        public System.Action<int> PoolChanged;
        public System.Action<bool> PoolMaxed;

        //Unity Events
        private void Start()
        {
            leftDownCorner = transform.position - transform.lossyScale / 2;
            rightUpCorner = transform.position + transform.lossyScale / 2;
            newEntities = new List<Transform>();
        }
        private void FixedUpdate()
        {
            UpdateEntityCount();
        }

        //Methods
        public void CheckPool()
        {
            //If pool maxed, trigger event
            bool isPoolMaxed = currentEntities.Count >= maxEntities;
            if (isPoolMaxed != poolMaxed)
            {
                poolMaxed = isPoolMaxed;
                PoolMaxed?.Invoke(poolMaxed);
            }
        }
        void UpdateEntityCount()
        {
            //Get all Players
            Collider2D[] hits;
            hits = Physics2D.OverlapAreaAll(leftDownCorner, rightUpCorner, hittableLayers);

            //If got the same players as before, exit
            if (hits.Length == currentEntities.Count) return;

            newEntities.Clear();

            //Add new entities to pool
            foreach (var hit in hits)
            {
                newEntities.Add(hit.transform);
            }

            //Add new entity
            currentEntities.Clear();
            currentEntities.AddRange(newEntities);
            PoolChanged?.Invoke(currentEntities.Count);

            CheckPool();
        }
    }
}