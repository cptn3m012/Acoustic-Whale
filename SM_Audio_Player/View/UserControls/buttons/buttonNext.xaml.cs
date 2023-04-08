﻿using NAudio.Wave;
using SM_Audio_Player.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SM_Audio_Player.View.UserControls.buttons
{
    public partial class buttonNext : UserControl
    {
        public delegate void NextButtonClickedEventHandler(object sender, EventArgs e);
        public static event NextButtonClickedEventHandler NextButtonClicked;

        private buttonPlay btnPlay = new buttonPlay();
        public buttonNext()
        {
            InitializeComponent();
        }

        /*Włącz następny utwór*/
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TracksProperties.isSchuffleOn)
                {
                    btnPlay.SchuffleFun();
                }
                else
                {
                    // Sprawdzanie czy to nie był ostatni utwór na liście
                    if (TracksProperties.SelectedTrack.Id != TracksProperties.tracksList.Count)
                    {
                        TracksProperties.SelectedTrack =
                            TracksProperties.tracksList.ElementAt(TracksProperties.SelectedTrack.Id);
                        btnPlay.PlayNewTrack();
                    }
                    else
                    {
                        // Przełączenie na 1 utwór z listy po zakończeniu ostatniego
                        TracksProperties.SelectedTrack = TracksProperties.tracksList.ElementAt(0);
                        btnPlay.PlayNewTrack();
                    }
                }
                NextButtonClicked?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Next button error!");
            }
        }
    }
}
