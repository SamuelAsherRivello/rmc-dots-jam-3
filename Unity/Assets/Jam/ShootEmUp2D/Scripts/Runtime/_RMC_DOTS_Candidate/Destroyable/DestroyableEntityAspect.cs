using RMC.DOTS.Systems.DestroyEntity;
using Unity.Entities;
using Unity.Transforms;

namespace RMC.DOTS.Systems.Destroyable
{
    
    /// <summary>
    /// TODO: Consider to replace the short-lifespan of the DestroyEntityComponent
    /// with an ever-present (per some entities) called DestroyableEntityComponent
    /// </summary>
    readonly partial struct DestroyableEntityAspect : IAspect
    {
        //  Properties ------------------------------------
        
        //This is a hack to have it queryable
        //TODO: Replace this with a new ever-present DestroyableEntityComponent
        //for anything that may EVER be destroyed (now or later)
        readonly RefRW<LocalTransform> Dummy;
        
        [Optional]
        readonly RefRW<DestroyEntityComponent> DestroyEntityComponentRefRW;
        
        //  Methods ---------------------------------------
    }
}