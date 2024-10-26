
using UnityEngine;

public class PickAndThrowInteraction : InteractionSource
{
    private bool _isItemGrabbed = false;
    //If the player is not moving, allow the player to drop the item on front of it
    public bool allowDrop = true;
    
    private Rigidbody _rb;
    private Transform _parentTransform;
    private PickAndThrowData _data;
    public override void Awake()
    {
        base.Awake();
        LockInteractionObject = true;
        _rb = GetComponentInParent<Rigidbody>();
        _rb.useGravity = false;
        _parentTransform = transform.parent.transform;
    }

    public override void Start()
    {
        base.Start();
        _data = GameManager.Instance.playerData.PickAndThrowData;
    }

    public override void ExecuteInteraction(InteractionController controller)
    {
        this.Controller = controller;
        //GameManager.Instance.inputManager.groundedInput.InteractInput = false;
        
        if (_isItemGrabbed)
        {
            _isItemGrabbed = false;
            EndGrab();
        }
        else
        {
            _isItemGrabbed = true;
            GrabItem();
        }
    }

    private void EndGrab()
    {
        Controller.EndInteraction();
        
        _parentTransform.parent = null;
        _rb.isKinematic = false;

        if (!allowDrop || GameManager.Instance.inputManager.groundedInput.CurrentMoveInput != Vector2.zero)
        {
            ThrowItem();
        }
        else
        {
            DropItem();
        }
    }
    
    private void ThrowItem()
    {
        Vector3 forceDirection = Quaternion.AngleAxis(-_data.throwAngle, _parentTransform.right) * _parentTransform.forward;
        _rb.AddForce(forceDirection * _data.throwStrength);
    }
    
    private void DropItem()
    {
        _parentTransform.position += (_parentTransform.forward * _data.dropDistanceMultiplier);
    }
    

    public void GrabItem()
    {
        _parentTransform.position = Controller.grabItemsTransform.position;
        _parentTransform.rotation = Controller.grabItemsTransform.rotation;
        _parentTransform.parent = Controller.grabItemsTransform;
        _rb.isKinematic = true;
    }
    
    void FixedUpdate() {

        if (!_rb.isKinematic)
        {
            _rb.AddForce(Physics.gravity * (_rb.mass * _rb.mass));
        }
    }
    
    public override void ShowInteractionWidget(Sprite sprite)
    {
        if (SpriteRenderer)
        {
            SpriteRenderer.sprite = sprite;
            SpriteRenderer.enabled = true;
        }
    }

    public override bool CanBeInteractedWith()
    {
        bool canBeInteractedWith = base.CanBeInteractedWith();
        if (!canBeInteractedWith) return false;
        
        return true;
    }
}
