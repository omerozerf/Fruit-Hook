using _Game._Scripts.MapSystem;
using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "GridMapBuilderSettings",
        menuName = "Game/Map/Grid Map Builder Settings",
        order = 0)]
    public class GridMapBuilderSettingsSO : ScriptableObject
    {
        [Header("Grid")] [Min(1)] [SerializeField]
        private int _width = 10;

        [Min(1)] [SerializeField] private int _height = 10;
        [Min(0.01f)] [SerializeField] private float _cellSize = 1f;

        [Header("Ground Prefabs (Weighted)")] [SerializeField]
        private WeightedPrefab[] _groundVariants;

        [Header("Fence")] [SerializeField] private Transform _fenceVertical;

        [SerializeField] private Transform _fenceHorizontal;
        [SerializeField] private Transform _fenceCorner;

        [Header("Global Sorting")] [SerializeField]
        private int _startSortingOrder;

        [Header("Extra Padding")] [Min(0)] [SerializeField]
        private int _extraPaddingCells;

        [Header("Area Transform")]
        [Tooltip("GridMapBuilder içindeki Area Transform güncellemesini aç/kapat")]
        [SerializeField]
        private bool _canUpdateArea = true;

        [Tooltip("Harita boyutundan hesaplanan base scale ile çarpılır")] [SerializeField]
        private float _areaScaleMultiplier = 1f;

        [Header("Chunk Culling")] [SerializeField]
        private bool _enableChunkCulling = true;

        [SerializeField] [Min(1)] private int _chunkSizeInCells = 8;
        [SerializeField] [Min(0f)] private float _cullPaddingWorld = 1f;

        [Header("Boundary Colliders")] [SerializeField]
        private bool _enableBoundaryColliders = true;

        [SerializeField] [Min(0.01f)] private float _boundaryThicknessWorld = 0.5f;


        public int Width => _width;
        public int Height => _height;
        public float CellSize => _cellSize;

        public WeightedPrefab[] GroundVariants => _groundVariants;

        public Transform FenceVertical => _fenceVertical;
        public Transform FenceHorizontal => _fenceHorizontal;
        public Transform FenceCorner => _fenceCorner;

        public int StartSortingOrder => _startSortingOrder;
        public int ExtraPaddingCells => _extraPaddingCells;

        public bool CanUpdateArea => _canUpdateArea;
        public float AreaScaleMultiplier => _areaScaleMultiplier;

        public bool EnableChunkCulling => _enableChunkCulling;
        public int ChunkSizeInCells => _chunkSizeInCells;
        public float CullPaddingWorld => _cullPaddingWorld;

        public bool EnableBoundaryColliders => _enableBoundaryColliders;
        public float BoundaryThicknessWorld => _boundaryThicknessWorld;
    }
}