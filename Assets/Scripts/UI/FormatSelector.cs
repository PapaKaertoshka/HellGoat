using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FormatSelector : HorizontalSelector<AspectRatio>
{
    protected override void UpdateText()
    {
        _text.text = _data[_currentIndex].ToString();
    }

    protected override void Execute()
    {
        PlayerSettings.SetAspectRatio(_data[_currentIndex], true);
    }
}
