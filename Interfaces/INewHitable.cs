

using UnityEngine;

public interface INewHitable
{
    //Returns true if the effect has been pplied
    public bool TryHit(HitEffect_SO hitEffectSo, GameObject hitSource);

    //Aux function to apply multiple hit effects
    public bool TryHits(HitEffect_SO[] hitEffectsSo, GameObject hitSource)
    {
        bool result = true;
        for(int i = 0; i < hitEffectsSo.Length; ++i)
        {
            result = TryHit(hitEffectsSo[i], hitSource);
        }

        return result;
    }
}