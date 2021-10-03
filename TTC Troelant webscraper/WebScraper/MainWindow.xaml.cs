using System;
using System.Windows;

namespace WebScraper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Scraper scraper;
        public MainWindow()
        {
            InitializeComponent();
            scraper = new Scraper();
            DataContext = scraper;
        }

        private void ItemExport_Click(object sender, RoutedEventArgs e)
        {
            scraper.Export();
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnScraper_Click(object sender, RoutedEventArgs e)
        {
            BtnScraper.IsEnabled = false;
            scraper.Entries.Clear();
            try
            {
                scraper.Entries.Add(new EntryModel
                {
                    Data = $"Lidnummer;Naam;Club;Leeftijdscategorie;Klassement;Klassementswaarde"
                });
                //string pages = TbPage.Text;
                //int nrOfPages = Convert.ToInt32(pages);
                scraper.ScrapeData();
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.StackTrace);
                //TbPage.Text = "Een getal ingeven AUB";
            }

        }
    }
}
