using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolController : MonoBehaviour
{

    [SerializeField] private int startingID;
    [SerializeField] private string entityName;
    [SerializeField] private int capacity;
    [SerializeField] private GameObject container;
    public GameObject usedContainer;
    public GameObject prefab;
    private int availableID;
    List<Transform> used = new();
       



    void Start()
    {
        availableID = startingID;
        InitCapacity();
    }

  
    void InitCapacity()
    {
        for(int i =0; i < capacity; i++)
        {
            CreateNew().transform.SetParent(container.transform);
        }
    }


    public GameObject Get(GameObject newParent=null)
    {
        int count = container.transform.childCount;
        GameObject entity = count <= 0 ? CreateNew() : container.transform.GetChild(count - 1).gameObject;
        if (newParent != null)
        {
            entity.transform.SetParent(newParent.transform);
        }
        else
        {
            entity.transform.SetParent(usedContainer.transform);
        }
        used.Add(entity.transform);
        return entity;
    }

    public void Restore(GameObject entity)
    {
        entity.SetActive(false);
        entity.transform.SetParent(container.transform);
        for(int i=0; i < used.Count; i++)
        {
            if(used[i].name == entity.name)
            {
                used.RemoveAt(i);
                break;
            }
        }
    }

    public void RestoreAll()
    {
        while(used.Count > 0)
        {
            Restore(used[0].gameObject);
        }
    }
    
    
    GameObject CreateNew()
    {
        GameObject entity = Instantiate(prefab);
        entity.SetActive(false);
        entity.name = prefab.name + availableID++;
        entity.transform.SetParent(transform);
        return entity;
    }
}
