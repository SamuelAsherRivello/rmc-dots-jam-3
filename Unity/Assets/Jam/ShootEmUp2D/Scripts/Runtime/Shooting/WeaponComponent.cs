using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public enum WeaponType
    {
        SINGLE_SHOT,
        SHOTGUN_SPREAD,
        TRIPLE_SHOT
    }

    public partial struct WeaponComponent : IComponentData
    {
        public readonly Entity BulletPrefab;
        public readonly float BulletSpeed;
        public readonly float BulletFireRate;
        public readonly WeaponType Type;

        public double _NextCooldownTime;

        public WeaponComponent(Entity bulletPrefab, float bulletSpeed, float bulletFireRate, WeaponType newType)
        {
            BulletPrefab = bulletPrefab;
            BulletSpeed = bulletSpeed;
            BulletFireRate = bulletFireRate;
            Type = newType;
            _NextCooldownTime = 0.0f;
        }
    }
}