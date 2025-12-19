using System;
using System.Collections;
using ScratchCardAsset.Core;
using ScratchCardAsset.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace ScratchCardAsset
{
	/// <summary>
	/// Calculates scratching progress in range from 0 to 1, where 0 - card scratched completely, 1 - scratch surface is whole
	/// </summary>
	public class EraseProgress : MonoBehaviour
	{
		#region Events

		public event Action<float> OnProgress;
		public event Action<float> OnCompleted;

		#endregion

		#region Variables

		[SerializeField, FormerlySerializedAs("Card")] private ScratchCard card;
		public ScratchCard Card
		{
			get => card;
			set => card = value;
		}

		[SerializeField, FormerlySerializedAs("ProgressMaterial")] private Material progressMaterial;
		public Material ProgressMaterial
		{
			get => progressMaterial;
			set => progressMaterial = value;
		}
		
		[SerializeField, FormerlySerializedAs("SampleSourceTexture")] private bool sampleSourceTexture;
		public bool SampleSourceTexture
		{
			get => sampleSourceTexture;
			set => sampleSourceTexture = value;
		}

		[SerializeField] private ProgressAccuracy progressAccuracy;
		public ProgressAccuracy ProgressAccuracy
		{
			get => progressAccuracy;
			set
			{
				progressAccuracy = value;

				// Luna/WebGL: AsyncGPUReadback yolu derlenmiyor / desteklenmiyor.
				// High seçilse bile Default akışa düş.
				if (progressAccuracy == ProgressAccuracy.High)
				{
					Debug.LogWarning("ProgressAccuracy.High is not supported on this platform. Switching to ProgressAccuracy.Default.");
					progressAccuracy = ProgressAccuracy.Default;
				}

				UpdateAccuracy();
				updateProgress = false;
			}
		}

		private ScratchMode scratchMode;
		private int updateProgressFrame;
		private Color[] sourceSpritePixels;
		private CommandBuffer commandBuffer;
		private Mesh mesh;
		private RenderTexture percentRenderTexture;
		private RenderTargetIdentifier percentTargetIdentifier;
		private Rect percentTextureRect;
		private Texture2D progressTexture;
		private float progress;
		private bool updateProgress;
		private bool isCalculating;
		private bool isCompleted;

		#endregion

		#region MonoBehaviour Methods

		private void Start()
		{
			Init();
		}

		private void OnDestroy()
		{
			if (percentRenderTexture != null && percentRenderTexture.IsCreated())
			{
				percentRenderTexture.Release();
				Destroy(percentRenderTexture);
				percentRenderTexture = null;
			}
			
			if (progressTexture != null)
			{
				Destroy(progressTexture);
				progressTexture = null;
			}

			if (mesh != null)
			{
				Destroy(mesh);
				mesh = null;
			}

			if (commandBuffer != null)
			{
				commandBuffer.Release();
				commandBuffer = null;
			}

			if (card != null)
			{
				card.OnRenderTextureInitialized -= OnCardRenderTextureInitialized;
			}
		}

		private void LateUpdate()
		{
			if (card.Mode != scratchMode)
			{
				scratchMode = card.Mode;
				ResetProgress();
			}
			
			if ((card.IsScratched || updateProgress) && !isCompleted)
			{
				UpdateProgress();
			}
		}

		#endregion

		#region Private Methods

		private void Init()
		{
			if (card == null)
			{
				Debug.LogError("Card field is not assigned!");
				enabled = false;
				return;
			}
			
			if (card.Initialized)
			{
				OnCardRenderTextureInitialized(card.RenderTexture);
			}
			
			card.OnRenderTextureInitialized += OnCardRenderTextureInitialized;
			UpdateAccuracy();
			scratchMode = card.Mode;
			commandBuffer = new CommandBuffer {name = "EraseProgress"};
			mesh = MeshGenerator.GenerateQuad(Vector3.one, Vector3.zero);
			var renderTextureFormat = RenderTextureFormat.ARGB32;
			percentRenderTexture = new RenderTexture(1, 1, 0, renderTextureFormat);
			percentTargetIdentifier = new RenderTargetIdentifier(percentRenderTexture);
			percentTextureRect = new Rect(0, 0, percentRenderTexture.width, percentRenderTexture.height);
			var textureFormat = TextureFormat.ARGB32;
			progressTexture = new Texture2D(percentRenderTexture.width, percentRenderTexture.height, textureFormat, false, true);
		}
		
		private void OnCardRenderTextureInitialized(RenderTexture renderTexture)
		{
			// No-op: High accuracy path disabled for Luna/WebGL compatibility.
		}

		private void UpdateAccuracy()
		{
			// High accuracy yolu kapalı. Default dışında bir şey kalmasın.
			if (progressAccuracy != ProgressAccuracy.Default)
			{
				progressAccuracy = ProgressAccuracy.Default;
			}
		}

		/// <summary>
		/// Calculates scratch progress
		/// </summary>
		private IEnumerator CalcProgress()
		{
			if (!isCompleted && !isCalculating)
			{
				isCalculating = true;

				var prevRenderTexture = RenderTexture.active;
				RenderTexture.active = percentRenderTexture;
				progressTexture.ReadPixels(percentTextureRect, 0, 0);
				progressTexture.Apply();
				RenderTexture.active = prevRenderTexture;

				var pixel = progressTexture.GetPixel(0, 0);
				progress = pixel.r;

				OnProgress?.Invoke(progress);
				if (OnCompleted != null)
				{
					var completeValue = card.Mode == ScratchMode.Erase ? 1f : 0f;
					if (Mathf.Abs(progress - completeValue) < float.Epsilon)
					{
						OnCompleted?.Invoke(progress);
						isCompleted = true;
					}
				}

				isCalculating = false;
			}
			else if (isCalculating && card.IsScratched)
			{
				updateProgress = true;
				updateProgressFrame = Time.frameCount;
			}

			yield break;
		}
		
		#endregion
		
		#region Public Methods

		public float GetProgress()
		{
			return progress;
		}

		public void UpdateProgress()
		{
			if (commandBuffer == null)
			{
				Debug.LogError("Can't update progress cause commandBuffer is null!");
				return;
			}
			
			GL.LoadOrtho();
			commandBuffer.Clear();
			commandBuffer.SetRenderTarget(percentTargetIdentifier);
			commandBuffer.ClearRenderTarget(false, true, Color.clear);
			var pass = sampleSourceTexture ? 1 : 0;
			commandBuffer.DrawMesh(mesh, Matrix4x4.identity, progressMaterial, 0, pass);
			Graphics.ExecuteCommandBuffer(commandBuffer);
			if (gameObject.activeInHierarchy)
			{
				StartCoroutine(CalcProgress());
			}
		}

		public void ResetProgress()
		{
			isCompleted = false;
		}

		public void SetSpritePixels(Color[] pixels)
		{
			sourceSpritePixels = pixels;
		}

		#endregion
	}
}