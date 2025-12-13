using UnityEngine;

namespace _Game
{
	public class TileCreator : MonoBehaviour
	{
		[SerializeField]
		private Tile _tilePrefab;

		public Tile CreateTile(Vector3 position, Transform parent)
		{
			return Object.Instantiate(_tilePrefab, position, Quaternion.identity, parent);
		}
	}
}
