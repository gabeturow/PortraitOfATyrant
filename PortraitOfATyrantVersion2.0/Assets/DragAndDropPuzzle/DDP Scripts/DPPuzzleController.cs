using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System;
using System.Collections;
using System.Collections.Generic;


public class DPPuzzleController : MonoBehaviour {


#region "Public Variables"

    public Texture2D PuzzleImage;
    public Texture2D BackgroundImage;
    
    [Space(15)]
    [Range(0.1f, 1)]
    public float AnimationSpeed = 1f;

    
    [Range(1, 10)]
    public int TotalRows = 1;
    
    [Range(3, 10)]
    public int TotalCols = 3;

    [Space(10)]
    public int TimeToFinishInSeconds = 60;

    [Space(10)]
    public Text RemainingTimeDisplay;
    public Text NumberOfAttemptsDisplay;
    public Image PeekImage;
    public Button btnHidePeekImage;

    
    [Space(15)]
    //Music and sfx variables
    public AudioClip PieceSlide;
    public AudioClip PuzzleCompletionSound;
    public AudioClip TimeUpSound;
    public AudioClip BackgroundMusic;


    [Space(20)]
    public UnityEvent OnPieceMovementComplete;
    public UnityEvent OnPuzzleSolved;
    public UnityEvent OnTimeUp;

#endregion
    
#region "Private Variables"

    private GameObject[][] _pieceInstances;
    private Vector3[][] _piecePositions;        //Holds origional piece positions

    private GameObject _dpPuzzlePiece;

    private const float PiecesSpacing = 0.01f;

    private const float PieceWidthInWorld = 1f;

    private int _currentNumberOfAttempts = 0;
    private float _currentTime = 0f;
    private bool _isTimeUp = false;

    private GameObject _currentHoldingPiece = null;
    private Vector3 _holdingPieceGrabPosition = Vector3.zero;

    //Holds instance for audio manager
    private DDP.AudioManager _audioManager;

    private GameObject _currentPieceToMove = null;
    private int _pieceFromRow = -1;
    private int _pieceFromCol = -1;

    private bool _piecePickingAllowed = true; //Used to check if piece is animating towards a position
    private bool _isShuffling = false;

    private Rect _frameRectangle;

    private const string PieceNamePrefix = "DPPiece";

#endregion



    void Awake()
    {
        _audioManager = GetComponent<DDP.AudioManager>();
        _currentTime = TimeToFinishInSeconds;
        
        PeekImage.sprite = Sprite.Create(PuzzleImage, new Rect(0, 0, PuzzleImage.width, PuzzleImage.height), new Vector2(0.5f, 0.5f));
    }

