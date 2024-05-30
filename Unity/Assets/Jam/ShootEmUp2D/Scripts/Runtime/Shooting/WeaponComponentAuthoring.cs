using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public class WeaponComponentAuthoring : MonoBehaviour
    {
        [Header("Bullet")]
        public GameObject BulletPrefab;
        public float BulletSpeed = 10;
        public float BulletFireRate = 10;

		[Header("MuzzleFlash")]
		public GameObject MuzzleFlashPrefab;

		[Header("Shooting")]
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
					GetEntity(authoring.MuzzleFlashPrefab, TransformUsageFlags.Dynamic),
					authoring.ShootDirectionIsUp,
					authoring.BulletSpeed,
                    authoring.BulletFireRate,
                    authoring.Type)
                );
            }
        }
    }
}
