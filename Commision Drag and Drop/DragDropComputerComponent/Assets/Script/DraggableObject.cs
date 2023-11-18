using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField]
    private string triggerTag;
    [SerializeField]
    bool isMobo;
    public bool isDragging = false;
    private bool isTouchingSomething = false;
    private Vector3 touchPositionOffset;
    private Vector3 tempPos;
    static bool isMoboDrag = false;
    public bool isDrop { get; private set; }
    private void Start()
    {
    }
    public bool getMoboDrag()
    {
        return isMoboDrag;
    }
    public void returnPos()
    {
        transform.position = tempPos;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnTouchStart(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            OnTouchMove(Input.mousePosition);
        }
        else if (isDragging)
        {
            OnTouchEnd();
        }
    }

    private void OnTouchStart(Vector2 touchPosition)
    {
        if (IsTouchOverObject(touchPosition))
        {
            if (isMobo) isMoboDrag = true;
            else isMoboDrag = false;

            isDrop = false;
            isDragging = true;
            tempPos = gameObject.transform.position;
            touchPositionOffset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
        }
    }

    private void OnTouchMove(Vector2 touchPosition)
    {
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z)) + touchPositionOffset;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

            isTouchingSomething = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isDragging)
            isTouchingSomething = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isDragging)
        {
            isTouchingSomething = false;
        }
    }

    private void OnTouchEnd()
    {
        isDrop = true;
        isDragging = false;
        isMoboDrag = false;
    }

    private bool IsTouchOverObject(Vector2 touchPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPosition), Vector2.zero);
        return (hit.collider != null && hit.collider.gameObject == gameObject);
    }
}
