using _Game._Scripts.SwordBubbleSystem;
using _Game._Scripts.SwordOrbitSystem;
using UnityEngine;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerCollisionController : MonoBehaviour
    {
        [SerializeField] private SwordOrbitController _swordOrbitController;
        [SerializeField] private LayerMask _bubbleSwordLayerMask;


        private void OnTriggerEnter2D(Collider2D other)
        {
            HandleSwordBubbleCollision(other);
        }

    
        private void HandleSwordBubbleCollision(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, _bubbleSwordLayerMask)) return;
            if (!other.TryGetComponent(out SwordBubbleCollision swordBubbleCollision)) return;
        
            swordBubbleCollision.GetSwordBubble().gameObject.SetActive(false);
            _swordOrbitController.SpawnSword();
        }


        private bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            return (mask.value & (1 << obj.layer)) != 0;
        }
    
    
        public SwordOrbitController GetSwordOrbitController()
        {
            return _swordOrbitController;
        }
    }
}