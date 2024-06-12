using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.GameState;
using RMC.DOTS.Systems.Input;
using RMC.DOTS.Systems.Player;
using RMC.DOTS.Systems.VFX;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    /// <summary>
    /// This system moves the player in 3D space.
    /// </summary>
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    public partial class PlayerMoveSystem : SystemBase
    {
        private ComponentLookup<WeaponComponent> _weaponComponentLookup;


        protected override void OnCreate()
        {
            RequireForUpdate<InputComponent>();
        }

        protected override void OnUpdate()
        {
            float2 move = SystemAPI.GetSingleton<InputComponent>().MoveFloat2;
            float2 look = SystemAPI.GetSingleton<InputComponent>().LookFloat2;
            float deltaTime = SystemAPI.Time.DeltaTime;
            float2 moveComposite = float2.zero;
            
            //Use one input, but not both
            if (math.length(move) > 0.001f)
            {
                moveComposite.x = move.x;
                moveComposite.y =  move.y;
            }
            else
            {
                moveComposite.x = look.x;
                moveComposite.y = look.y;
            }

            
            foreach (var (physicsVelocity, physicsMass, playerMoveComponent, shootAspect) in 
                     SystemAPI.Query<RefRW<PhysicsVelocity>,RefRW<PhysicsMass>, RefRO<PlayerMoveComponent>, ShootAspect>().
                         WithAll<PlayerTag>())
            {
                float3 moveComposite3D = new float3(moveComposite.x, moveComposite.y, 0) *
                                         (deltaTime * playerMoveComponent.ValueRO.LinearSpeed);
                
                physicsVelocity.ValueRW.ApplyLinearImpulse(in physicsMass.ValueRW, moveComposite3D);

                if (Input.GetKeyDown(KeyCode.Alpha1))
                    shootAspect.SwitchToWeaponInSlot(0);
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    shootAspect.SwitchToWeaponInSlot(1);
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    shootAspect.SwitchToWeaponInSlot(2);
            }
        }

        public int CurrentActiveWeaponSlot
        {
            get
            {
                EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAspect<ShootAspect>().WithAll<PlayerTag>();
                EntityQuery query = entityQueryBuilder.Build(entityManager);
                var entities = query.ToEntityArray(Allocator.Temp);
                if (entities.Length == 0)
                    return -1;

                ShootAspect shootAspect = entityManager.GetAspect<ShootAspect>(entities[0]);
                return shootAspect.GetWeaponSlot();
            }
            set
            {
                EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAspect<ShootAspect>().WithAll<PlayerTag>();
                EntityQuery query = entityQueryBuilder.Build(entityManager);
                var entities = query.ToEntityArray(Allocator.Temp);
                if (entities.Length == 0)
                    return;

                ShootAspect shootAspect = entityManager.GetAspect<ShootAspect>(entities[0]);
                shootAspect.SwitchToWeaponInSlot(value);
            }
        }
    }
}