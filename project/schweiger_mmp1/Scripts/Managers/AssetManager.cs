/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Audio;
using SFML.Graphics;
using System;

class AssetManager
{
    private static AssetManager? instance;
    public static AssetManager Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }
    private AssetManager()
    {
        ASSETFOLDER = Settings.GAMEFOLDER + "Assets/";
    }

    public string ASSETFOLDER;

    public readonly Dictionary<string, Texture> textures = new();
    public readonly Dictionary<string, SoundBuffer> soundBuffers = new();
    public readonly Dictionary<string, Music> music = new();
    public readonly Dictionary<string, Font> fonts = new();


    public void LoadTexture(string name, string fileName) =>
        textures.Add(name, new Texture($"{ASSETFOLDER}Textures/{fileName}"));

    public void LoadSoundBuffers(string name, string fileName) =>
        soundBuffers.Add(name, new SoundBuffer($"{ASSETFOLDER}SoundBuffers/{fileName}"));

    public void LoadManySoundBuffers(string name, int count)
    {
        for (int i = 1; i <= count; i++) LoadSoundBuffers($"{name}{i}", $"{name}{i}.wav");
    }

    public void LoadMusic(string name, string fileName) =>
        music.Add(name, new Music($"{ASSETFOLDER}Music/{fileName}"));

    public void LoadFonts(string name, string fileName) =>
        fonts.Add(name, new Font($"{ASSETFOLDER}Fonts/{fileName}"));
}