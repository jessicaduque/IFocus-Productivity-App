using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimeScrollPart : MonoBehaviour, IEndDragHandler
{
    [SerializeField] UnityEngine.UI.LoopVerticalScrollRect scrollRect;
    private Transform _content;
    private int _currentValue;

    void Awake()
    {
        _content = scrollRect.content;
    }
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.01f);
        scrollRect.ScrollToCellWithinTime(0, 0.01f);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(scrollRect.GetLastItem(out float test));
        scrollRect.ScrollToCellWithinTime(scrollRect.GetLastItem(out float offset), 0.1f);
        _currentValue = scrollRect.GetLastItem(out offset);
    }
}
