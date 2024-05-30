using System.Linq;
using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.Cinemachines;
using Unity.Entities;
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
                    CinemachineController = Object.FindObjectsByType<CinemachineController>(FindObjectsSortMode.None).FirstOrDefault()
                });
        }

        protected override void OnUpdate()
        {
            //
        }
    }
}