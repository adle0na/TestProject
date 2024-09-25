using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(CustomInspector))] //CustomClass는 커스텀 Inspector로 표시할 것
public class CustomInspector : Editor
{
    /* Inspector를 그리는 함수 */
    public override void OnInspectorGUI()
    {
    
    }
}
#endif
