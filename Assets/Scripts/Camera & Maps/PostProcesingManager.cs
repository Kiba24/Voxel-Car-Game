using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


public class PostProcesingManager : MonoBehaviour
{   

    public PostProcessVolume volume;
    public LensDistortion lensDistortion;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out lensDistortion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
