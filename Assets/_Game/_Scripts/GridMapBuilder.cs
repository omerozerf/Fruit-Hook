using UnityEngine;
using UnityEngine.Serialization;

namespace _Game._Scripts
{
    public sealed class GridMapBuilder : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private float _cellSize;

        [Header("Prefabs")]
        [SerializeField, Range(0, 100)] private int _baseGroundWeight;
        [SerializeField] private GameObject[] _groundVariants;
        [SerializeField] private GameObject _fenceCorner;


        private void Start()
        {
            BuildGround();
            BuildFence();
        }


        private void BuildGround()
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var prefab = GetRandomGround();
                    if (prefab == null)
                        continue;

                    Instantiate(prefab, GridToWorld(x, y), Quaternion.identity, transform);
                }
            }
        }

        private void BuildFence()
        {
            // Alt ve üst kenarlar (corner prefab full)
            for (var x = -1; x <= _width; x++)
            {
                Instantiate(_fenceCorner, GridToWorld(x, -1), Quaternion.identity, transform);
                Instantiate(_fenceCorner, GridToWorld(x, _height), Quaternion.identity, transform);
            }

            // Sol ve sağ kenarlar (corner prefab full)
            for (var y = 0; y < _height; y++)
            {
                Instantiate(_fenceCorner, GridToWorld(-1, y), Quaternion.identity, transform);
                Instantiate(_fenceCorner, GridToWorld(_width, y), Quaternion.identity, transform);
            }
        }

        private Vector3 GridToWorld(int x, int y)
        {
            return new Vector3(x * _cellSize, y * _cellSize, 0f);
        }

        private GameObject GetRandomGround()
        {
            if (_groundVariants == null || _groundVariants.Length == 0)
            {
                Debug.LogError("No ground variants assigned in GridMapBuilder.");
                return null;
            }
            
            int totalWeight = _baseGroundWeight + (_groundVariants.Length - 1);
            int random = Random.Range(0, totalWeight);

            if (random < _baseGroundWeight)
                return _groundVariants[0];

            int index = random - _baseGroundWeight + 1;
            return _groundVariants[index];
        }
    }
}