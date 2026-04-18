using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.IO;

public class LevelExtractor : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;

    [Header("Points de détection (0 à 1)")]
    public Vector2[] detectionPoints = new Vector2[4]; 
    
    [Header("Couleurs Cibles")]
    public Color colorVoie1 = Color.blue;
    public Color colorVoie2 = Color.red;
    public Color colorVoie3 = Color.green;
    public Color colorVoie4 = new Color(1f, 0f, 1f);

    [Header("Filtre Anti-Bruit (Important)")]
    [Range(0f, 1f)] public float tolerance = 0.25f;
    [Tooltip("Nombre d'images suivies pour confirmer une note (3-5 conseillé)")]
    public int framesDeConfirmation = 4;
    [Tooltip("Temps mort entre deux notes (0.15 = 150ms)")]
    public float cooldownNote = 0.15f;

    private List<float> beatTimes = new List<float>();
    private List<int> lanes = new List<int>();
    
    // Variables de suivi interne
    private bool[] isCurrentlyDetecting = new bool[4];
    private int[] consecutiveFrames = new int[4];
    private float[] potentialStartTime = new float[4];
    private float[] lastRecordedTime = new float[4];

    void Start()
    {
        videoPlayer.skipOnDrop = false;
        for (int i = 0; i < 4; i++) lastRecordedTime[i] = -10f;
    }

    void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame) { ExportToFile(); return; }
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
                // ÉTAPE 1 : C'est la première fois qu'on voit la couleur
                if (consecutiveFrames[i] == 0)
                {
                    // On mémorise la frame exacte du début au cas où c'est une vraie note
                    potentialStartTime[i] = (float)(videoPlayer.frame / videoPlayer.frameRate);
                }

                consecutiveFrames[i]++;

                // ÉTAPE 2 : On a assez de frames pour confirmer (ex: 4 frames d'affilée)
                if (consecutiveFrames[i] >= framesDeConfirmation && !isCurrentlyDetecting[i])
                {
                    // ÉTAPE 3 : On vérifie si ce n'est pas un doublon (cooldown)
                    if ((potentialStartTime[i] - lastRecordedTime[i]) > cooldownNote)
                    {
                        RecordData(i + 1, potentialStartTime[i]);
                        lastRecordedTime[i] = potentialStartTime[i];
                        isCurrentlyDetecting[i] = true;
                    }
                }
            }
            else
            {
                // La couleur a disparu : on reset les compteurs pour cette voie
                consecutiveFrames[i] = 0;
                isCurrentlyDetecting[i] = false;
            }
        }

        RenderTexture.active = null;
        Destroy(tempTex);

        // Auto-save
        if (videoPlayer.frame >= (long)videoPlayer.frameCount - 1 && videoPlayer.frameCount > 0)
        {
            ExportToFile();
            videoPlayer.Stop();
        }
    }

    bool IsColorMatch(Color pixel, Color target)
    {
        return Mathf.Abs(pixel.r - target.r) + Mathf.Abs(pixel.g - target.g) + Mathf.Abs(pixel.b - target.b) < tolerance;
    }

    void RecordData(int lane, float time)
    {
        beatTimes.Add((float)System.Math.Round(time, 3));
        lanes.Add(lane);
        Debug.Log($"<color=green>STABLE :</color> Voie {lane} détectée à {time:F3}s");
    }

    void ExportToFile()
    {
        string path = Path.Combine(Application.dataPath, "Niveau_1_Data.txt");
        using (StreamWriter writer = new StreamWriter(path))
        {
            // Join permet d'éviter la virgule en trop à la fin
            string timesStr = "private float[] beatTimes1 = { " + string.Join("f, ", beatTimes.ConvertAll(t => t.ToString("F3").Replace(",", "."))) + "f };";
            string lanesStr = "private int[] emplacementsManquants1 = { " + string.Join(", ", lanes) + " };";
            writer.WriteLine(timesStr);
            writer.WriteLine("");
            writer.WriteLine(lanesStr);
        }
        Debug.Log("Fichier exporté proprement !");
    }
}