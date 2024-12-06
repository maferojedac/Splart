// Abstract class to be overriden so that different FX can be pooled
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    protected Color _color;
    protected SoundManager _soundManager;

    public virtual void Execute() {
        gameObject.SetActive(true);
    }
    public virtual void Cancel() { 
        gameObject.SetActive(false);
    }

    public void SetSoundManager(SoundManager newManager)
    {
        _soundManager = newManager;
    }

    public virtual void SetColor(Color color) {
        _color = color;
    }

    public virtual void SetPosition(Vector3 position) {
        transform.position = position;    
    }
}
