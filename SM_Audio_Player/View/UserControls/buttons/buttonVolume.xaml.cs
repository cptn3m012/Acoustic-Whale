﻿using Newtonsoft.Json;
using SM_Audio_Player.Music;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace SM_Audio_Player.View.UserControls.buttons;

public partial class ButtonVolume : INotifyPropertyChanged
{
    private bool _isMuted;
    private double _savedVolumeValue;
    private double _currentVolumeValue;
    private string? _volumeIcon;


    private const string JsonPath = @"MusicVolumeJSON.json";
    public event PropertyChangedEventHandler? PropertyChanged;

    public ButtonVolume()
    {
        try
        {
            DataContext = this;
            InitializeComponent();
            if (File.Exists(JsonPath))
                _currentVolumeValue = ReadVolumeFromJsonFile();
            else
                _currentVolumeValue = 50;

            sldVolume.Value = _currentVolumeValue;
            VolumeIcon = ValueIconChange(sldVolume.Value);
            ButtonNext.NextButtonClicked += OnTrackSwitch;
            ButtonPrevious.PreviousButtonClicked += OnTrackSwitch;
            ButtonPlay.TrackEnd += OnTrackSwitch;
            Library.DoubleClickEvent += OnTrackSwitch;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"ButtonVolume constructor exception: {ex.Message}");
            throw;
        }
    }

    public string? VolumeIcon
    {
        get => _volumeIcon;
        set
        {
            _volumeIcon = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VolumeIcon"));
        }
    }

    /*Metoda sprawdzająca aktualną wartość slidera i na jej podstawie ustawiająca ikonkę*/
    private string? ValueIconChange(double volumeValue)
    {
        try
        {
            if (volumeValue == 0)
            {
                VolumeIcon = Icons.GetVolumeIconZero();
                return Icons.GetVolumeIconZero();
            }
            else if (volumeValue > 0 && volumeValue <= 40)
            {
                VolumeIcon = Icons.GetVolumeIconLow();
                return Icons.GetVolumeIconLow();
            }
            else if (volumeValue > 40 && volumeValue <= 75)
            {
                VolumeIcon = Icons.GetVolumeIconHalf();
                return Icons.GetVolumeIconHalf();
            }
            else if (volumeValue > 75)
            {
                VolumeIcon = Icons.GetVolumeIconMax();
                return Icons.GetVolumeIconMax();
            }

            return Icons.GetVolumeIconHalf();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"ValueIconChange exception: {ex.Message}");
            throw;
        }
    }

    /*Wycisz/Zmień poziom głośności*/
    private void btnVolume_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_isMuted && _savedVolumeValue != 0)
            {
                sldVolume.Value = _savedVolumeValue;
                ValueIconChange(_currentVolumeValue);
                _isMuted = false;
            }
            else
            {
                _savedVolumeValue = sldVolume.Value;
                sldVolume.Value = 0;
                VolumeIcon = Icons.GetVolumeIconZero();
                _isMuted = true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"btnVolume_Click exception: {ex.Message}");
            throw;
        }
    }

    private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        try
        {
            /*Pobieranie aktualnej wartości slidera*/
            _currentVolumeValue = e.NewValue;
            ValueIconChange(_currentVolumeValue);

            if (TracksProperties.AudioFileReader != null)
            {
                var sliderValue = _currentVolumeValue / 100.0; // Skalowanie wartości na zakres od 0 do 1
                var newVolume = sliderValue; // Obliczamy nową wartość głośności
                TracksProperties.AudioFileReader.Volume = (float)newVolume; // Aktualizujemy głośność pliku audio

                // Zapis do pliku JSON w celu ponownego odpalenia aplikacji z zapisaną wartością głośności
                var output = JsonConvert.SerializeObject(_currentVolumeValue, Formatting.Indented);
                File.WriteAllText(JsonPath, output);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Volume change error: {ex.Message}");
            throw;
        }
    }

    public double ReadVolumeFromJsonFile()
    {
        try
        {
            double volume = 50;
            var json = File.ReadAllText(JsonPath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json) ?? 50;
            if (jsonObj != null)
                volume = jsonObj;
            return volume;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"ReadValueJson error: {ex.Message}");
            return 0;
        }
    }

    // Aktualizacja głośności po zmienionym tracku
    private void OnTrackSwitch(object sender, EventArgs e)
    {
        try
        {
            ValueIconChange(_currentVolumeValue);

            if (TracksProperties.AudioFileReader != null)
            {
                var sliderValue = _currentVolumeValue / 100.0; // Skalowanie wartości na zakres od 0 do 1
                var newVolume = sliderValue; // Obliczamy nową wartość głośności
                TracksProperties.AudioFileReader.Volume = (float)newVolume; // Aktualizujemy głośność pliku audio
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Volume change error: {ex.Message}");
        }
    }
}