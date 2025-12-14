using System;
using _Game._Scripts;
using LoopGames.Combat;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCollisionController : MonoBehaviour
{
    [SerializeField] private PlayerSwordOrbitController _playerSwordOrbitController;
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
        _playerSwordOrbitController.SpawnSword();
    }


    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
    
    
    public PlayerSwordOrbitController GetSwordOrbitController()
    {
        return _playerSwordOrbitController;
    }
}