using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != this)
        {
            var objs = FindObjectsOfType<ObjectManager>();
            if (objs.Length > 1)
                Destroy(gameObject);
            else
                instance = this;
        }
        DontDestroyOnLoad(gameObject);

        var prefabs = Resources.LoadAll<GameObject>("Prefabs");
        foreach(var prefab in prefabs)
        {
            eObjectKey key = prefab.GetComponent<Prefab>().ObjectKey;
            if(!prefabList.ContainsKey(key))
            {
                if(key==eObjectKey.PLAYER)
                {
                    var playerObj = Instantiate<GameObject>(prefab);
                    playerObj.SetActive(false);
                    player = playerObj;
                }
                else
                prefabList.Add(prefab.GetComponent<Prefab>().ObjectKey, prefab);
            }
        }
    }

    public GameObject NewObject(eObjectKey _key)
    {
        if (!disableList.ContainsKey(_key))
        {
            if (prefabList.ContainsKey(_key))
                disableList.Add(_key, new Queue<GameObject>());
            else
                return null;
        }
        if (disableList[_key].Count == 0)
        {
            int size = createSize;
            eGroupKey groupKey = prefabList[_key].GetComponent<Prefab>().GroupKey;
            if (groupKey == eGroupKey.BOSS || groupKey == eGroupKey.EFFECT)
                size = 1;

            for (int i = 0; i < size; ++i)
            {
                var createObj = Instantiate<GameObject>(prefabList[_key]);
                createObj.SetActive(false);
                disableList[_key].Enqueue(createObj);
            }
        }
        if(!enableList.ContainsKey(_key))
            enableList.Add(_key, new LinkedList<GameObject>());

        var obj = disableList[_key].Dequeue();
        obj.SetActive(true);
        enableList[_key].AddLast(obj);

        return obj;
    }


    public GameObject NewObject(eObjectKey _key, Vector3 _position)
    {
        var obj = NewObject(_key);
        obj.transform.position = _position;
        return obj;
    }

    public GameObject NewObject(eObjectKey _key, Vector3 _position,float _direction)
    {
        var obj = NewObject(_key, _position);
        obj.GetComponent<Prefab>().Direction = _direction;
        return obj;
    }

    public GameObject NewObject(eObjectKey _key, Vector3 _position, int angle)
    {
        var obj = NewObject(_key, _position);
        obj.GetComponent<Prefab>().Angle = angle;
        return obj;
    }

    public void RecallObject(GameObject _obj)
    {
        eObjectKey key = _obj.GetComponent<Prefab>().ObjectKey;

        if(enableList[key].Remove(_obj))
        {
            _obj.SetActive(false);
            disableList[key].Enqueue(_obj);
        }
    }







    // Getter & Setter
    public GameObject Player => player;

    public static ObjectManager Instance
    {
        get
        {
            if(instance==null)
            {
                instance = FindObjectOfType<ObjectManager>();
                if(instance==null)
                    instance = new GameObject("ObjectManager").AddComponent<ObjectManager>();
            }
            return instance;
        }
    }

    // 생성자
    private ObjectManager() { }






    // ** Field
    Dictionary<eObjectKey, LinkedList<GameObject>> enableList = new Dictionary<eObjectKey, LinkedList<GameObject>>();
    Dictionary<eObjectKey, Queue<GameObject>> disableList = new Dictionary<eObjectKey, Queue<GameObject>>();
    Dictionary<eObjectKey, GameObject> prefabList = new Dictionary<eObjectKey, GameObject>();

    // 플레이어
    GameObject player;
    int createSize = 4;


    // 싱글톤 인스턴스
    private static ObjectManager instance;
}
