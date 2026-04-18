using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem; // Requis pour Keyboard.current
using System.Collections.Generic;
using System.IO;

public class LevelExtractor : MonoBehaviour
{
    [Header("Configuration Vidéo")]
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;

    [Header("Points de détection (Coordonnées 0 à 1)")]
    [Tooltip("X et Y entre 0 et 1. Exemple: 0.5 est le milieu.")]
    public Vector2[] detectionPoints = new Vector2[4]; 
    
    [Header("Couleurs Cibles")]
    public Color colorVoie1 = Color.blue;
    public Color colorVoie2 = Color.red;
    public Color colorVoie3 = Color.green;
    public Color colorVoie4 = new Color(1f, 0f, 1f); // Rose / Magenta

    [Header("Paramètres d'Analyse")]
    [Range(0f, 1f)] public float tolerance = 0.2f;
    public string fileName = "Niveau_Expert_Data.txt";

    private List<float> beatTimes = new List<float>();
    private List<int> lanes = new List<int>();
    private bool[] isDetecting = new bool[4];

    void Update()
    {
        // 1. On vérifie si on doit sauvegarder (Touche S)
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            ExportToFile();
            return;
        }

        if (!videoPlayer.isPlaying) return;

        // 2. Extraction des pixels de la RenderTexture
        Texture2D tempTex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tempTex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tempTex.Apply();

        Color[] targetColors = { colorVoie1, colorVoie2, colorVoie3, colorVoie4 };

        // 3. Analyse des 4 points de détection
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

        // Nettoyage pour éviter les fuites de mémoire (très important ici)
        RenderTexture.active = null;
        Destroy(tempTex);
    }

    bool IsColorMatch(Color pixel, Color target)
    {
        // Calcul de la différence de couleur (Distance Manhattan)
        float diff = Mathf.Abs(pixel.r - target.r) + Mathf.Abs(pixel.g - target.g) + Mathf.Abs(pixel.b - target.b);
        return diff < tolerance;
    }

    void RecordData(int lane)
    {
        float time = (float)System.Math.Round(videoPlayer.time, 2);
        beatTimes.Add(time);
        lanes.Add(lane);
        
        string hex = ColorUtility.ToHtmlStringRGB(GetColorForLane(lane));
        Debug.Log($"<color=#{hex}>[CAPTURÉ]</color> Voie {lane} à {time}s");
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
            writer.WriteLine("// Données générées le " + System.DateTime.Now);
            writer.WriteLine("// Format : Bleu=1, Rouge=2, Vert=3, Rose=4");
            writer.WriteLine("");

            // Tableau des Timings
            string times = "private float[] beatTimes1 = { ";
            foreach (float t in beatTimes) times += t.ToString("F2").Replace(",", ".") + "f, ";
            times += "};";
            writer.WriteLine(times);

            writer.WriteLine("");

            // Tableau des Emplacements
            string positions = "private int[] emplacementsManquants1 = { ";
            foreach (int l in lanes) positions += l + ", ";
            positions += "};";
            writer.WriteLine(positions);
        }

        Debug.Log($"<color=cyan><b>SUCCÈS :</b> Fichier créé dans Assets/{fileName}</color>");
        videoPlayer.Pause(); // On met en pause pour confirmer la fin
    }
}