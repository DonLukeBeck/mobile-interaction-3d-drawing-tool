using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class UserTesting : MonoBehaviour
{
    [SerializeField] BrushController _brushController;
    public GameObject UI;
    public TMP_Text TimerText;
    public TMP_Text TaskText;
    [FormerlySerializedAs("Gestures")] public GestureController.Gesture[] GesturesToTest;
    [FormerlySerializedAs("Shapes")] public string[] ShapesToTest;

    private int[] _gesturePerms;
    private int[] _shapePerms;
    private int _gesturePermIdx = 0;
    private int _shapePermIdx = 0;
    private string _currGesture, _currShape;
    private bool _startTimer = false;
    private Stopwatch _timer;

    private void Start()
    {
        Debug.developerConsoleVisible = true;
        _timer = new Stopwatch();
        UpdateTask();
        GenerateGesturePermutations();
        GenerateShapePermutations();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startTimer = !_startTimer;
            if (_startTimer)
            {
                _timer = new Stopwatch();
                _timer.Start();
            }
            else
            {
                _timer.Stop();
                TimerText.text = _timer.Elapsed.ToString();
                ScreenCapture.CaptureScreenshot("Screenshots/" + _currGesture + "_" + _currShape + ".png");
            }

            UI.SetActive(_startTimer);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateShapePermutations();
            _gesturePermIdx++;
            _gesturePermIdx %= _gesturePerms.Length;
            _currGesture = GesturesToTest[_gesturePerms[_gesturePermIdx]].ToString();
            UpdateTask();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _shapePermIdx++;
            _shapePermIdx %= _shapePerms.Length;
            _currShape = ShapesToTest[_shapePerms[_shapePermIdx]];
            UpdateTask();
        }
    }

    void UpdateTask()
    {
        TaskText.text = "Use [" + _currGesture +"] to draw a [" + _currShape + "]";
    }

    int[] GeneratePermutations(int size)
    {
        int[] indices = new int[size];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = i;
        }

        Random rnd = new Random();
        return indices.OrderBy(x => rnd.Next()).ToArray();
    }

    void GenerateGesturePermutations()
    {
        _gesturePerms = GeneratePermutations(GesturesToTest.Length);
    }
    
    void GenerateShapePermutations()
    {
        _shapePerms = GeneratePermutations(ShapesToTest.Length);
    }
}