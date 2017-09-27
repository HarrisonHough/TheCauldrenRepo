using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnemy : MonoBehaviour {

    [SerializeField]
    private float SecondsToDisappear;
    private Material DeadEnemyMaterial;

    private bool inactive = true;
    public bool Inactive { get { return inactive; } }

    // Use this for initialization
    void Start()
    {
        if(DeadEnemyMaterial == null)
            DeadEnemyMaterial = GetComponent<MeshRenderer>().material;

        DeadEnemyMaterial.SetColor("_TintColor", new Color(255, 255, 255, 255));
        SetInactive();
    }

    public void StartDeathEffect()
    {
        if (!inactive)
        {
            Debug.Log("Dead enemy is not inactive");
            return;
        }
        Debug.Log("Start Death Effect");
        StartCoroutine(DeathEffectCoroutine());
    }

    IEnumerator DeathEffectCoroutine()
    {
        float seconds = SecondsToDisappear;
        while (seconds > 0)
        {
            seconds -= Time.deltaTime;
            DeadEnemyMaterial.SetColor("_TintColor", new Color(255, 255, 255, seconds * 0.25f));
            yield return null;
        }
        SetInactive();
    }

    void SetInactive()
    {
        inactive = true;
        // move out of view
        transform.position = new Vector3(0, -10, 0);
        DeadEnemyMaterial.SetColor("_TintColor", new Color(255, 255, 255, 255));
    }
}
