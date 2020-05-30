using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Newtonsoft.Json;
using TweetTrends.Data_Access_Layer.Parsers;
using TweetTrends.Business_Layer;
using Newtonsoft.Json.Linq;
using TweetTrends.Data_Access_Layer;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using System.Net;
using TweetTrends.Service_Layer;
using System.Drawing;

namespace TweetTrends
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Service service;

        public MainWindow()
        {
            InitializeComponent();
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
        }

        private void mapView_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            mapView.MapProvider = OpenStreetMapProvider.Instance;
            mapView.MinZoom = 2;
            mapView.MaxZoom = 17;
            mapView.Zoom = 3;
            mapView.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            mapView.CanDragMap = true;
            mapView.DragButton = MouseButtons.Left;
            mapView.ShowCenter = false;
            PointLatLng center = new PointLatLng(50, -90);
            mapView.Position = center;

        }

        private void tweetsTopic_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String tweetsFilepath = null;
            string content = (String)((ComboBoxItem)tweetsTopic_ComboBox.SelectedItem).Content;
            switch (content)
            {
                case "Cali":
                    {
                        tweetsFilepath = "Data/cali_tweets2014.txt";
                        break;
                    }
                case "Family":
                    {
                        tweetsFilepath = "Data/family_tweets2014.txt";
                        break;
                    }
                case "Football":
                    {
                        tweetsFilepath = "Data/football_tweets2014.txt";
                        break;
                    }
                case "High school":
                    {
                        tweetsFilepath = "Data/high_school_tweets2014.txt";
                        break;
                    }
                case "Movie":
                    {
                        tweetsFilepath = "Data/movie_tweets2014.txt";
                        break;
                    }
                case "Shopping":
                    {
                        tweetsFilepath = "Data/shopping_tweets2014.txt";
                        break;
                    }
                case "Snow":
                    {
                        tweetsFilepath = "Data/snow_tweets2014.txt";
                        break;
                    }
                case "Texas":
                    {
                        tweetsFilepath = "Data/texas_tweets2014.txt";
                        break;
                    }
                case "Weekend":
                    {
                        tweetsFilepath = "Data/weekend_tweets2014.txt";
                        break;
                    }
            }

            service = new Service(tweetsFilepath);
            DrawPolygons();

        }

        private void DrawPolygons()
        {
            mapView.Overlays.Clear();
            GMapOverlay overlay = new GMapOverlay("Polygons");
            List<GMapPolygon> polygons = service.GetPolygons();
            SetPolygonsColors(polygons);

            foreach (GMapPolygon polygon in polygons)
            {
                overlay.Polygons.Add(polygon);
            }

            overlay.IsVisibile = true;
            mapView.Overlays.Add(overlay);    
        }

        private void SetPolygonsColors(List<GMapPolygon> polygons)
        {
            Dictionary<string, State> states = service.GetStates();
            foreach (GMapPolygon polygon in polygons)
            {
                polygon.Stroke = new Pen(Color.Black, 1);
                polygon.Fill = new SolidBrush(DetermineStateColor(states[polygon.Name]));
            }
        }

        private Color DetermineStateColor(State state)
        {
            if (state.Tweets == null || state.Tweets.Count < 1) return Color.Gray;
            if (state.Weight == 0) return Color.White;
            if (state.Weight > 0) return Color.FromArgb(200, 0, 255 - (int)(state.Weight * 255), 0);
            if (state.Weight < 0) return Color.FromArgb(200, 255 - (int)(Math.Abs(state.Weight) * 255), 0, 0);

            return Color.Empty;
        }

    }
}
