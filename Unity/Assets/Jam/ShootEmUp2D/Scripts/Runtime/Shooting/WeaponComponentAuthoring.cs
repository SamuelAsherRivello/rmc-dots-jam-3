using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public class WeaponComponentAuthoring : MonoBehaviour
    {
        public GameObject BulletPrefab;
        public float BulletSpeed = 10;
        public float BulletFireRate = 10;

        public class WeaponComponentAuthoringBaker : Baker<WeaponComponentAuthoring>
        {
            public override void Bake(WeaponComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new WeaponComponent
                (
                    GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                    authoring.BulletSpeed,
                    authoring.BulletFireRate)
                );
            }
        }
    }
}
