using System.Collections;
using UnityEngine;

public class TimeScrollPart : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.LoopVerticalScrollRect scrollRect;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.01f);
        scrollRect.ScrollToCellWithinTime(0, 0.01f);
    }
    
    

}
