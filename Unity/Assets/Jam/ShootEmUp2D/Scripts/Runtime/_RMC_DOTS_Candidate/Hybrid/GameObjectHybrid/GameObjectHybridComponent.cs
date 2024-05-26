using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
	public class GameObjectHybridComponent : IComponentData, IQueryTypeParameter
	{
		public GameObject GameObject;
	}
}