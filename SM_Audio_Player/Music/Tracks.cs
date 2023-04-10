﻿namespace SM_Audio_Player.Music;

/*
 * Klasa przechowuje informacje na danego utwor, które później umieszczane zostają w liście
 * TrackProperties.TrackList.
 */
public class Tracks
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Album { get; set; }
    public string Path { get; set; }
    public string Time { get; set; }

    public Tracks(int id, string ti, string auth, string alb, string pa, string tim)
    {
        Id = id;
        Title = ti;
        Author = auth;
        Album = alb;
        Path = pa;
        Time = tim;
    }
}