using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public class PickupSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField]
        GameObject Prefab;

        [SerializeField]
        float DefaultLinearSpeed;

        public class PickupSpawnerAuthoringBaker : Baker<PickupSpawnerAuthoring>
        {
            public override void Bake(PickupSpawnerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PickupSpawnerComponent
                (
                    GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                    authoring.DefaultLinearSpeed
                ));
            }
        }
    }
}
