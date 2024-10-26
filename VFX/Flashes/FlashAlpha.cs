using UnityEngine;

public class FlashAlpha : MonoBehaviour, IEffect
{
    [SerializeField] public Renderer mesh;
  
    public float period = 0.4f;
    public float duration = 0.25f;
    private float currentTime;

    private bool isLooping = true;
    // Update is called once per frame
    void Update()
    {
        currentTime+= Time.deltaTime;
        if (currentTime >= period)
        {
            mesh.enabled = false;

            if (currentTime >= period + duration)
            {
                mesh.enabled = true;
                currentTime -= (period + duration);

                if (!isLooping)
                {
                    enabled = false;
                }
            }
        }
    }

    public void StartEffect()
    {
        currentTime = 0;
        isLooping = true;
        enabled = true;
    }

    public void StopEffect(bool forceStop)
    {
        if (forceStop)
        {
            enabled = false;
        }
        isLooping = false;
    }

    private void OnDisable()
    {
        
        mesh.enabled = true;
        isLooping = false;
    }
}
