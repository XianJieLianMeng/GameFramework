using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(ThrowBomb))]
public class ThrowBombInspector : OdinEditor
{
    private enum PreviewState
    {
        Play,
        Pause,
        Stop
    }

    private ThrowBomb _bomb { get { return target as ThrowBomb; } }

    private PreviewState _previewState = PreviewState.Stop;

    private float _moveTotalTime = 0;

    private float _lastTime = 0;

    private const float DeltaTime = 0.005f;//每个DeltaTime移动的路程画一次线

    private const float AnchorSize = 0.3f;//控制点大小缩放

    private readonly Handles.CapFunction sphereCap = Handles.SphereHandleCap;//绘制球形控制柄

    protected override void OnEnable()
    {
        base.OnEnable();
        _bomb.UpdateOriginalPosition();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (EditorApplication.isPlaying)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play"))
        {
            _previewState = PreviewState.Play;
            _lastTime = (float)EditorApplication.timeSinceStartup;
        }
        if (GUILayout.Button("Pause"))
        {
            _previewState = PreviewState.Pause;
        }
        if (GUILayout.Button("Stop"))
        {
            _previewState = PreviewState.Stop;
            _moveTotalTime = 0;
            _bomb.transform.position = _bomb.originalPosition;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
    private void OnSceneGUI()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }

        switch (_previewState)
        {
            case PreviewState.Play:
                PreviewInScene();
                break;
            case PreviewState.Pause:
                break;
            case PreviewState.Stop:
                DrawHandle();
                _bomb.UpdateOriginalPosition();
                break;
            default:
                break;
        }

        DrawCurve();
    }

    private void PreviewInScene()
    {
        float currentTime = (float)EditorApplication.timeSinceStartup;
        float deltaTime = currentTime - _lastTime;
        _lastTime = currentTime;

        _moveTotalTime += deltaTime;
        if (_bomb.transform.position.y >= _bomb.originalPosition.y)
        {
            _bomb.transform.position = _bomb.GetPosition(_moveTotalTime);
        }
    }

    private void DrawHandle()
    {
        Handles.color = Color.blue;
        //绘制起点
        var originalPos = _bomb.transform.position;
        float handleSize = HandleUtility.GetHandleSize(originalPos);
        float constSize = handleSize * AnchorSize;//锚点大小

        _bomb.transform.position = Handles.FreeMoveHandle(originalPos, Quaternion.identity, constSize, Vector3.one, sphereCap);

        //绘制最高点
        float maxHeightTime = _bomb.GetTotalTime() / 2;
        Vector3 maxHeightPos = _bomb.GetPosition(maxHeightTime);

        handleSize = HandleUtility.GetHandleSize(maxHeightPos);
        constSize = handleSize * AnchorSize;
        maxHeightPos = Handles.FreeMoveHandle(maxHeightPos, Quaternion.identity, constSize, Vector3.one, sphereCap);

        //绘制最远点
        Vector3 maxLengthPos = _bomb.GetPosition(2 * maxHeightTime);
        handleSize = HandleUtility.GetHandleSize(maxLengthPos);
        constSize = handleSize * AnchorSize;
        maxLengthPos = Handles.FreeMoveHandle(maxLengthPos, Quaternion.identity, constSize, Vector3.one, sphereCap);

        float yAxisDeltaDistance = maxHeightPos.y - _bomb.originalPosition.y;
        float zAxisDeltaDistance = maxLengthPos.z - _bomb.originalPosition.z;
        if (yAxisDeltaDistance > 0 && zAxisDeltaDistance > 0)
        {
            _bomb.initialAngle = Mathf.Atan(4 * yAxisDeltaDistance / zAxisDeltaDistance) / Mathf.Deg2Rad;
            _bomb.initialSpeed = Mathf.Sqrt(_bomb.GetGravity() * zAxisDeltaDistance / Mathf.Sin(2 * _bomb.initialAngle * Mathf.Deg2Rad));
        }
    }

    private void DrawCurve()
    {
        float totalTime = 0;

        while (_bomb.GetPosition(totalTime).y >= _bomb.originalPosition.y)
        {
            Vector3 pos1 = _bomb.GetPosition(totalTime);
            totalTime += DeltaTime;
            Vector3 pos2 = _bomb.GetPosition(totalTime);

            Handles.color = Color.red;
            Handles.DrawLine(pos1, pos2);
        }
    }
}
