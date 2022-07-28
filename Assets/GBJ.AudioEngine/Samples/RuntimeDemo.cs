using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBJ.AudioEngine
{
    public class RuntimeDemo : MonoBehaviour
    {
        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            Audio.Play("Demo Event");
        }
    }
}