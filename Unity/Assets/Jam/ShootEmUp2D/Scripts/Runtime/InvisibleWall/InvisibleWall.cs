using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
	public class InvisibleWall : MonoBehaviour
	{
		[SerializeField]
		public bool _isVisibleOnAwake = false;

		[SerializeField]
		public SpriteRenderer _spriteRenderer;
		
		protected void Awake()
		{
			_spriteRenderer.enabled = _isVisibleOnAwake;
		}
	}
}