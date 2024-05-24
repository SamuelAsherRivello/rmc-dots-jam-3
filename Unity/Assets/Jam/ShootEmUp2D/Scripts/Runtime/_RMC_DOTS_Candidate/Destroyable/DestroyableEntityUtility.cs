using RMC.DOTS.Systems.DestroyEntity;
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
            if (destroyEntityComponentLookup.HasComponent(entity))
            {
                destroyEntityComponentLookup.GetRefRW(entity).ValueRW.TimeTillDestroyInSeconds = 0;
            }
            else
            {
                ecb.AddComponent<DestroyEntityComponent>(entity, new DestroyEntityComponent
                {
                    TimeTillDestroyInSeconds = 0
                });
            }
        }
    }
}