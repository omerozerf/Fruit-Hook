using UnityEngine;

namespace LoopGames.Playable
{
	public sealed class GridMapBuilder : MonoBehaviour
	{
		[Header("Grid")]
		[SerializeField]
		private int width = 10;

		[SerializeField]
		private int height = 10;

		[SerializeField]
		private float cellSize = 0.96f;

		[Header("Prefabs")]
		[SerializeField]
		private GameObject groundPrefab;

		[SerializeField]
		private GameObject[] groundVariants;

		[SerializeField]
		private GameObject fenceHorizontal;

		[SerializeField]
		private GameObject fenceVertical;

		[SerializeField]
		private GameObject fenceCorner;

		private void Start()
		{
			BuildGround();
			BuildFence();
		}

		private void BuildGround()
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Vector3 pos = GridToWorld(x, y);
					GameObject prefab = GetRandomGround();
					Object.Instantiate(prefab, pos, Quaternion.identity, base.transform);
				}
			}
		}

		private void BuildFence()
		{
			for (int x = -1; x <= width; x++)
			{
				Object.Instantiate(fenceHorizontal, GridToWorld(x, -1), Quaternion.identity, base.transform);
				Object.Instantiate(fenceHorizontal, GridToWorld(x, height), Quaternion.identity, base.transform);
			}
			for (int y = 0; y < height; y++)
			{
				Object.Instantiate(fenceVertical, GridToWorld(-1, y), Quaternion.identity, base.transform);
				Object.Instantiate(fenceVertical, GridToWorld(width, y), Quaternion.identity, base.transform);
			}
			Object.Instantiate(fenceCorner, GridToWorld(-1, -1), Quaternion.identity, base.transform);
			Object.Instantiate(fenceCorner, GridToWorld(width, -1), Quaternion.identity, base.transform);
			Object.Instantiate(fenceCorner, GridToWorld(-1, height), Quaternion.identity, base.transform);
			Object.Instantiate(fenceCorner, GridToWorld(width, height), Quaternion.identity, base.transform);
		}

		private Vector3 GridToWorld(int x, int y)
		{
			return new Vector3((float)x * cellSize, (float)y * cellSize, 0f);
		}

		private GameObject GetRandomGround()
		{
			if (groundVariants == null || groundVariants.Length == 0)
			{
				return groundPrefab;
			}
			return groundVariants[Random.Range(0, groundVariants.Length)];
		}
	}
}
