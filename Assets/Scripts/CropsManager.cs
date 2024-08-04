using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class CropsManager : MonoBehaviour
{
    public Tilemap tilemap; // 참조할 타일 맵
    public Tilemap cropTilemap; // 작물을 생성할 타일 맵
    public Tile[] cropTiles; // 작물 타일
    public Tile tillageTile; // 경작 타일

    // 자라는 작물들 (좌표, 상태)
    Dictionary<Vector3Int, int> plantedCrops = new Dictionary<Vector3Int, int>();

    public Vector3 maxBounds; // 심을 수 있는 영역의 최소 경계
    public Vector3 minBounds; // 심을 수 있는 영역의 최대 경계

    // 수확한 수
    public int count;
    Transform PlayerPos;

    public TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        uiText.text = count.ToString();
        PlayerPos = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
            Debug.Log("테스트");
            // 캐릭터 중심으로 심기

            StartCoroutine(PlantAt());
            StartCoroutine(Tillage());
        }
    }


    IEnumerator Tillage()
	{
        yield return new WaitForSeconds(0.5f);
        Vector3Int position = tilemap.WorldToCell(PlayerPos.position);

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
            if (isWithinBounds(pos) && tile == null)
            {
                tilemap.SetTile(pos, tillageTile);
            }
        }
    }

    IEnumerator PlantAt()
	{
        yield return new WaitForSeconds(0.5f);
        // 플레이어 캐릭터 찾기
        Vector3Int position = tilemap.WorldToCell(PlayerPos.position);

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
            TileBase ctile = cropTilemap.GetTile(pos);

            // 타일이 빈 공간인지 확인하고 작물 심기
            if(isWithinBounds(pos) && tile != null && ctile == null)
		    {
                cropTilemap.SetTile(pos, cropTiles[0]);
                plantedCrops.Add(pos, 0);
                StartCoroutine(GrowCrops(pos, 3f));
		    }
            else if(ctile == cropTiles[5]) // 다 자란 감자
			{
                // 수확
                cropTilemap.SetTile(pos, null);
                tilemap.SetTile(pos, null);

                plantedCrops.Remove(pos);
                count++;
                uiText.text = count.ToString();
            }
		}
	}

    bool isWithinBounds(Vector3 position)
	{
        return position.x >= minBounds.x && position.x <= maxBounds.x &&
                position.y >= minBounds.y && position.y <= maxBounds.y &&
                position.z >= minBounds.z && position.z <= maxBounds.z;
    }

    // 해당 타일이 자라면..... 
    IEnumerator GrowCrops(Vector3Int pos, float growTime)
	{
        for(int i = 1; i<cropTiles.Length; i++)
		{
            yield return new WaitForSeconds(growTime);
            if (plantedCrops.ContainsKey(pos))
			{
                cropTilemap.SetTile(pos, cropTiles[i]);
                plantedCrops[pos] = i;
			}
		}
	}
}
