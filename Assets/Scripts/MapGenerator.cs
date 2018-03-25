using UnityEngine;

public class MapGenerator : MonoBehaviour {

    //SINGLETON
    public static MapGenerator INSTANCE;

    // MAP INFORMATIONS
    private float m_left = -12;
    private float m_right = 12;
    private float m_bottom = -8;
    private float m_top = 8;

    /*[SerializeField]
    private int m_subdivision = 2;*/


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

                if (sample > 0.75)
                {
                    GameManager.INSTANCE.SpawnObject(GameManager.INSTANCE.m_spawnableBush, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
                }
            }
        }

    }

}
