using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : ScriptableObject
{
    private void Awake()
    {
        // 변환에 필요한 Tile을 resource에서 불러옴 
        var tiles = Resources.LoadAll<TileBase>("Tiles");
        foreach(var tile in tiles)
        {
            tileList.Add(int.Parse(tile.name), tile);
        }

        // tile의 정보를 담은 text 가져옴
        tileInfo = TextParse.To2DList(Resources.Load<TextAsset>("Texts/TileInfo"));
    }

    public TileBase GetTile(int _index)
    {
        return tileList[_index];
    }

    public int GetLeftPoint(int _index)
    {
        return tileInfo[_index-1][0];
    }

    public int GetRightPoint(int _index)
    {
        return tileInfo[_index - 1][1];
    }

    public int GetTile(int _leftPoint,int _rightPoint)
    {
        if (_leftPoint == 0 || _rightPoint == 0)
            return 0;

        for(int i=0;i< tileInfo.Count; ++i)
        {
            if(_leftPoint == tileInfo[i][0] && _rightPoint== tileInfo[i][1])
                return i+1;
        }

        return 0;
    }

    Dictionary<int, TileBase> tileList = new Dictionary<int, TileBase>();
    List<List<int>> tileInfo;
}
