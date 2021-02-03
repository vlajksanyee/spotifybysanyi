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
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO;

namespace sanyispotify
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MediaPlayer mplayer = new MediaPlayer();
		public MainWindow()
		{
			InitializeComponent();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog save = new SaveFileDialog();
			save.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			save.ShowDialog();
			StreamWriter sw;
			try
			{
				sw = new StreamWriter(save.FileName);
			}
			catch (Exception)
			{
				return;
			}
			for (int i = 0; i < kukanc.Items.Count; i++)
			{
				sw.WriteLine(kukanc.Items[i]);
			}
			sw.Close();
		}

		private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			open.ShowDialog();
			StreamReader sr;
			try
			{
				sr = new StreamReader(open.FileName);
			}
			catch (Exception)
			{
				return;
			}
			while (!sr.EndOfStream)
			{
				kukanc.Items.Add(sr.ReadLine());
			}
		}

		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				mplayer.Open(new Uri(Convert.ToString(kukanc.Items[whichMusic]), UriKind.Relative));
				mplayer.Play();
				mplayer.Volume = volumeSlider.Value;
				ChangeSliderPosition();
			}
			catch (Exception)
			{
				return;
			}
		}

		private void PauseButton_Click(object sender, RoutedEventArgs e)
		{
			mplayer.Pause();
		}

		private void ResumeButton_Click(object sender, RoutedEventArgs e)
		{
			mplayer.Play();
		}

		private void StopButton_Click(object sender, RoutedEventArgs e)
		{
			mplayer.Stop();
			mplayer.Close();
			progressSlider.Value = 0;
		}

		public void ChangeSliderPosition()
		{
			{
				DispatcherTimer timer = new DispatcherTimer();
				timer.Interval = TimeSpan.FromSeconds(0.5);
				timer.Tick += CheckProgress;
				timer.Start();
				void CheckProgress(object s, EventArgs ea)
				{
					if (mplayer.Source != null)
					{
						try
						{
							progressSlider.Minimum = 0;
							progressSlider.Maximum = mplayer.NaturalDuration.TimeSpan.TotalSeconds;
							progressSlider.Value = mplayer.Position.TotalSeconds;
						}
						catch (Exception)
						{
							return;
						}

						if (mplayer.Position.TotalSeconds == mplayer.NaturalDuration.TimeSpan.TotalSeconds)
						{
							NextMusic();
							mplayer.Open(new Uri(Convert.ToString(kukanc.Items[whichMusic]), UriKind.Relative));
							mplayer.Play();
						}
					}
				}
			}
		}

		private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			double volume = volumeSlider.Value;
			mplayer.Volume = volume;
		}

		private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			mplayer.Position = TimeSpan.FromSeconds(progressSlider.Value);
		}

		private void ImportButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "MP3 Files (*.mp3)|*.mp3|All Files (*.*)|*.*";
			open.ShowDialog();
			if (!kukanc.Items.Contains(open.FileName))
			{
				kukanc.Items.Add(open.FileName);
			}
			else
			{
				MessageBox.Show("Ez a zene már a listán van!");
			}
		}

		int whichMusic = -5;
		private void Kukanc_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			whichMusic = 0;
			for (int i = 0; i < kukanc.Items.Count; i++)
			{
				if (kukanc.Items[i] != kukanc.SelectedItem)
				{
					whichMusic++;
				}
				else
				{
					break;
				}
			}
		}

		private void PreviousButton_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < kukanc.Items.Count; i++)
			{
				if (kukanc.Items[i] == kukanc.SelectedItem)
					try
					{
						kukanc.SelectedItem = kukanc.Items[i - 1];
						break;
					}
					catch (Exception)
					{
						return;
					}
			}
		}

		public void NextMusic()
		{
			for (int i = 0; i < kukanc.Items.Count; i++)
			{
				if (kukanc.Items[i] == kukanc.SelectedItem)
					try
					{
						kukanc.SelectedItem = kukanc.Items[i + 1];
						break;
					}
					catch (Exception)
					{
						return;
					}
			}
		}

		private void NextButton_Click(object sender, RoutedEventArgs e)
		{
			NextMusic();
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			kukanc.Items.Remove(kukanc.SelectedItem);
		}
	}
}
