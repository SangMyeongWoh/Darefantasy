using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
	public const int WELL_POINT = 3;
    public const int MIN_FRAME = 10;
	public const int SPACE_FRAME = 30;
    public const int MAX_INDEX = 5;
	public const int DEFAULT_MAX_ITEM_LENGTH = 2;
	public const int DEFAULT_MAX_HEART_LENGTH = 3;
	public const int ANIMTIME = 10;
	public const float GRID_SIZE = 1.5f;
	public const float DELAY_TIME = 0.2f;
	public const int DEFAULT_PLAYER_HEART = 3;
    public const int HIGHTLIGHT_ANIMTIME = 45;
	public const float ITEM_UI_X = 6.1f;
	public const float ITEM_UI_Y = -2.0f;
    public const int FLOORSPRITESORTINGORDER = -20000;

}
public enum Key
{
	None,
	Space,
	W,
	S,
	A,
	D,
	Up,
	Down,
	Left,
	Right,
}

public enum Process_Type
{
	None,
	Nodetransition,
	Roomtransition,
	Item,
}

public enum Turn_Type
{
	None,
	Player_turn,
	Enemy_turn,
	Both_turn,
    Object_turn,
}
public enum CameraAnimType
{
    ROOMTRANSITION_SOFT,
    ROOMTRANSITION_HARD,
    SHAKE_CAMERA,
    HIDESCREEN,
    ZOOM_CAMERA,
    HP_DOWN,
    CHARACTER_DEATH,
}

public enum ObjectBaseType
{
	NONE,
	CHARACTER,
	MONSTER,
    BLOCK,
	TRAP,
	DOOR,
    ITEM,
}

public enum AnimType
{
    IDLE,              //0
    MOVE,              //1
    ATTACK,            //2
    HIT,               //3
    DEATH,             //4
    ATTACKEND,         //5
    RECALL,            //6
    LANDING,           //7
    TRY,               //8
    HIGHLIGHT,         //9
    HIT_WITHBLOOD,     //10
    HIT_WITHOUTBLOOD,  //11
    CANNOTATTACK,      //12
    DEATHWITHOUTDELAY, //13
    ITEMEFFECT,
    FASTMOVE,
}



public enum RoomType
{
    None,
	INITIAL,
    NORMAL,
	WELL,
	MAGICCIRCLE,
    WORKSHOP,
}

public enum ItemType
{
    ITEM_HEART,
    ITEM_SWORD,
    ITEM_SWORD_RED,
    ITEM_SHIELD,
    ITEM_SHIELD_RED,
    ITEM_BAG,
	ITEM_KEY,
    ITEM_BLUEHEART,
    ITEM_HOLYGRAIL,
    ITEM_HOLYGRAIL_RED,
    ITEM_HOLYSWORD,
    ITEM_HOLYSWORD_RED,
    ITEM_CURSEPIECE,
    ITEM_BAG_HERAT,
    ITEM_POOP,
    ITEM_KEY_RED,
    ITEM_NONE,
}

public enum ItemUIType
{
    ITEM_HEART_UI,
    ITEM_SWORD_UI,
    ITEM_RED_SWORD_UI,
    ITEM_SHIELD_UI,
    ITEM_RED_SHIELD_UI,
    ITEM_BAG_UI,
    ITEM_KEY_UI,
    ITEM_BLUEHEART_UI,
    ITEM_HOLYGRAIL_UI,
    ITEM_RED_HOLYGRAIL_UI,
    ITEM_HOLYSWORD_UI,
    ITEM_RED_HOLYSWORD_UI,
    ITEM_CURSEPIECE_UI,
    ITEM_BAG_HEART_UI,
    ITEM_POOP_UI,
    ITEM_RED_KEY_UI,
}


public enum MonsterType
{
    NONE,
    ZOMBIE,
    CHARACTER,
    GHOST,
    ZOMBIE_TONGUE,
    GHOST_LV2,
}

public enum BlockType
{
	NONE,
	BOX,
	CASE,
    WELL,
    VENDINGMACHINE,
    CASE_LOCKED,
    WELL_LOCKED,
    
}

public enum TrapType
{
	NONE,
    MAGICCIRCLE, //Trap_MagiccircleBase
    NORMAL, //Trap_NormalBase
    NORMAL_TYPE2,
    NORMAL_TYPE3,
}

public enum DoorType
{
	NONE, //0

	NORMAL, // 1
	UP_STAIR, // 2
	DOWN_STAIR, // 3

	LOCKED_DOOR, // 4
	
	LOCKED_DOOR_NORAML, // 5
	LOCKED_DOOR_UP_STAIR, // 6 
	LOCKED_DOOR_DOWN_STAIR, // 7
}

public enum StageType
{
    DEFAULT,
    GHOST,
}



/*
 * 그림자는 -10000
 * 그외 기본 인덱스는 0~100에 있다.
 * 또한 위치에따라서 0~5000을 더하도록 하자. 5*5니까
 */
