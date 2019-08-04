using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    #region SerialiseFields ( Value Variables)
    [SerializeField]
    private float _handlerRange = 1;
    [SerializeField]
    private float _deadZone = 0;
    [SerializeField]
    private AxisOptions _axisOptions = AxisOptions.Both;
    [SerializeField]
    private bool _snapX = false;
    [SerializeField]
    private bool _snapY = false;
    [SerializeField]
    private RectTransform _background = null;
    [SerializeField]
    private RectTransform _handle = null;


    #endregion
    #region References ( Reference Variables)
    private RectTransform _baseRect = null;
    private Canvas _canvas;
    private Camera _cam;
    public Vector2 input = Vector2.zero;
    [Header("Player")]
    public GameObject _player;
    public Vector2 _playerDir;
    public float _speed = 4f;

    

    #endregion
    #region Properties
    public float Horizontal
    {
        get { return (_snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; }
    }
    public float Vertical
    {
        get { return (_snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; }
    }
    public Vector2 Direction
    {
        get { return new Vector2(Horizontal, Vertical); }
    }
    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
        {
            return value;
        }
        if (_axisOptions == AxisOptions.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                {
                    return 0;
                }
                else
                {
                    return (value > 0) ? 1 : -1;
                }
            }
            else if (snapAxis == AxisOptions.Vertical)
            {
                if (angle > 67.5f || angle < 112.5f)
                {
                    return 0;
                }
                return value;
            }
            else
            {
                if (value > 0)
                {
                    return 1;
                }
                if (value < 0)
                {
                    return -1;
                }
            }
        }
        return 0;
    }
    public bool SnapX
    {
        get { return _snapX; }
        set { _snapX = value; }
    }
    public bool SnapY
    {
        get { return _snapY; }
        set { _snapY = value; }
    }
    public float HandleRange
    {
        get { return _handlerRange; }
        set { _handlerRange = Mathf.Abs(value); }
    }
    public float DeadZone
    {
        get { return _deadZone; }
        set { _deadZone = Mathf.Abs(value); }
    }
    public AxisOptions AxisOptions
    {
        get { return _axisOptions; }
        set { _axisOptions = value; }
    }
    #endregion
    #region Functions
    protected virtual void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody2D _playerRigid= _player.GetComponent<Rigidbody2D>();
        _playerDir = _playerRigid.velocity;
        HandleRange = _handlerRange;
        DeadZone = _deadZone;
        _baseRect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
        {
            Debug.Log("This script goes on a child in the canvas...NOT ON THE CANVAS");
        }
        Vector2 center = new Vector2(0.5f, 0.5f);
        _background.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = Vector2.zero;
        
    }
   
    void FormatInput()
    {
        if (_axisOptions == AxisOptions.Horizontal)
        {
            input = new Vector2(input.x, 0);
            
        }
        else if (_axisOptions == AxisOptions.Vertical)
        {
            input = new Vector2(0, input.y);
            
        }


    }
    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > _deadZone)
        {
            if (magnitude > 1)
            {
                input = normalised;
               
            }
        }
        else
        {
            input = Vector2.zero;
        }
    }
    protected Vector2 ScreenToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _cam, out localPoint))
        {

            return localPoint - (_background.anchorMax * _baseRect.sizeDelta);
        }

        return Vector2.zero;
    }
    
    #endregion
    #region Interface
    public void OnDrag(PointerEventData eventData)
    {
        _cam = null;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            _cam = _canvas.worldCamera;
        }
        Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, _background.position);
        Vector2 radius = _background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * _canvas.scaleFactor);
        FormatInput();
        HandleInput(input.magnitude, input.normalized, radius, _cam);
        _handle.anchoredPosition = input * radius * _handlerRange;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }
    #endregion

}
public enum AxisOptions
{
    Both,
    Horizontal,
    Vertical
}
