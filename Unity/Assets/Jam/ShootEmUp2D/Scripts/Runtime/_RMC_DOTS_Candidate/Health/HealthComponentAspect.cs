using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.Health
{
    readonly partial struct HealthComponentAspect : IAspect
    {
        //  Properties ------------------------------------
        public float HealthCurrent => _healthComponentRefRW.ValueRO.HealthCurrent;
        public float HealthMax => _healthComponentRefRW.ValueRO.HealthMax;
        public float HealthMin => _healthComponentRefRW.ValueRO.HealthMin;

        //  Fields ----------------------------------------
        private readonly RefRW<HealthComponent> _healthComponentRefRW;
        private readonly Entity _entity;
        
        [Optional]
        private readonly RefRW<HealthChangeExecuteOnceComponent> _healthChangeExecuteOnceComponentRefRO;
        
        
        //  Methods ---------------------------------------

        public bool HasHealthChangeExecuteOnceComponent()
        {
            return _healthChangeExecuteOnceComponentRefRO.IsValid;
        }

        public void ProcessHealthChangeExecuteOnceComponent()
        {
           if (!HasHealthChangeExecuteOnceComponent())
           {
               return;
           }
           _healthComponentRefRW.ValueRW.HealthCurrent += _healthChangeExecuteOnceComponentRefRO.ValueRO.HealthChangeBy;
           if (_healthComponentRefRW.ValueRW.HealthCurrent < _healthComponentRefRW.ValueRW.HealthMin)
           {
               _healthComponentRefRW.ValueRW.HealthCurrent = _healthComponentRefRW.ValueRW.HealthMin;
           }
           if (_healthComponentRefRW.ValueRW.HealthCurrent > _healthComponentRefRW.ValueRW.HealthMax)
           {
               _healthComponentRefRW.ValueRW.HealthCurrent = _healthComponentRefRW.ValueRW.HealthMax;
           }
        }


        public void RemoveHealthChangeExecuteOnceComponent(EntityCommandBuffer ecb)
        {
            if (!HasHealthChangeExecuteOnceComponent())
            {
                return;
            }
            ecb.RemoveComponent<HealthChangeExecuteOnceComponent>(_entity);
        }

        public void HealthChangeBy(EntityCommandBuffer ecb, float healthChangeBy)
        {
            ecb.AddComponent<HealthChangeExecuteOnceComponent>(_entity, HealthChangeExecuteOnceComponent.FromHealthChangeBy(healthChangeBy));

        }
    }
}