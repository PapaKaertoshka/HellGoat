using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualitySelector : HorizontalSelector<string>
{
    protected override void UpdateText()
    {
        _text.text = _data[_currentIndex];
    }

    protected override void Execute()
    {
        QualitySettings.SetQualityLevel(_currentIndex);
    }
}
