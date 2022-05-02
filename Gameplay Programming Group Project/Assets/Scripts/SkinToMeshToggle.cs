using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinToMeshToggle : MonoBehaviour
{
    [SerializeField] private Material newMaterial;

    private InteractionReceiver receiver;
    private bool do_once = false;

    private void Awake()
    {
        receiver = GetComponent<InteractionReceiver>();
    }

    private void Update()
    {
        if(!do_once && receiver.signal)
        {
            do_once = true;
            var old_material = GetComponent<Material>();
            if(old_material != null)
            {

            }
            /*GetComponent<SkinnedMeshRenderer>().material = GetComponent<SkinnedMeshRenderer>().materials[1];*/
            GetComponent<SkinnedMeshRenderer>().material = newMaterial;
        }
    }

}
