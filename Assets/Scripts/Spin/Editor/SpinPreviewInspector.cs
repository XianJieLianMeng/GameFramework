using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine.Unity;

[CustomEditor(typeof(SpinPreview))]
public class SpinPreviewInspector :Editor
{
    private SpinPreview _spinPreview;

    private SkeletonAnimation _skeletonAnimation;

    private Spine.TrackEntry _playTrack;

    private float _previewTimeScale;

    private float _currentNormalizeTime;

    private float _lastTime;

    private void OnEnable()
    {
        _spinPreview = target as SpinPreview;
        _skeletonAnimation = _spinPreview.GetComponent<SkeletonAnimation>();

        if(Application.isPlaying == false)
        {
            if(_spinPreview.animationName != "")
            {
                _skeletonAnimation.Initialize(false);
                _playTrack = _skeletonAnimation.state.SetAnimation(0, _spinPreview.animationName, true);

                _playTrack.TimeScale = _previewTimeScale;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        if(Application.isPlaying == false)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);

            DrawAnimationName();
            DrawTimeScale();
            DrawTimeLine();

            EditorGUILayout.EndVertical();
        }

        if(EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_spinPreview);
        }
    }

    private void DrawAnimationName()
    {
        List<string> animationNames = new List<string>();

        var animations = _skeletonAnimation.SkeletonDataAsset.GetSkeletonData(true).Animations;

        foreach (var anim in animations)
        {
            animationNames.Add(anim.Name);
        }

        int index = 0;
        if(animationNames.Contains(_spinPreview.animationName))
        {
            index = animationNames.IndexOf(_spinPreview.animationName);
        }

        index = EditorGUILayout.Popup("AnimationName", index, animationNames.ToArray());

        var animationName = animationNames[index];

        if(_spinPreview.animationName != animationName)
        {
            _spinPreview.animationName = animationName;
            _playTrack = _skeletonAnimation.state.SetAnimation(0, _spinPreview.animationName, true);
        }
    }

    private void DrawTimeScale()
    {
        _previewTimeScale = EditorGUILayout.Slider("TimeScale", _previewTimeScale, 0, 1);
        if(_playTrack != null)
        {
            _playTrack.TimeScale = _previewTimeScale;
        }
    }

    private void DrawTimeLine()
    {
        if(_playTrack == null)
        {
            return;
        }

        var borderRect = EditorGUILayout.GetControlRect();
        EditorGUI.DrawRect(borderRect, Color.black);

        var contentRect = ShrinkRect(borderRect, 1, 1);
        EditorGUI.DrawRect(contentRect, Color.gray);

        if(_playTrack.TimeScale != 0)
        {
            _currentNormalizeTime = _playTrack.AnimationTime / _playTrack.Animation.Duration;
        }
        else
        {
            var mouseInsiderBar = contentRect.Contains(Event.current.mousePosition);
            if(mouseInsiderBar && (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag))
            {
                _currentNormalizeTime = (Event.current.mousePosition.x - borderRect.x) / borderRect.width;
            }

            _playTrack.TrackTime = _currentNormalizeTime * _playTrack.Animation.Duration;
        }

        var currentX = contentRect.x + contentRect.width * _currentNormalizeTime;
        Handles.color = Color.white;
        Handles.DrawLine(new Vector3(currentX, borderRect.yMin), new Vector3(currentX, borderRect.yMax));
    }

    //绘制内部矩形区域
    private Rect ShrinkRect(Rect rect,float x,float y)
    {
        return new Rect(rect.x + x, rect.y + y, rect.width - 2 * x, rect.height - 2 * y);
    }

    private void OnSceneGUI()
    {
        if(Application.isPlaying || _playTrack == null)
        {
            return;
        }

        var currentTime = (float)EditorApplication.timeSinceStartup;
        var deltaTime = currentTime - _lastTime;
        _lastTime = currentTime;

        _skeletonAnimation.Update(deltaTime);
        _skeletonAnimation.LateUpdate();
    }
}