    void Start()
    {
        if (BackgroundMusic != null)
            _audioManager.PlayMusic(BackgroundMusic);

        //Find piece gameobject
        _dpPuzzlePiece = gameObject.transform.FindChild("PuzzlePieceDP").gameObject;

        //Assign image to puzzle piece instance
        _dpPuzzlePiece.GetComponent<Renderer>().material.mainTexture = PuzzleImage;

        Transform BackgroundImageObj = transform.FindChild("BackgroundImage");

        //Seperate from cam for cam resizing to adjust whole puzzle in cam view
        Transform parentCamTransform = gameObject.transform.parent;
        Camera parentCam = parentCamTransform.GetComponent<Camera>();
        gameObject.transform.parent = null;


#region "Initialize pieces and position them in place"

        _pieceInstances = new GameObject[TotalRows][];
        _piecePositions = new Vector3[TotalRows][];

        int PieceWidth = PuzzleImage.width / TotalCols;
        int PieceHeight = PuzzleImage.height / TotalRows;

        float PieceHeightWidthRatio = (float)PieceHeight / (float)PieceWidth;

        for (int Rowtrav = 0; Rowtrav < TotalRows; Rowtrav++)
        {
            _pieceInstances[Rowtrav] = new GameObject[TotalCols];
            _piecePositions[Rowtrav] = new Vector3[TotalCols];

            for (int Coltrav = 0; Coltrav < TotalCols; Coltrav++)
            {
                //Instantiate
                _pieceInstances[Rowtrav][Coltrav] = GameObject.Instantiate(_dpPuzzlePiece) as GameObject;

                SetPuzzlePieceUV(_pieceInstances[Rowtrav][Coltrav], Coltrav, Rowtrav,
                    TotalRows, TotalCols);


                //Name this piece instance
                _pieceInstances[Rowtrav][Coltrav].name = ArrIndexToPieceName(Rowtrav, Coltrav);
                _pieceInstances[Rowtrav][Coltrav].GetComponent<DPPieceController>().OrigionalName = 
                    _pieceInstances[Rowtrav][Coltrav].name;

                //Make child of main gameobject
                _pieceInstances[Rowtrav][Coltrav].transform.parent = gameObject.transform;

                //Resize
                _pieceInstances[Rowtrav][Coltrav].transform.localScale = new Vector3(PieceWidthInWorld,
                                            PieceWidthInWorld * PieceHeightWidthRatio, 1);

                //Place in position
                float PositionX = //gameObject.transform.position.x + 
                    (Coltrav * PieceWidthInWorld) + (Coltrav * PiecesSpacing);
                float PositionY = //gameObject.transform.position.y + 
                    (Rowtrav * PieceWidthInWorld * PieceHeightWidthRatio) + (Rowtrav * PiecesSpacing * 1.02f);

                Vector3 CalcPosition = new Vector3(PositionX, PositionY, 0);

                _pieceInstances[Rowtrav][Coltrav].transform.localPosition = CalcPosition;
                _piecePositions[Rowtrav][Coltrav] = CalcPosition;
                
                //Enable instance
                _pieceInstances[Rowtrav][Coltrav].SetActive(true);


                _pieceInstances[Rowtrav][Coltrav].GetComponentInChildren<DPPieceController>().DPPuzzleControllerInstance = this;

            }
        }

#endregion

        //Resize camera to display whole puzzle in camera view
        float widthToBeSeen = TotalCols * PieceWidthInWorld + (PiecesSpacing * (TotalCols - 1));
        float heightToBeSeen = TotalRows * PieceWidthInWorld * PieceHeightWidthRatio + (PiecesSpacing * (TotalRows - 1));

        parentCam.orthographicSize = widthToBeSeen > heightToBeSeen ? widthToBeSeen * 2.4f : heightToBeSeen * 2.4f;

        //Position camera in centre of puzzle
        float CalcCameraX = ((_pieceInstances[1][TotalCols - 1].transform.position.x -
            _pieceInstances[1][0].transform.position.x) / 2) + _pieceInstances[1][0].transform.position.x;
        float CalcCameraY = ((_pieceInstances[TotalRows - 1][0].transform.position.y -
            _pieceInstances[0][0].transform.position.y) / 2) + _pieceInstances[0][0].transform.position.y;

        parentCamTransform.position = new Vector3(CalcCameraX, CalcCameraY, _pieceInstances[1][0].transform.position.z - 3);

        if (BackgroundImage != null)
        {
            //Get background image object
            BackgroundImageObj.GetComponent<Renderer>().material.mainTexture = BackgroundImage;
            BackgroundImageObj.transform.parent = null;

            //Resize Background
            float xScaleOfFrame = (Camera.main.orthographicSize * 2f * Screen.width) / Screen.height;
            float yScaleOfFrame = Camera.main.orthographicSize * 2f;

            BackgroundImageObj.transform.localScale = new Vector3(xScaleOfFrame, yScaleOfFrame, 1);
            BackgroundImageObj.transform.position = new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y, BackgroundImageObj.transform.position.z);
        }
        else
            BackgroundImageObj.gameObject.SetActive(false);


