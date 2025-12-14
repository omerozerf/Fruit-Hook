using UnityEngine;

namespace _Game._Scripts.SwordBubbleSystem
{
    public class SwordBubbleCollision : MonoBehaviour
    {
        [SerializeField] private SwordBubble _swordBubble;


        private void OnValidate()
        {
            if (!_swordBubble) _swordBubble = GetComponentInParent<SwordBubble>();
        }

        public SwordBubble GetSwordBubble()
        {
            return _swordBubble;
        }
    }
}