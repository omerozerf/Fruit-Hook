using System;
using LoopGames.Combat;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [SerializeField] private SwordOrbitController _swordOrbitController;

    
    public SwordOrbitController GetSwordOrbitController()
    {
        return _swordOrbitController;
    }
}