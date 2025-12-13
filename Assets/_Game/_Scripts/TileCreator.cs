using UnityEngine;

namespace _Game
{
    public class TileCreator : MonoBehaviour
    {
        [SerializeField] private Tile _tilePrefab;


        public Tile CreateTile(Vector3 position, Transform parent)
        {
            var tile = Instantiate(_tilePrefab, position, Quaternion.identity, parent);
            return tile;
        }
    }
}