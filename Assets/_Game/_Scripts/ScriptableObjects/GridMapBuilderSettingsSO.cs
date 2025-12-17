using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GridMapBuilderSettings", menuName = "Game/Map/Grid Map Builder Settings", order = 0)]
    public class GridMapBuilderSettingsSO : ScriptableObject
    {
        [Header("Grid")]
        [Min(1)] [SerializeField] private int _width = 10;
        [Min(1)] [SerializeField] private int _height = 10;
        [Min(0.01f)] [SerializeField] private float _cellSize = 1f;

        [Header("Ground Prefabs (Weighted)")]
        [SerializeField] private GridMapBuilder.WeightedPrefab[] _groundVariants;

        [Header("Fence")]
        [SerializeField] private Transform _fenceVertical;
        [SerializeField] private Transform _fenceHorizontal;
        [SerializeField] private Transform _fenceCorner;

        [Header("Global Sorting")]
        [Tooltip("Assigned in decreasing order. Example: 0, -1, -2 ... (negative values are OK).")]
        [SerializeField] private int _startSortingOrder = 0;

        [Header("Extra Padding")]
        [Tooltip("Number of extra tile rows/columns to generate outside the main grid on each side.")]
        [Min(0)] [SerializeField] private int _extraPaddingCells = 0;

        public int Width => _width;
        public int Height => _height;
        public float CellSize => _cellSize;

        public GridMapBuilder.WeightedPrefab[] GroundVariants => _groundVariants;

        public Transform FenceVertical => _fenceVertical;
        public Transform FenceHorizontal => _fenceHorizontal;
        public Transform FenceCorner => _fenceCorner;

        public int StartSortingOrder => _startSortingOrder;
        public int ExtraPaddingCells => _extraPaddingCells;
    }
}