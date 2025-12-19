using _Game._Scripts.MapSystem.Helpers;
using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.MapSystem
{
    public sealed class GridMapBuilder : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GridMapBuilderSettingsSO _settings;

        [Header("Scene References")]
        [SerializeField] private Camera _cullCamera;
        [SerializeField] private Transform _areaTransform;
        
        private BoundaryColliderBuilder m_BoundaryBuilder;
        private ChunkRootProvider m_ChunkRoots;

        private BuildContext m_Ctx;
        private ChunkCullingController m_Culling;
        private FenceBuilder m_FenceBuilder;
        private GroundBuilder m_GroundBuilder;
        private WeightedGroundSelector m_GroundSelector;
        private SortingService m_Sorting;

        private void Awake()
        {
            if (!ValidateSettings())
                return;

            if (!_cullCamera)
                _cullCamera = Camera.main;

            m_Ctx = new BuildContext();
            m_ChunkRoots = new ChunkRootProvider(transform, _settings);
            m_GroundSelector = new WeightedGroundSelector(_settings);
            m_GroundBuilder = new GroundBuilder(_settings, m_ChunkRoots, m_GroundSelector, m_Ctx);
            m_FenceBuilder = new FenceBuilder(_settings, m_ChunkRoots, m_Ctx);
            m_BoundaryBuilder = new BoundaryColliderBuilder(transform, _settings, m_Ctx);
            m_Sorting = new SortingService(_settings, m_Ctx);
            m_Culling = new ChunkCullingController(_settings, _cullCamera, m_ChunkRoots);

            BuildAll();
            UpdateAreaTransform();
        }

        private void LateUpdate()
        {
            m_Culling?.Tick();
        }

        private void OnDisable()
        {
            m_BoundaryBuilder?.Clear();
        }

        private bool ValidateSettings()
        {
            if (_settings)
                return true;

            Debug.LogError(
                $"{nameof(GridMapBuilder)} on '{name}' has no {nameof(GridMapBuilderSettingsSO)} assigned.", this);
            enabled = false;
            return false;
        }

        private void BuildAll()
        {
            m_Ctx.Reset();

            m_GroundBuilder.Build();
            m_FenceBuilder.Build();

            if (_settings.EnableBoundaryColliders)
                m_BoundaryBuilder.Build();

            m_Sorting.ApplyGlobalSorting();
            m_Sorting.DisableTopCornerLastRenderers();

            m_Culling.ForceRefreshIfEnabled();
        }

        /// <summary>
        /// Area'yı haritanın merkezine alır ve harita boyutuna göre scale eder
        /// </summary>
        private void UpdateAreaTransform()
        {
            if (!_areaTransform || !_settings.CanUpdateArea)
                return;

            Vector2 mapSizeWorld = GetMapWorldSize();

            Vector3 centerPos = new Vector3(
                mapSizeWorld.x * 0.5f,
                mapSizeWorld.y * 0.5f,
                _areaTransform.position.z
            );

            float baseScale = Mathf.Max(mapSizeWorld.x, mapSizeWorld.y);
            float finalScale = baseScale * _settings.AreaScaleMultiplier;

            _areaTransform.position = centerPos;
            _areaTransform.localScale = Vector3.one * finalScale;
        }

        /// <summary>
        /// Haritanın world space boyutunu döner
        /// </summary>
        private Vector2 GetMapWorldSize()
        {
            float width = _settings.Width * _settings.CellSize;
            float height = _settings.Height * _settings.CellSize;
            return new Vector2(width, height);
        }
    }
}