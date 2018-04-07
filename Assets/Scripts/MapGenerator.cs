using UnityEngine;

public class MapGenerator : MonoBehaviour {

    //SINGLETON
    public static MapGenerator INSTANCE;


    // TODO: Get the Map Infos with the borders in the Scene
    void Start () {
        #region  SINGLETON
        if (INSTANCE != null && INSTANCE != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            INSTANCE = this;
            DontDestroyOnLoad(this);
        }
        #endregion

    }

    public void GenerateMap()
    {
        float offsetX = Random.Range(0f, 3f);
        float offsetY = Random.Range(0f, 3f);
        //Mathf.PerlinNoise(float x, float y)
        for (int x=-11; x<12; x++)
        {
            for(int y=-7; y<8; y++)
            {
                float coX = ((float)x+12f)/24f * 20 + offsetX;
                float coY = ((float)y+8f)/16f  *20 + offsetY;

                float sample = Mathf.PerlinNoise(coX, coY);

                if (sample > 0.7)
                {
                    //random rotation for the bush
                    Quaternion bush_rot = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

                    float spawnableType = Random.Range(0f, 1f);
                    if (spawnableType > 0.1f)
                    {
                        //Spawn a bush
                        GameManager.INSTANCE.SpawnObject(GameManager.INSTANCE.m_spawnableBush, new Vector3(x, y, 0), bush_rot);
                    }
                    else
                    {
                        //Spawn a rock
                        GameManager.INSTANCE.SpawnObject(GameManager.INSTANCE.m_spawnableRock, new Vector3(x, y, 0), bush_rot);
                    }
                    
                    
                }
            }
        }

    }

}
