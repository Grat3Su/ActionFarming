using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropsManager : MonoBehaviour
{
    Tilemap tilemap; // 참조할 타일 맵
    Tilemap cropTilemap; // 작물을 생성할 타일 맵
    Tile cropTile; // 작물 타일
    Tile tillageTile; // 경작 타일

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(1))
		{
            // 캐릭터 중심으로 심기
            PlantAt();
            Tillage();
        }
    }


    void Tillage()
	{
        Vector3 PlayerPos = GameObject.Find("Player").transform.position;
        Vector3Int position = tilemap.WorldToCell(PlayerPos);

        List<Vector3Int> list = new List<Vector3Int>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
                list.Add(new Vector3Int(position.x - 1 + i, position.y - 1 + j, position.z));
        }

        foreach (var pos in list)
        {
            // 캐릭터 타일 중심
            TileBase tile = tilemap.GetTile(pos);

            // 타일이 빈 공간인지 확인하고 작물 심기
            if (tile == null)
            {
                tilemap.SetTile(pos, tillageTile);
            }
            else
            {
                Debug.Log("해당 위치에 이미 타일이 있음");
            }
        }
    }

    void PlantAt()
	{
        // 플레이어 캐릭터 찾기
        Vector3 PlayerPos = GameObject.Find("Player").transform.position;
        Vector3Int position = tilemap.WorldToCell(PlayerPos);

        List<Vector3Int> list = new List<Vector3Int>();

        for(int i = 0; i<3; i++)
		{
            for(int j = 0; j<3; j++)
                list.Add(new Vector3Int(position.x-1+i, position.y-1+j, position.z));
        }

        foreach(var pos in list)
		{
            // 캐릭터 타일 중심
            TileBase tile = tilemap.GetTile(pos);
            TileBase ctile = tilemap.GetTile(pos);

            // 타일이 빈 공간인지 확인하고 작물 심기
            if(tile != null && ctile == null)
		    {
                cropTilemap.SetTile(pos, cropTile);
		    }
		    else
		    {
                Debug.Log("해당 위치에 이미 타일이 있음");
		    }
		}
	}
}
