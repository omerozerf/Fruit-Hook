using System;
using UnityEngine;

namespace _Game._Scripts.SwordSystem
{
    public class SwordCollisionController : MonoBehaviour
    {
        [SerializeField] private Sword _sword;
        [SerializeField] private LayerMask _swordLayerMask;

        private void OnTriggerEnter2D(Collider2D other)
        {
            HandleSwordCollision(other);
        }

        
        private void HandleSwordCollision(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, _swordLayerMask)) return;
            if (!other.TryGetComponent(out SwordCollisionController otherSwordCollision)) return;
            var otherOrbitController = otherSwordCollision.GetSword().GetSwordOrbitController();
            var myOrbitController = GetSword().GetSwordOrbitController();
            if (myOrbitController == otherOrbitController) return;
            
            var otherSword = otherSwordCollision.GetSword();
            var mySword = GetSword();

            if (otherOrbitController && otherSword.transform) otherOrbitController.RemoveSword(otherSword.transform);
            if (myOrbitController && mySword.transform) myOrbitController.RemoveSword(mySword.transform);
        }

        private void OnValidate()
        {
            if (!_sword) _sword = GetComponentInParent<Sword>();
        }
        
        
        private bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            return (mask.value & (1 << obj.layer)) != 0;
        }


        public Sword GetSword()
        {
            return _sword;
        }
    }
}