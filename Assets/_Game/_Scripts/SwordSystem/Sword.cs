using _Game._Scripts.ObjectPoolSystem;
using _Game._Scripts.SwordOrbitSystem;
using UnityEngine;

namespace _Game._Scripts.SwordSystem
{
    public class Sword : MonoBehaviour, IPoolable
    {
        [SerializeField] private SwordCollisionController _collisionController;
        [SerializeField] private Collider2D _collider;
        
        private SwordOrbitController m_SwordOrbitController;
        
        
        private void OnValidate()
        {
            if (!_collisionController) _collisionController = GetComponentInChildren<SwordCollisionController>();
            if (!_collider) _collider = GetComponentInChildren<Collider2D>();
        }


        public void SetSwordOrbitController(SwordOrbitController controller)
        {
            m_SwordOrbitController = controller;
        }
        
        public SwordOrbitController GetSwordOrbitController()
        {
            return m_SwordOrbitController;
        }
        
        public SwordCollisionController GetCollisionController()
        {
            return _collisionController;
        }
        
        public Collider2D GetCollider()
        {
            return _collider;
        }

        public void SetColliderEnabled(bool isEnabled)
        {
            _collider.enabled = isEnabled;
        }

        
        public void OnSpawnedFromPool()
        {
            SetColliderEnabled(true);
        }

        public void OnDespawnedToPool()
        {
            SetColliderEnabled(false);
        }
    }
}