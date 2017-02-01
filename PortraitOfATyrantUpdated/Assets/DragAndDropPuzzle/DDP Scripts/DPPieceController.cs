using UnityEngine;
using System.Collections;

public class DPPieceController : MonoBehaviour {

    [HideInInspector]
    public DPPuzzleController DPPuzzleControllerInstance;

    [HideInInspector]
    public string OrigionalName = "Piece";


    public bool _isCollidingWithPiece = false;
    
    private bool _isStartupCollision = true;
    private GameObject _collidingPieceInstance = null;


    public bool IsCollidingWithPiece
    {
        get { return _isCollidingWithPiece; }
    }

    public GameObject CollidingPieceInstance
    {
        get { return _collidingPieceInstance; }
    }



    void OnTriggerEnter(Collider Obj)
    {
        if (Obj.name.ToLower().Contains("piecereplacement") && !_isStartupCollision)
        {
            _isCollidingWithPiece = true;
            _collidingPieceInstance = Obj.transform.parent.gameObject;
        }

        if (_isStartupCollision)
            _isStartupCollision = false;

    }

    void OnTriggerExit(Collider Obj)
    {

        if (Obj.name.ToLower().Contains("piecereplacement") && !_isStartupCollision)
        {
            if (_collidingPieceInstance != null)
            {
                if (Obj.transform.parent.name.ToLower() == _collidingPieceInstance.name.ToLower())
                {
                    //Debug.Log("Collision exit from " + Obj.transform.parent.name);
                    _isCollidingWithPiece = false;
                    _collidingPieceInstance = null;
                }
            }

        }

        if (_isStartupCollision)
            _isStartupCollision = false;

    }


}
