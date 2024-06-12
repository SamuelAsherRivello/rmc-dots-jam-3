using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
	public class GameObjectHybridComponent : IComponentData
	{
		public GameObject GameObject;
		public float3 PositionOffset { get; set; }
	}
}