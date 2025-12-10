using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundEffectSO : ScriptableObject
{
    [Header("Sound Effect Settings")]
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Vector2 volume = new Vector2(0.9f, 1);
    [SerializeField] private Vector2 pitch = new Vector2(0.8f, 1.2f);
    [SerializeField] private SoundClipPlayOrder playOrder;
    private int playIndex;

    private enum SoundClipPlayOrder
    {
        RANDOM,
        IN_ORDER,
        REVERSE
    }

    public AudioSource Play(AudioSource audioSourceParam = null)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning($"{name} has no audio clips.");
            return null;
        }

        var source = audioSourceParam;
        if (source == null)
        {
            var obj = new GameObject("Sound", typeof(AudioSource));
            source = obj.GetComponent<AudioSource>();
        }

        source.clip = GetAudioClip();
        source.volume = Random.Range(volume.x, volume.y);
        source.pitch = Random.Range(pitch.x, pitch.y);

        source.Play();

        if (source.name == "Sound")
            Destroy(source.gameObject, source.clip.length / source.pitch);

        return source;
    }

    private AudioClip GetAudioClip()
    {
        // Get current clip
        var clip = clips[playIndex >= clips.Length ? 0 : playIndex];

        // Find next clip
        switch (playOrder)
        {
            case SoundClipPlayOrder.RANDOM:
                playIndex = Random.Range(0, clips.Length);
                break;
            case SoundClipPlayOrder.IN_ORDER:
                playIndex = (playIndex + 1) % clips.Length;
                break;
            case SoundClipPlayOrder.REVERSE:
                playIndex = (playIndex + clips.Length - 1) % clips.Length;
                break;
        }

        // Return clip
        return clip;
    }
}
