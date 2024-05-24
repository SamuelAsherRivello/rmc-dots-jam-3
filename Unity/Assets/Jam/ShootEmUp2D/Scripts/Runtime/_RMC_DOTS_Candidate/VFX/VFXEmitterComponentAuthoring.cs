using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.VFX
{
    public class VFXEmitterComponentAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        
		public class VFXEmitterComponentAuthoringBaker : Baker<VFXEmitterComponentAuthoring>
        {
            public override void Bake(VFXEmitterComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                                
                AddComponent(entity, VFXEmitterComponent.From
                (
	                GetEntity(authoring.Prefab,
		                TransformUsageFlags.Dynamic)
                ));
            }
        }
    }
}