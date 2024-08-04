using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class CropsManager : MonoBehaviour
{
    public Tilemap tilemap; // ������ Ÿ�� ��
    public Tilemap cropTilemap; // �۹��� ������ Ÿ�� ��
    public Tile[] cropTiles; // �۹� Ÿ��
    public Tile tillageTile; // ���� Ÿ��

    // �ڶ�� �۹��� (��ǥ, ����)
    Dictionary<Vector3Int, int> plantedCrops = new Dictionary<Vector3Int, int>();

    public Vector3 maxBounds; // ���� �� �ִ� ������ �ּ� ���
    public Vector3 minBounds; // ���� �� �ִ� ������ �ִ� ���

    // ��Ȯ�� ��
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
            Debug.Log("�׽�Ʈ");
            // ĳ���� �߽����� �ɱ�

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
            // ĳ���� Ÿ�� �߽�
            TileBase tile = tilemap.GetTile(pos);

            // Ÿ���� �� �������� Ȯ���ϰ� �۹� �ɱ�
            if (isWithinBounds(pos) && tile == null)
            {
                tilemap.SetTile(pos, tillageTile);
            }
        }
    }

    IEnumerator PlantAt()
	{
        yield return new WaitForSeconds(0.5f);
        // �÷��̾� ĳ���� ã��
        Vector3Int position = tilemap.WorldToCell(PlayerPos.position);

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
            TileBase ctile = cropTilemap.GetTile(pos);

            // Ÿ���� �� �������� Ȯ���ϰ� �۹� �ɱ�
            if(isWithinBounds(pos) && tile != null && ctile == null)
		    {
                cropTilemap.SetTile(pos, cropTiles[0]);
                plantedCrops.Add(pos, 0);
                StartCoroutine(GrowCrops(pos, 3f));
		    }
            else if(ctile == cropTiles[5]) // �� �ڶ� ����
			{
                // ��Ȯ
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

    // �ش� Ÿ���� �ڶ��..... 
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
