using UnityEngine;
using System.Collections;

public static class CoroutineUtil {
     public static IEnumerator WaitForRealSeconds(float time)
     {
         float start = Time.realtimeSinceStartup;
         while (Time.realtimeSinceStartup < start + time)
         {
             yield return null;
         }
     }
 
}

// USAGE
// private IEnumerator MyCoroutine()
// {
//     // Do stuff
// 
//     yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(DURATION));
// 
//     // Do other stuff
// }