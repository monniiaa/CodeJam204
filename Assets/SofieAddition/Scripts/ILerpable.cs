using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILerpable
{
    IEnumerator LinearInterpolation(float startvalue, float endvalue, float lerpDuration);

}
   
