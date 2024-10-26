
    using UnityEngine;

    public class DestroyOnHit : MonoBehaviour, INewHitable
    {
        public bool hasReward;
        private SpawnReward spawnReward;
        
        public bool TryHit(HitEffect_SO hitEffectSo, GameObject hitSource)
        {
            if (hitEffectSo.DamageType != EDamageType.InstantDamage)
            {
                return false;
            }
            
            if (hasReward)
            {
                spawnReward = GetComponent<SpawnReward>();
                spawnReward.Spawn();
            }
            else
            {
                Destroy(gameObject);
            }

            return true;
        }
    }
