using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public class WeaponComponentAuthoring : MonoBehaviour
    {
        public GameObject BulletPrefab;
        public float BulletSpeed = 10;
        public float BulletFireRate = 10;
		public bool ShootDirectionIsUp = true;
		public WeaponType Type = WeaponType.SINGLE_SHOT;

        public class WeaponComponentAuthoringBaker : Baker<WeaponComponentAuthoring>
        {
            public override void Bake(WeaponComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new WeaponComponent
                (
                    GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                    authoring.ShootDirectionIsUp,
					authoring.BulletSpeed,
                    authoring.BulletFireRate,
                    authoring.Type)
                );
            }
        }
    }
}
