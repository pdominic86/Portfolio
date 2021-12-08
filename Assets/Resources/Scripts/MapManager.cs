using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private void Awake()
    {
        tileInfo = ScriptableObject.CreateInstance<TileManager>();
        map = FindObjectOfType<Tilemap>();
        mapInfo = TextParse.To2DList(Resources.Load<TextAsset>("Texts/MapInfo"));
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(position);
        }
    }

    public void StartEvent(Vector2Int _range)
    {
        eventRange = _range;
        bMapChange = true;
        StartCoroutine(ChangeMap());
    }

    IEnumerator ChangeMap()
    {
        while(bMapChange)
        {
            yield return new WaitForSeconds(0.2f);
            int x = Random.Range(eventRange.x, eventRange.y);
            RaycastHit2D hit=Physics2D.Raycast(new Vector2(x,15),Vector2.down);
            Vector3Int gridPosition = map.WorldToCell(hit.point);
            int tileIndex = int.Parse(map.GetTile(gridPosition).name);
            Debug.Log($"{x} : {tileIndex} tile");

            if (mapInfo[gridPosition.x][gridPosition.y]!=tileIndex)
            {
                int changeIndex = 0;

                if (mapInfo[gridPosition.x][gridPosition.y] == 0)
                {

                }
                else
                {
                    switch(tileIndex)
                    {
                        case 1:
                            changeIndex = 2;
                            break;
                        case 2:
                        case 4:
                            changeIndex = 3;
                            break;
                        case 3:
                            changeIndex = 5;
                            break;

                    }
                }

                if (changeIndex != 0)
                {
                    map.SetTile(gridPosition, tileInfo.GetTile(changeIndex));
                    VerticalChange(gridPosition, changeIndex);
                    HorizontalChange(gridPosition, changeIndex, -1);
                    HorizontalChange(gridPosition, changeIndex, 1);
                }

                bool same = true;
                for(int i=eventRange.x;i<=eventRange.y;++i)
                {
                    if (mapInfo[i][gridPosition.y] != int.Parse(map.GetTile(new Vector3Int(i, gridPosition.y, 0)).name))
                    {
                        same = false;
                        break;
                    }
                }
                if (same)
                    bMapChange = false;
            }
        }
    }

    void VerticalChange(Vector3Int _pos,int _tileIndex)
    {
        Vector3Int up = _pos;
        ++up.y;
        Vector3Int down = _pos;
        --down.y;

        if(_tileIndex<7)
        {
            int downTile = int.Parse(map.GetTile(down).name);
            int changedTile = _tileIndex + 6;
            if (downTile != changedTile)
                map.SetTile(down, tileInfo.GetTile(changedTile));
        }
        else if(_tileIndex<11)
        {
            int upTile = int.Parse(map.GetTile(up).name);
            int changedTile = _tileIndex - 6;
            if (upTile != changedTile)
                map.SetTile(up, tileInfo.GetTile(changedTile));
        }
        else
        {
            int upTile = int.Parse(map.GetTile(up).name);
            int changedTile = _tileIndex -6;
            if (upTile != changedTile)
                map.SetTile(up, tileInfo.GetTile(changedTile));

            int downTile = int.Parse(map.GetTile(down).name);
            changedTile = 9;
            if (downTile != changedTile)
                map.SetTile(down, tileInfo.GetTile(changedTile));
        }

    }

    void HorizontalChange(Vector3Int _pos, int _tileIndex, int _direction)
    {
        Vector3Int left = _pos;
        Vector3Int right = _pos;
        Vector3Int center = _pos;
        int leftTile = 0;
        int rightTile = 0;
        int centerTile = 0;
        if (_direction < 0)
        {
            --center.x;
            left.x += -2;
            centerTile = int.Parse(map.GetTile(center).name);
            leftTile = int.Parse(map.GetTile(left).name);
            rightTile = _tileIndex;
        }
        else
        {
            ++center.x;
            right.x += 2;
            centerTile = int.Parse(map.GetTile(center).name);
            rightTile = int.Parse(map.GetTile(right).name);
            leftTile = _tileIndex;
        }

        int changedTile = tileInfo.GetTile(tileInfo.GetRightPoint(leftTile), tileInfo.GetLeftPoint(rightTile));
        Debug.Log($"{leftTile} : {tileInfo.GetRightPoint(leftTile)}");
        Debug.Log($"{rightTile} : {tileInfo.GetLeftPoint(rightTile)}");
        if (changedTile != 0 && changedTile != centerTile)
        {
            map.SetTile(center, tileInfo.GetTile(changedTile));
            VerticalChange(center, changedTile);
            HorizontalChange(center, changedTile, _direction);
        }
    }

    TileManager tileInfo;
    Tilemap map;
    List<List<int>> mapInfo = new List<List<int>>();

    bool bMapChange;
    Vector2Int eventRange = new Vector2Int();
}