        //Shuffle pieces
        ShufflePuzzle();

    }

    void Update()
    {
        UpdateUI();

        if (Input.GetMouseButtonDown(0) && _currentPieceToMove == null && _piecePickingAllowed)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.transform.name.Contains("Piece"))
                {
                    _currentHoldingPiece = hit.collider.transform.gameObject;
                    _holdingPieceGrabPosition = _currentHoldingPiece.transform.position;

                    _currentHoldingPiece.transform.position = new Vector3(_currentHoldingPiece.transform.position.x,
                        _currentHoldingPiece.transform.position.y, _currentHoldingPiece.transform.position.z - 0.1f);
                }
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_currentHoldingPiece != null)
            {
                _currentHoldingPiece.transform.position = new Vector3(_currentHoldingPiece.transform.position.x,
                        _currentHoldingPiece.transform.position.y, _currentHoldingPiece.transform.position.z + 0.1f);

                DPPieceController pieceControllerInstance = _currentHoldingPiece.GetComponentInChildren<DPPieceController>();

                if (pieceControllerInstance.IsCollidingWithPiece)
                {
                    //Interchange piece names
                    string CollidingPieceName = pieceControllerInstance.CollidingPieceInstance.name;
                    pieceControllerInstance.CollidingPieceInstance.name = _currentHoldingPiece.name;
                    _currentHoldingPiece.name = CollidingPieceName;

                    //Move pieces to each others position
                    Vector3 CollidingPiecePosition = pieceControllerInstance.CollidingPieceInstance.transform.position;
                    Vector3 HoldingPiecePosition = _currentHoldingPiece.transform.position;

                    InterchangePieces(_currentHoldingPiece, CollidingPiecePosition,
                        pieceControllerInstance.CollidingPieceInstance, _holdingPieceGrabPosition);
                }
                else
                {
                    StartCoroutine(MovePieceToPosition(_currentHoldingPiece, _holdingPieceGrabPosition));
                }

                 _holdingPieceGrabPosition = Vector3.zero;
                _currentHoldingPiece = null;
            }
        }


        if (_currentHoldingPiece != null)
        {
            float MousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float MousePositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

            _currentHoldingPiece.transform.position = new Vector3(MousePositionX, MousePositionY,
                                                _currentHoldingPiece.transform.position.z);
        }

        if (!_isTimeUp)
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                OnTimeUp.Invoke();
                _isTimeUp = true;
                _piecePickingAllowed = false;
                _audioManager.PlaySFXSound(TimeUpSound);
            }
        }
    }




    public Vector2 PieceNameToArrIndex(string PieceName)
    {
        Vector2 ResultedIndex = new Vector2(-1, -1);

        if (PieceName.Contains(PieceNamePrefix))
        {
            string Temp = PieceName.Remove(0, PieceNamePrefix.Length);
            string[] SplitInts = Temp.Split(new char[] { '_' });

            ResultedIndex.x = System.Convert.ToInt32(SplitInts[0]);
            ResultedIndex.y = System.Convert.ToInt32(SplitInts[1]);
        }

        return ResultedIndex;
    }

    /// <summary>
    /// Merges provided row and column number to form piece name
    /// </summary>
    /// <param name="Row">Row number to be inserted in name . Value should be greate then -1</param>
    /// <param name="Col">Col number to be inserted in name . Value should be greate then -1</param>
    /// <returns>Returns piece name created using provided position</returns>
    public string ArrIndexToPieceName(int Row, int Col)
    {
        return PieceNamePrefix + Row.ToString() + "_" + Col.ToString();
    }

    /// <summary>
    /// Randomly shuffle pieces in instance holder variable and in world
    /// </summary>
    private void ShufflePuzzle()
    {
        _isShuffling = true;
        float ActualAnimSpeed = AnimationSpeed;
        AnimationSpeed = 10f;

        int TotalPieces = TotalRows * TotalCols;
        int PiecesToInterChange = TotalPieces / 2;

        GameObject[] pInstances = new GameObject[TotalPieces];

        //Disable colliders for all pieces and get all pieces in a single dimensional array
        for (int RowTrav = 0, counter = 0; RowTrav < TotalRows; RowTrav++, counter++)
        {
            for (int ColTrav = 0; ColTrav < TotalCols; ColTrav++, counter++)
            {
                _pieceInstances[RowTrav][ColTrav].GetComponent<MeshCollider>().enabled = false;
                _pieceInstances[RowTrav][ColTrav].transform.GetComponentInChildren<BoxCollider>().enabled = false;

                pInstances[counter] = _pieceInstances[RowTrav][ColTrav];
            }

            counter--;
        }


        List<GameObject> tempHalfObjs = new List<GameObject>();
        //Get half pieces for interchange
        for (int i = TotalPieces - PiecesToInterChange; i < TotalPieces; i++)
            tempHalfObjs.Add(pInstances[i]);


        //Change piece positions to random positions and change names
        int loopCount = tempHalfObjs.Count;
        for (int i = 0; i < loopCount; i++)
        {
            int SelectedRandomPieceIndex = UnityEngine.Random.Range(0, tempHalfObjs.Count);
            Vector3 Piece1Position = pInstances[i].transform.position;
            Vector3 Piece2Position = tempHalfObjs[SelectedRandomPieceIndex].transform.position;

            //Interchange piece names
            string TempName = pInstances[i].name;
            pInstances[i].name = tempHalfObjs[SelectedRandomPieceIndex].name;
            tempHalfObjs[SelectedRandomPieceIndex].name = TempName;

            InterchangePieces(pInstances[i], Piece2Position, tempHalfObjs[SelectedRandomPieceIndex], Piece1Position);

            tempHalfObjs.RemoveAt(SelectedRandomPieceIndex);
        }
        


        //Re-enable colliders for all pieces
        for (int RowTrav = 0; RowTrav < TotalRows; RowTrav++)
        {
            for (int ColTrav = 0; ColTrav < TotalCols; ColTrav++)
            {
                _pieceInstances[RowTrav][ColTrav].GetComponent<MeshCollider>().enabled = true;
                _pieceInstances[RowTrav][ColTrav].transform.GetComponentInChildren<BoxCollider>().enabled = true;
            }
        }

        AnimationSpeed = ActualAnimSpeed;
        _isShuffling = false;
    }

    private void InterchangePieces(GameObject Piece1, Vector3 TargetPosPiece1, GameObject Piece2, Vector3 TargetPosPiece2)
    {
        
        StartCoroutine(MovePieceToPosition(Piece1, TargetPosPiece1));
        StartCoroutine(MovePieceToPosition(Piece2, TargetPosPiece2));

        _currentNumberOfAttempts++;

        if (IsPuzzleSolved())
        {
            OnPuzzleSolved.Invoke();

            _audioManager.PlaySFXSound(PuzzleCompletionSound);
            _piecePickingAllowed = false;

        }
        else if (!_isShuffling)
        {
            _audioManager.PlaySFXSound(PieceSlide);
        }
    }

    private IEnumerator MovePieceToPosition(GameObject PieceToMove,  Vector3 TargetPosition)
    {
        _piecePickingAllowed = false;

        Vector3 Velocity = Vector3.zero;

        while (Vector3.Distance(PieceToMove.transform.position, TargetPosition) > 0.2f)
        {
            //PieceToMove.transform.position = Vector3.MoveTowards(PieceToMove.transform.position, TargetPosition, Time.deltaTime * AnimationSpeed);
            PieceToMove.transform.position = Vector3.SmoothDamp(PieceToMove.transform.position, TargetPosition, ref Velocity, AnimationSpeed);
            _piecePickingAllowed = false;
            yield return new WaitForFixedUpdate();
        }

        PieceToMove.transform.position = TargetPosition;

        _piecePickingAllowed = true;
    }

    /// <summary>
    /// Checks whether puzzle is solved.
    /// </summary>
    /// <returns>Returns true if puzzle is solved</returns>
    private bool IsPuzzleSolved()
    {
        for (int Rowtrav = 0; Rowtrav < TotalRows; Rowtrav++)
        {
            for (int Coltrav = 0; Coltrav < TotalCols; Coltrav++)
                if (_pieceInstances[Rowtrav][Coltrav].name != _pieceInstances[Rowtrav][Coltrav].GetComponent<DPPieceController>().OrigionalName)
                    return false;
        }

        return true;
        
    }

    private void UpdateUI()
    {
        if (NumberOfAttemptsDisplay != null)
        {
            string Temp = _currentNumberOfAttempts.ToString();
            Temp = Temp.Length == 1 ? "00" + Temp : Temp.Length == 2 ? "0" + Temp : Temp;
            NumberOfAttemptsDisplay.text = Temp;
        }

        if (RemainingTimeDisplay != null)
        {
            TimeSpan Temp = TimeSpan.FromSeconds(_currentTime);
            RemainingTimeDisplay.text = Temp.Minutes.ToString() + " : " + Temp.Seconds.ToString();
        }
    }

    /// <summary>
    /// Sets pieces instance uvs according to their position in puzzle
    /// </summary>
    /// <param name="Piece">Piece instance for uv setting</param>
    /// <param name="PieceXPositioninPuzzle">X Position of piece in puzzle</param>
    /// <param name="PieceYPositionInPuzzle">Y Posiiton of piece in puzzle</param>
    /// <param name="TotalRowsInPuzzle">No of rows in puzzle / total pieces in col</param>
    /// <param name="TotalColsInPuzzle">No of cols in puzzle / total pieces in rows</param>
    private static void SetPuzzlePieceUV(GameObject Piece, int PieceXPositioninPuzzle,
        int PieceYPositionInPuzzle, int TotalRowsInPuzzle, int TotalColsInPuzzle)
    {
        float PieceWidth = 1f / (float)TotalColsInPuzzle;
        float PieceHeight = 1f / (float)TotalRowsInPuzzle;

        //Set uv for this piece
        float StartX = PieceWidth * (float)PieceXPositioninPuzzle + 0.0012f * (float)TotalColsInPuzzle;
        float StartY = PieceHeight * (float)PieceYPositionInPuzzle + 0.0012f * (float)TotalRowsInPuzzle;
        float EndX = StartX + PieceWidth - 0.0024f * (float)TotalColsInPuzzle;
        float EndY = StartY + PieceHeight - 0.0024f * (float)TotalRowsInPuzzle;

        Vector2[] UVPoints = {new Vector2(StartX, StartY),
                                      new Vector2(EndX, EndY),
                                      new Vector2(EndX, StartY),
                                      new Vector2(StartX, EndY)
                                     };
        Piece.GetComponent<MeshFilter>().mesh.uv = UVPoints;

    }



    /// <summary>
    /// Resets puzzle to start
    /// </summary>
    public void Reset()
    {
        //Take pieces to their origional positions and change their names to origional names
        for (int RowTrav = 0; RowTrav < TotalRows; RowTrav++)
        {
            for (int ColTrav = 0; ColTrav < TotalCols; ColTrav++)
            {
                _pieceInstances[RowTrav][ColTrav].name = _pieceInstances[RowTrav][ColTrav].GetComponent<DPPieceController>().OrigionalName;

                _pieceInstances[RowTrav][ColTrav].transform.localPosition = _piecePositions[RowTrav][ColTrav];
            }
        }

        ShufflePuzzle();

        _piecePickingAllowed = true;
        _currentNumberOfAttempts = 0;
        _currentTime = TimeToFinishInSeconds;
        _isTimeUp = false;

        UpdateUI();
    }

    /// <summary>
    /// Called on successfull completion of this puzzle
    /// </summary>
    public void E_OnPuzzleSolved()
    {
        Debug.LogWarning("Puzzle is solved");
    }

    /// <summary>
    /// Called when piece successfully completes its sliding animation
    /// </summary>
    public void E_OnPieceMovementComplete()
    {
        Debug.LogWarning("Movement completed");
    }

    public void OnShowPeekButtonPress()
    {
        PeekImage.gameObject.SetActive(true);
        btnHidePeekImage.gameObject.SetActive(true);
    }

    public void OnHidePeekButtonPress()
    {
        PeekImage.gameObject.SetActive(false);
        btnHidePeekImage.gameObject.SetActive(false);
    }

}
