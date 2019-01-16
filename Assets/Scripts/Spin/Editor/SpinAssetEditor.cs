using Spine.Unity;
using Spine.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpinAsset))]
public class SpinAssetEditor : Editor
{
    [MenuItem("Asset/Create/SpinAsset")]
    public static void CreateInstance()
    {
        SpinAsset spinAsset = ScriptableObject.CreateInstance<SpinAsset>();
        AssetDatabase.CreateAsset(spinAsset, "Assets/Resources/SpinAsset/SpinAsset.asset");
    }

    private SpinAsset spinAsset { get { return target as SpinAsset; } }

    private float _lastAnimationTime;

    private float currentAnimationTime
    {
        get
        {
            return (float)EditorApplication.timeSinceStartup;
        }
    }

    private GameObject _previewGameObject;

    private SkeletonAnimation _skeletonAnimation;

    private bool _requierRefresh = false;

    //Unity内部专门用来绘制预览窗口类
    private PreviewRenderUtility _previewRenderUtility;

    private Camera previewUtilityCamera
    {
        get
        {
            if (_previewRenderUtility == null)
            {
                return null;
            }
            else
            {
                return _previewRenderUtility.camera;
            }
        }
    }

    //渲染图层
    private const int PreviewLayer = 30;

    //告诉unity说我有预览窗口
    public override bool HasPreviewGUI()
    {
        return true;
    }

    //预览窗口标题
    public override GUIContent GetPreviewTitle()
    {
        return new GUIContent("Preview");
    }

    //绘制可交互预览窗口函数
    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        if(spinAsset.skeletonDataAsset == null)
        {
            return;
        }
        DisposePreviewRenderUtility();
        DestroyPreviewGameObject();

        Initialize(spinAsset.skeletonDataAsset,"default");
        HandleInterativePreviewGUI(r, background);
    }

    private void DisposePreviewRenderUtility()
    {
        if (_previewRenderUtility != null)
        {
            _previewRenderUtility.Cleanup();
            _previewRenderUtility = null;
        }
    }

    private void DestroyPreviewGameObject()
    {
        if (_previewGameObject != null)
        {
            GameObject.DestroyImmediate(_previewGameObject);
            _previewGameObject = null;
        }
    }

    public void OnDestroy()
    {
        DisposePreviewRenderUtility();
        DestroyPreviewGameObject();
    }

    private void Initialize(SkeletonDataAsset skeletonDataAsset, string skinName = "")
    {
        if (_previewRenderUtility != null)
        {
            return;
        }

        _previewRenderUtility = new PreviewRenderUtility(true);
        _lastAnimationTime = currentAnimationTime;

        if (_previewGameObject == null)
        {
            _previewGameObject = SpineEditorUtilities.InstantiateSkeletonAnimation(skeletonDataAsset, skinName).gameObject;

            if (_previewGameObject != null)
            {
                _previewGameObject.hideFlags = HideFlags.HideAndDontSave;
                _previewGameObject.layer = PreviewLayer;

                _skeletonAnimation = _previewGameObject.GetComponent<SkeletonAnimation>();
                _skeletonAnimation.initialSkinName = skinName;
                _skeletonAnimation.LateUpdate();
                //初始化时不进行渲染
                _previewGameObject.GetComponent<Renderer>().enabled = false;
            }
            _requierRefresh = true;
        }
        InitializePreviewCamera();
    }

    private void InitializePreviewCamera()
    {
        int previewCameraCullingMask = 1 << PreviewLayer;
        var camera = this.previewUtilityCamera;
        camera.orthographic = true;
        camera.cullingMask = previewCameraCullingMask;//要绘制的图层为我们设置的图层
        camera.nearClipPlane = 0.01f;
        camera.farClipPlane = 1000f;

        if(_previewGameObject != null)
        {
            Bounds bounds = _previewGameObject.GetComponent<Renderer>().bounds;
            camera.orthographicSize  = bounds.size.y;//调整范围
            camera.transform.position = bounds.center + new Vector3(0, 0, -10);//拉远相机
        }
    }

    //处理预览窗口
    private void HandleInterativePreviewGUI(Rect r,GUIStyle background)
    {
        if(Event.current.type == EventType.Repaint)
        {
            Texture previewTexture = null;
            //让绘制函数不用每帧执行
            if(_requierRefresh)
            {
                //固定格式
                _previewRenderUtility.BeginPreview(r, background);
                //进行绘制
                RenderPreview();
                previewTexture = _previewRenderUtility.EndPreview();

                _requierRefresh = false;
            }

            if(previewTexture != null)
            {
                //绘制贴图
                GUI.DrawTexture(r, previewTexture, ScaleMode.StretchToFill, false);
            }
        }
    }

    private void RenderPreview()
    {
        if(this.previewUtilityCamera.activeTexture == null || this.previewUtilityCamera.targetTexture == null)
        {
            return;
        }

        if (_requierRefresh && _previewGameObject != null)
        {
            var renderer = _previewGameObject.GetComponent<Renderer>();
            renderer.enabled = true;

            if(EditorApplication.isPlaying == false)
            {
                //编辑器模式下替代Time.deltaTime
                float current = currentAnimationTime;
                float deltaTime = current - _lastAnimationTime;
                _skeletonAnimation.Update(deltaTime);
                _lastAnimationTime = current;

                previewUtilityCamera.Render();
                //不关的话物体会显示在场景中
                renderer.enabled = false;
            }
        }
    }
}
