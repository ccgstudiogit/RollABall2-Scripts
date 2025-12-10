using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(MeshCollider))]
public class VictoryZone : MonoBehaviour, ITriggerable
{
    public static event Action OnPlayerWin;

    [Header("References")]
    [SerializeField] private ParticleSystem[] confettis;
    private MeshCollider col;

#if UNITY_EDITOR
    [Header("Gizmos Settings")]
    [SerializeField] private bool enableGizmos = true;
    [SerializeField] private Color gizmosColor = Color.blue;
#endif

    private void Awake()
    {
        if (confettis.Length == 0)
            Debug.LogWarning("VictoryZone has no confetti references. No confetti will play on player victory.");
    }

    public void Trigger()
    {
        OnPlayerWin?.Invoke();

        if (confettis.Length > 0)
        {
            foreach (ParticleSystem confetti in confettis)
            {
                confetti.Play();
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        col = GetComponent<MeshCollider>();
    }

    private void OnDrawGizmos()
    {
        if (!enableGizmos || col == null)
            return;

        Gizmos.color = gizmosColor;
        Gizmos.DrawMesh(col.sharedMesh, transform.position, transform.rotation, transform.localScale);
    }
#endif
}
