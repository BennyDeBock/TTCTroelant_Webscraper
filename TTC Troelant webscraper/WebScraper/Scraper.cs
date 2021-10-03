
using CsvHelper;
using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace WebScraper
{
    enum KlassementWaarde
    {
        A = 19,
        B0 = 18,
        B2 = 17,
        B4 = 16,
        B6 = 15,
        C0 = 14,
        C2 = 13,
        C4 = 12,
        C6 = 11,
        D0 = 10,
        D2 = 9,
        D4 = 8,
        D6 = 7,
        E0 = 6,
        E2 = 5,
        E4 = 4,
        E6 = 3,
        F = 2,
        NG = 1
    }
    public class Scraper
    {
        private ObservableCollection<EntryModel> _entries = new ObservableCollection<EntryModel>();

        public ObservableCollection<EntryModel> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }

        /*public void ScrapeData(int nrOfPages)
        {
            string html;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;

            for (int i = 1; i <= nrOfPages; i++)
            {
                html = "https://ttonline.sporta.be/?province=4&menu=6&club_id=0&cur_page=" + " " + i;
                doc = web.Load(html); //Load the webpage

                //Geef alle leden die de klasse hebben en een ID
                var Leden = doc.DocumentNode.SelectNodes("//*[@class='DBTable selectable' and @id]");

                foreach (var lid in Leden)
                {
                    var lidnummer =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Lidnummer']").InnerText);
                    var naam =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Naam']").InnerText);
                    var voornaam =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Voornaam']").InnerText);
                    var klassement =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Kl.']").InnerText);
                    var club =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Club']").InnerText);
                    var klassementwaarde = (int)Enum.Parse(typeof(KlassementWaarde), klassement);
                    /*_entries.Add(new EntryModel { 
                        Lidnummer = lidnummer, 
                        Naam = naam, 
                        Voornaam = voornaam, 
                        Klassement = klassement, 
                        Club = club 
                    });*/

                    /*_entries.Add(new EntryModel
                    {
                        Data = $"{lidnummer};{naam} {voornaam};{club};;{klassement};{klassementwaarde}"
                    });

                    Debug.Print($"Lidnummer: {lidnummer} \n" +
                        $"Naam: {naam} \n" +
                        $"Voornaam: {voornaam} \n" +
                        $"Klassement: {klassement} \n" +
                        $"Club: {club} \n");
                }
            }


        }*/

        public void ScrapeData()
        {
            string html;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;

            html = "https://ttonline.sporta.be/?province=4&menu=6&club_id=0&cur_page=1";
            doc = web.Load(html); //Load the webpage

            var paging = doc.DocumentNode.SelectSingleNode("//*[@class='DBTable_Paging']");


            string pages = "";
            foreach (HtmlNode node in paging.SelectNodes(".//a[@class='DBTable']"))
            {
                if (node.InnerText == "&nbsp;[Volgende]&nbsp;")
                    continue;

                pages = node.InnerText;

            }

            pages = pages.Replace("&nbsp;", "");
            int pageTotal = int.Parse(pages);

            for (int i = 1; i <= pageTotal; i++)
            {
                html = "https://ttonline.sporta.be/?province=4&menu=6&club_id=0&cur_page=" + " " + i;
                doc = web.Load(html); //Load the webpage

                //Geef alle leden die de klasse hebben en een ID
                var Leden = doc.DocumentNode.SelectNodes("//*[@class='DBTable selectable' and @id]");

                foreach (var lid in Leden)
                {
                    var lidnummer =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Lidnummer']").InnerText);
                    var naam =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Naam']").InnerText);
                    var voornaam =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Voornaam']").InnerText);
                    var klassement =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Kl.']").InnerText);
                    var club =
                        HttpUtility.HtmlDecode(
                            lid.SelectSingleNode(".//td[@field = 'Club']").InnerText);
                    var klassementwaarde = (int)Enum.Parse(typeof(KlassementWaarde), klassement);
                    /*_entries.Add(new EntryModel { 
                        Lidnummer = lidnummer, 
                        Naam = naam, 
                        Voornaam = voornaam, 
                        Klassement = klassement, 
                        Club = club 
                    });*/

                    _entries.Add(new EntryModel
                    {
                        Data = $"{lidnummer};{naam} {voornaam};{club};;{klassement};{klassementwaarde}"
                    });

                    Debug.Print($"Lidnummer: {lidnummer} \n" +
                        $"Naam: {naam} \n" +
                        $"Voornaam: {voornaam} \n" +
                        $"Klassement: {klassement} \n" +
                        $"Club: {club} \n");

                    Debug.Print("" + _entries.Count);
                }
            }


        }

        public void Export()
        {
            using (TextWriter tw = File.CreateText("ledenlijst.csv"))
            {
                using (var cw = new CsvWriter(tw, System.Globalization.CultureInfo.GetCultureInfo("nl-BE")))
                {
                    cw.Configuration.ShouldQuote = (field, context) => false;

                    foreach (var entry in Entries)
                    {
                        cw.WriteRecord(entry);
                        cw.NextRecord();
                    }
                }
            }
        }

    }
}
