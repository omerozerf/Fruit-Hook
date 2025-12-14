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
        [SerializeField] private GameObject _groundPrefab;
        [SerializeField] private GameObject[] _groundVariants;
        [SerializeField] private GameObject _fenceHorizontal;
        [SerializeField] private GameObject _fenceVertical;
        [SerializeField] private GameObject _fenceCorner;

        
        private void Start()
        {
            BuildGround();
            BuildFence();
        }

        
        private void BuildGround()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector3 pos = GridToWorld(x, y);
                    GameObject prefab = GetRandomGround();
                    Instantiate(prefab, pos, Quaternion.identity, transform);
                }
            }
        }

        private void BuildFence()
        {
            for (int x = -1; x <= _width; x++)
            {
                Instantiate(_fenceHorizontal, GridToWorld(x, -1), Quaternion.identity, transform);
                Instantiate(_fenceHorizontal, GridToWorld(x, _height), Quaternion.identity, transform);
            }

            for (int y = 0; y < _height; y++)
            {
                Instantiate(_fenceVertical, GridToWorld(-1, y), Quaternion.identity, transform);
                Instantiate(_fenceVertical, GridToWorld(_width, y), Quaternion.identity, transform);
            }

            Instantiate(_fenceCorner, GridToWorld(-1, -1), Quaternion.identity, transform);
            Instantiate(_fenceCorner, GridToWorld(_width, -1), Quaternion.identity, transform);
            Instantiate(_fenceCorner, GridToWorld(-1, _height), Quaternion.identity, transform);
            Instantiate(_fenceCorner, GridToWorld(_width, _height), Quaternion.identity, transform);
        }

        private Vector3 GridToWorld(int x, int y)
        {
            return new Vector3(x * _cellSize, y * _cellSize, 0f);
        }

        private GameObject GetRandomGround()
        {
            if (_groundVariants == null || _groundVariants.Length == 0)
                return _groundPrefab;

            return _groundVariants[Random.Range(0, _groundVariants.Length)];
        }
    }
}