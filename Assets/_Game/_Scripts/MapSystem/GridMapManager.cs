using _Game._Scripts.Patterns.SingletonPattern;
using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.MapSystem
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [SerializeField] private GridMapBuilderSettingsSO _settings;


        public int GetWidth()
        {
            return _settings.Width;
        }

        public int GetHeight()
        {
            return _settings.Height;
        }
    }
}