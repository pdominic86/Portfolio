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
        mapMask = LayerMask.GetMask("Map");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(position);
        }
    }

    public void StartEvent(int _min,int _max)
    {
        eventRange.x=_min;
        eventRange.y= _max;
        bMapChange = true;
        StartCoroutine(ChangeMap());
    }

    IEnumerator ChangeMap()
    {
        while(bMapChange)
        {
            yield return new WaitForSeconds(0.1f);
            if (leftIndex<= eventRange.x)
            {
                leftIndex = (eventRange.x+ eventRange.y)/2;
                rightIndex = leftIndex + 1;
            }
            else 
            {
                --leftIndex;
                ++rightIndex;
                if (rightIndex > eventRange.y)
                    rightIndex = eventRange.y;
            }

            // ¿ÞÂÊ
            TryChange(leftIndex, -1);
            TryChange(rightIndex, 1);
            
            bool same = true;
            for (int i = eventRange.x; i <= eventRange.y; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    if(int.TryParse(map.GetTile(new Vector3Int(i,j,0))?.name,out int result))
                    {
                        if(result!=mapInfo[i][j])
                        {
                            same = false;
                            break;
                        }
                    }
                }
                if (!same)
                    break;
            }
            if (same)
                    bMapChange = false;
        }
    }
    void TryChange(int _xIndex, int _direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(_xIndex, 15), Vector2.down, Mathf.Infinity, mapMask);
        Vector3Int gridPosition = map.WorldToCell(hit.point);
        int tileIndex = 0;
        int.TryParse(map.GetTile(gridPosition)?.name, out tileIndex);

        if (mapInfo[gridPosition.x][gridPosition.y] != tileIndex)
        {
            int changeIndex = 0;

            if (mapInfo[gridPosition.x][gridPosition.y] == 0)
            {

            }
            else
            {
                if (mapInfo[gridPosition.x][gridPosition.y] < 7)
                {
                    changeIndex = mapInfo[gridPosition.x][gridPosition.y];
                }
                else
                {

                    switch (tileIndex)
                    {
                        case 1:
                            changeIndex = (_direction > 0 ? 2 : 4);
                            break;
                        case 2:
                        case 4:
                            changeIndex = 3;
                            break;
                        case 3:
                            changeIndex = (_direction > 0 ? 11 : 12);
                            break;
                    }
                }
            }

            if (changeIndex != 0)
            {
                map.SetTile(gridPosition, tileInfo.GetTile(changeIndex));
                VerticalChange(gridPosition, changeIndex);
                HorizontalChange(gridPosition, -1);
                HorizontalChange(gridPosition, 1);
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
            int changedTile = _tileIndex + 6;
            int downTile = 0;
            int.TryParse(map.GetTile(down)?.name, out downTile);
            if (downTile != changedTile)
            {
                map.SetTile(down, tileInfo.GetTile(changedTile));
                VerticalChange(down, changedTile);
            }
        }
        else 
        {
            int changedTile = _tileIndex -6;
            int upTile = 0;
            int.TryParse(map.GetTile(up)?.name, out upTile);
            if (upTile != changedTile)
                map.SetTile(up, tileInfo.GetTile(changedTile));

            changedTile = 9;
            int downTile = 0;
            int.TryParse(map.GetTile(down)?.name, out downTile);
            if (downTile != changedTile)
                map.SetTile(down, tileInfo.GetTile(changedTile));
        }

    }

    void HorizontalChange(Vector3Int _pos, int _direction)
    {
        Vector3Int left = _pos;
        Vector3Int center = _pos;
        Vector3Int right = _pos;
        int leftTile = 0;
        int centerTile = 0;
        int rightTile = 0;
        if (_direction < 0)
        {
            --center.x;
            left.x += -2;
        }
        else
        {
            ++center.x;
            right.x += 2;
        }
        int.TryParse(map.GetTile(left)?.name, out leftTile);
        int.TryParse(map.GetTile(center)?.name, out centerTile);
        int.TryParse(map.GetTile(right)?.name, out rightTile);

        int changedTile = 0;
        if (leftTile!=0 && rightTile!=0)
            changedTile = tileInfo.GetTile(tileInfo.GetRightPoint(leftTile), tileInfo.GetLeftPoint(rightTile));

        if (changedTile != 0 && changedTile != centerTile)
        {
            map.SetTile(center, tileInfo.GetTile(changedTile));
            VerticalChange(center, changedTile);
            HorizontalChange(center, _direction);
        }
    }


    TileManager tileInfo;
    Tilemap map;
    List<List<int>> mapInfo = new List<List<int>>();

    bool bMapChange;
    Vector2Int eventRange = new Vector2Int();
    int leftIndex;
    int rightIndex;
    int eventDirection;
    int mapMask;
}
