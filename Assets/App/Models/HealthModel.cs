using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class HealthModel : NetworkBehaviour {
    [SyncVar(hook = "HealthChanged")]
    private int _health;
    public int Health
    {
        get { return _health; }
        private set { _health = value; }
    }

    public class HealthChange : UnityEvent<int>{}
    public HealthChange OnHealthChanged = new HealthChange();
    
    public void TakeDamage(int amount)
    {
        Adjust(-amount);
    }

    public void Revive(int amount)
    {
        Adjust(amount - Health);
    }

    public void Restore(int amount)
    {
        Adjust(amount);
    }

    private void HealthChanged(int newHealth)
    {
        OnHealthChanged.Invoke(newHealth);
    }

    private void Adjust(int amount)
    {
        if (isServer)
        {
            Health += amount;
        }
    }
}
