using System;
using UnityEngine;

namespace _Game._Scripts
{
    public class SwordBubbleCollision : MonoBehaviour
    {
        [SerializeField] private SwordBubble _swordBubble;


        private void OnValidate()
        {
            if (!_swordBubble) _swordBubble = GetComponentInParent<SwordBubble>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
        }

        public SwordBubble GetSwordBubble()
        {
            return _swordBubble;
        }
    }
}