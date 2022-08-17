#if UNITY_EDITOR
using UnityEditor;

namespace Engine
{
    [CustomEditor(typeof(VolumeChanger))]
    public class VolumeChangerEditor : Editor
    {
        VolumeChanger volumeChanger;
        private void OnEnable()
        {
            volumeChanger = (VolumeChanger)target;
            volumeChanger.Init();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            volumeChanger.UpdateValueSlider();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
