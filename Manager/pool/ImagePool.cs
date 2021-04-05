using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePool : MonoBehaviour
{
    public List<TileBox> tileBoxes = new List<TileBox>();
    public List<MapBox> mapBoxes = new List<MapBox>();
    
    
    public List<Sprite> get_tile_imageList(StageType stageType)
    {
        for(int i = 0; i < tileBoxes.Count; i++)
        {
            if(stageType == tileBoxes[i].stageType) return tileBoxes[i].tile_sprite_list;
        }
        return null;
    }
    public Sprite get_mapimage(StageType stageType)
    {
        for (int i = 0; i < mapBoxes.Count; i++)
        {
            if (stageType == mapBoxes[i].stageType) return mapBoxes[i].map_sprite;
        }
        return null;
    }

}
[System.Serializable]
public class TileBox
{
    public StageType stageType;
    public List<Sprite> tile_sprite_list = new List<Sprite>();
}

[System.Serializable]
public class MapBox
{
    public StageType stageType;
    public Sprite map_sprite;
}
