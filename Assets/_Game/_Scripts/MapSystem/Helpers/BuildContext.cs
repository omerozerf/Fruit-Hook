using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts.MapSystem.Helpers
{
    internal sealed class BuildContext
    {
        public readonly List<GameObject> boundaryObjects = new(4);
        public readonly List<Transform> spawnedRoots = new(512);
        public readonly List<Transform> topCornerRoots = new(4);

        public void Reset()
        {
            spawnedRoots.Clear();
            topCornerRoots.Clear();
            boundaryObjects.Clear();
        }
    }
}