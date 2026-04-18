using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;
using System.IO;

public class LevelExtractor : MonoBehaviour
{
    [Header("Configuration Vidéo")]
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;

    [Header("Points de détection (0 à 1)")]
    public Vector2[] detectionPoints = new Vector2[4]; 
    
    [Header("Couleurs Cibles")]
    public Color colorVoie1 = Color.blue;
    public Color colorVoie2 = Color.red;
    public Color colorVoie3 = Color.green;
    public Color colorVoie4 = new Color(1f, 0f, 1f); // Rose / Magenta

    [Range(0f, 1f)] public float tolerance = 0.15f;
    public string fileName = "Niveau_Expert_Data.txt";

    private List<float> beatTimes = new List<float>();
    private List<int> lanes = new List<int>();
    private bool[] isDetecting = new bool[4];

    void Update()
    {
        if (!videoPlayer.isPlaying) return;

        Texture2D tempTex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tempTex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tempTex.Apply();

        Color[] targetColors = { colorVoie1, colorVoie2, colorVoie3, colorVoie4 };

        for (int i = 0; i < 4; i++)
        {
            int px = (int)(detectionPoints[i].x * renderTexture.width);
            int py = (int)(detectionPoints[i].y * renderTexture.height);

            Color pixelColor = tempTex.GetPixel(px, py);

            if (IsColorMatch(pixelColor, targetColors[i]))
            {
                if (!isDetecting[i])
                {
                    RecordData(i + 1);
                    isDetecting[i] = true;
                }
            }
            else
            {
                isDetecting[i] = false;
            }
        }

        RenderTexture.active = null;
        Destroy(tempTex);

        if (Input.GetKeyDown(KeyCode.S)) ExportToFile();
    }

    bool IsColorMatch(Color pixel, Color target)
    {
        // On calcule la distance entre les couleurs
        float diff = Mathf.Abs(pixel.r - target.r) + Mathf.Abs(pixel.g - target.g) + Mathf.Abs(pixel.b - target.b);
        return diff < tolerance;
    }

    void RecordData(int lane)
    {
        float time = (float)System.Math.Round(videoPlayer.time, 2);
        beatTimes.Add(time);
        lanes.Add(lane);
        
        // Debug avec couleur pour s'y retrouver dans la console
        string colorHex = ColorUtility.ToHtmlStringRGB(GetColorForLane(lane));
        Debug.Log($"<color=#{colorHex}>Capté Voie {lane}</color> à {time}s");
    }

    Color GetColorForLane(int lane) {
        if(lane == 1) return colorVoie1;
        if(lane == 2) return colorVoie2;
        if(lane == 3) return colorVoie3;
        return colorVoie4;
    }

    void ExportToFile()
    {
        string path = Application.dataPath + "/" + fileName;
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("// Données extraites - Couleurs : B=1, R=2, V=3, P=4");
            
            string times = "private float[] beatTimes1 = { ";
            foreach (float t in beatTimes) times += t.ToString("F2").Replace(",", ".") + "f, ";
            times += "};";
            writer.WriteLine(times);

            string positions = "private int[] emplacementsManquants1 = { ";
            foreach (int l in lanes) positions += l + ", ";
            positions += "};";
            writer.WriteLine(positions);
        }
        Debug.Log("<color=white><b>Fichier sauvegardé dans Assets/</b></color>" + fileName);
        videoPlayer.Stop();
    }
}