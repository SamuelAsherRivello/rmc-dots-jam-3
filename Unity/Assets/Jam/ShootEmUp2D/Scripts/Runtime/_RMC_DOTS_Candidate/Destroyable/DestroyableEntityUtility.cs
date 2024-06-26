﻿using RMC.DOTS.Systems.DestroyEntity;
using Unity.Entities;
using Unity.Transforms;

namespace RMC.DOTS.Systems.Destroyable
{
    
    public static class DestroyableEntityUtility
    {
        
        //  Methods ---------------------------------------
               
        /// <summary>
        /// The DestroyEntityComponent is used for two things
        /// destroy now, or destroy after x seconds.
        ///
        /// TODO: Move the functionality below (or something similar) into its rmc dots source
        /// </summary>
        public static void DestroyEntityImmediately(
            EntityCommandBuffer ecb, 
            ComponentLookup<DestroyEntityComponent> destroyEntityComponentLookup, 
            Entity entity)
        {
            DestroyEntity(ecb, destroyEntityComponentLookup, 0, entity);
        }
        
        public static void DestroyEntity(
            EntityCommandBuffer ecb, 
            ComponentLookup<DestroyEntityComponent> destroyEntityComponentLookup, 
            float timeTillDestroyInSeconds,
            Entity entity)
        {
            if (destroyEntityComponentLookup.HasComponent(entity))
            {
                destroyEntityComponentLookup.GetRefRW(entity).ValueRW.TimeTillDestroyInSeconds = timeTillDestroyInSeconds;
            }
            else
            {
                ecb.AddComponent<DestroyEntityComponent>(entity, new DestroyEntityComponent
                {
                    TimeTillDestroyInSeconds = timeTillDestroyInSeconds
                });
            }
        }
    }
}