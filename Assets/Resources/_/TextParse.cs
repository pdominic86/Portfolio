using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParse
{
    public static List<List<int>> To2DList(TextAsset _text)
    {
        const char LINE_SPLIT = '\n';
        const char ELEM_SPLIT = ',';
        List<List<int>> list2D = new List<List<int>>();

        var lines = _text.text.Split(LINE_SPLIT);
        foreach(var line in lines)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            List<int> elemList = new List<int>();
            var elems = line.Split(ELEM_SPLIT);
            foreach(var elem in elems)
            {
                elemList.Add(int.Parse(elem));
            }
            list2D.Add(elemList);
        }

        return list2D;
    }
}
