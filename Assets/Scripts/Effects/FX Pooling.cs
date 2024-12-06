using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPooling : MonoBehaviour
{
    private Dictionary<GameObject, List<Effect>> effects = new Dictionary<GameObject, List<Effect>>();    // Dynamic pooling

    private SoundManager soundManager;

    public Effect Spawn(GameObject type)
    {
        if (!effects.ContainsKey(type))   // Initialize pool if no key
            effects[type] = new List<Effect>();

        List<Effect> currentList = effects[type];    // grab current pool

        foreach (Effect effect in currentList)
        {
            if (!effect.gameObject.activeSelf)
            {
                effect.Execute();
                return effect;
            }
        }

        GameObject newEffectObj = Instantiate(type);

        Effect newEffect = newEffectObj.GetComponent<Effect>();
        newEffect.SetSoundManager(soundManager);
        newEffect.Execute();

        currentList.Add(newEffect);

        return newEffect;
    }

    public void CancelAllEffects()
    {
        foreach(List<Effect> EffectList in effects.Values)
        {
            foreach (Effect effect in EffectList)
            {
                if(effect.gameObject.activeSelf)
                    effect.Cancel();
            }
        }
    }

    void Awake()
    {
        soundManager = GetComponent<SoundManager>();
    }
}
