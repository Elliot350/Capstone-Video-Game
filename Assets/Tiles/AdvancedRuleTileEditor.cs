using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityEditor
{
    [CustomEditor(typeof(AdvancedRuleTile))]
    [CanEditMultipleObjects]
    public class AdvancedRuleTileEditor : RuleTileEditor
    {
        public Texture2D any, specified, empty;

        public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor)
        {
            switch (neighbor)
            {
                case 3:
                    GUI.DrawTexture(rect, any);
                    return;
                case 4:
                    GUI.DrawTexture(rect, specified);
                    return;
                case 5:
                    GUI.DrawTexture(rect, empty);
                    return;
            }
            
            base.RuleOnGUI(rect, position, neighbor);
        }
    }
}


