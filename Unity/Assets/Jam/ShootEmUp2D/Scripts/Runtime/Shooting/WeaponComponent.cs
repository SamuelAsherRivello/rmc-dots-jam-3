using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public partial struct WeaponComponent : IComponentData
    {
        public readonly Entity BulletPrefab;
        public readonly float BulletSpeed;
        public readonly float BulletFireRate;

        public double _NextCooldownTime;

        public WeaponComponent(Entity bulletPrefab, float bulletSpeed, float bulletFireRate)
        {
            BulletPrefab = bulletPrefab;
            BulletSpeed = bulletSpeed;
            BulletFireRate = bulletFireRate;
            _NextCooldownTime = 0.0f;
        }
    }
}