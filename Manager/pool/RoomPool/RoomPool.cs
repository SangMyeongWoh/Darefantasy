using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RoomPool
{
    protected List<List<List<NodeSetter>>> nodeSetters_list = new List<List<List<NodeSetter>>>();
    
    public void readcsv(RoomType roomType, int stagelv)
    {


        object[] datas = Resources.LoadAll(roomType.ToString() + "/" + stagelv.ToString() + "/");
         
        for (int i = 0; i < datas.Length; i++)
        {
            
            //List<string> data = CSVReader.read_roominfo(subpath + info[i].Name.Substring(0, info[i].Name.Length - 4));
            
            List<string> data = CSVReader.return_sting_list((TextAsset)datas[i]);
            
            nodeSetters_list[stagelv - 1].Add(new List<NodeSetter>());
            

            ObjectBaseType temptype = ObjectBaseType.NONE;
            int subtype = 0;
            int index = 0;
            for (int j = 0; j < 50; j++) //총 25 * 2개
            {       
                if (j % 2 == 0)
                {                    
                    temptype = (ObjectBaseType)System.Enum.Parse(typeof(ObjectBaseType), data[j]); //오브젝트 베이스타입                    
                    index = j / 2;
                    
                } else
                {
                    subtype = int.Parse(data[j]);
                    nodeSetters_list[stagelv - 1][i].Add(new NodeSetter(temptype, subtype, index));
                }                
            }
            //여기에 방 한개의 정보가 담김. 그럼 stagelv마다 몇개의 방 정보가 잇어야겟지?
        }
        

    }
   

    public void setpool(RoomType roomType)
    {        
        for(int i = 0; i < GameManager.Instance.maxstagelv; i++)
        {
            nodeSetters_list.Add(new List<List<NodeSetter>>());
            
            readcsv(roomType, i + 1);
        }
    }

    public List<NodeSetter> get_nodesetters(int stagelv)
    {
        int kinds = Random.Range(0, nodeSetters_list[stagelv - 1].Count);
        return nodeSetters_list[stagelv - 1][kinds];
        
    }
    
}

public class NodeSetter
{
    public NodeSetter(ObjectBaseType _objectBaseType, int _subtype, int _nodeindex)
    {
        objectBaseType = _objectBaseType;
        subtype = _subtype;
        nodeindex = _nodeindex;
    }
    public ObjectBaseType objectBaseType;
    public int subtype;
    public int nodeindex;    
}
