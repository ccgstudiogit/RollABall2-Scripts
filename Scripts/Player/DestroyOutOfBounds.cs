using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    [SerializeField] private float yBounds = -250f;
    private bool destroyThisObjectCalled;

    private void Update()
    {   
        if (transform.position.y < yBounds)
                DestroyThisObject();
    }

    private void DestroyThisObject()
    {
        if (destroyThisObjectCalled)
            return;
        
        if (TryGetComponent(out AudioListener listener))
        {
            Destroy(listener);
            var obj = new GameObject("NewAudioListener", typeof(AudioListener));
            obj.transform.position = transform.position;
        }

        destroyThisObjectCalled = true;
        Destroy(gameObject);
    }
}
