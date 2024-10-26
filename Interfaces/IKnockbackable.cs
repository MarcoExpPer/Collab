
using UnityEngine;

public interface IKnockbackable
{
    //Returns true if the effect has been pplied
    public void ApplyImpulse(Vector3 impulseDir, float force);

    public void ForceEndKnockback();
}