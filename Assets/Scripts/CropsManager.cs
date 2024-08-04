using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropsManager : MonoBehaviour
{
    Tilemap tilemap; // ������ Ÿ�� ��
    Tilemap cropTilemap; // �۹��� ������ Ÿ�� ��
    Tile cropTile; // �۹� Ÿ��
    Tile tillageTile; // ���� Ÿ��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(1))
		{
            // ĳ���� �߽����� �ɱ�
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
            // ĳ���� Ÿ�� �߽�
            TileBase tile = tilemap.GetTile(pos);

            // Ÿ���� �� �������� Ȯ���ϰ� �۹� �ɱ�
            if (tile == null)
            {
                tilemap.SetTile(pos, tillageTile);
            }
            else
            {
                Debug.Log("�ش� ��ġ�� �̹� Ÿ���� ����");
            }
        }
    }

    void PlantAt()
	{
        // �÷��̾� ĳ���� ã��
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
            // ĳ���� Ÿ�� �߽�
            TileBase tile = tilemap.GetTile(pos);
            TileBase ctile = tilemap.GetTile(pos);

            // Ÿ���� �� �������� Ȯ���ϰ� �۹� �ɱ�
            if(tile != null && ctile == null)
		    {
                cropTilemap.SetTile(pos, cropTile);
		    }
		    else
		    {
                Debug.Log("�ش� ��ġ�� �̹� Ÿ���� ����");
		    }
		}
	}
}
