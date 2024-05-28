using System.Linq;
using RMC.DOTS.Samples.Games.ShootEmUp2D;
using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.Cinemachines;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace RMC.DOTS.Systems.Health
{
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    public partial class CinemachineControllerSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<CinemachineControllerSystemAuthoring.CinemachineControllerSystemIsEnabled>();
            RequireForUpdate<CinemachineControllerReferenceComponent>();
            
            var entity = EntityManager.CreateEntity();
            EntityManager.AddComponentData<CinemachineControllerReferenceComponent>(entity,
                new CinemachineControllerReferenceComponent
                {
                    CinemachineController = Object.FindObjectsOfType<CinemachineController>().FirstOrDefault()
                });
        }

        protected override void OnUpdate()
        {
            //
        }
    }
}