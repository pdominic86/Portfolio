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

        var prefabs = Resources.LoadAll<GameObject>("Prefabs");
        foreach(var prefab in prefabs)
        {
            preFabList.Add(prefab.GetComponent<Prefab>().ObjectKey, prefab);
        }
    }

    public GameObject NewObject(eObjectKey _key, Vector3 _position)
    {
        if (!disableList.ContainsKey(_key))
        {
            if (preFabList.ContainsKey(_key))
                disableList.Add(_key, new Queue<GameObject>());
            else
                return null;
        }
        if (disableList[_key].Count == 0)
        {
            for (int i = 0; i < createSize; ++i)
            {
                var createObj = Instantiate<GameObject>(preFabList[_key]);
                createObj.SetActive(false);
                disableList[_key].Enqueue(createObj);
            }
        }
        if(!enableList.ContainsKey(_key))
            enableList.Add(_key, new LinkedList<GameObject>());

        var obj = disableList[_key].Dequeue();
        obj.SetActive(true);
        obj.transform.position = _position;
        enableList[_key].AddLast(obj);

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

    // ������
    private ObjectManager() { }






    // ** Field
    Dictionary<eObjectKey, LinkedList<GameObject>> enableList = new Dictionary<eObjectKey, LinkedList<GameObject>>();
    Dictionary<eObjectKey, Queue<GameObject>> disableList = new Dictionary<eObjectKey, Queue<GameObject>>();
    Dictionary<eObjectKey, GameObject> preFabList = new Dictionary<eObjectKey, GameObject>();
    int createSize = 4;


    // �̱��� �ν��Ͻ�
    private static ObjectManager instance;
}
