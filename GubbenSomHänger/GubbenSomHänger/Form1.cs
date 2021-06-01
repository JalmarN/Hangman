using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Linq;
using System.Text; //Denna använder jag för att kunna komma åt med min Encoding så att jag kan visa ÅÄÖ
using System.Threading.Tasks;
using System.IO; //För att kunna nå txt filen jag läser listan ifrån behöver jag anvädna mig av System.IO
using System.Windows.Forms;

namespace GubbenSomHänger
{
    public partial class HangaGubbe : Form
    {
        

        private Bitmap[] Bilder = { GubbenSomHänger.Properties.Resources.Bild01, GubbenSomHänger.Properties.Resources.Bild02, GubbenSomHänger.Properties.Resources.Bild03, GubbenSomHänger.Properties.Resources.Bild04, GubbenSomHänger.Properties.Resources.Bild05, GubbenSomHänger.Properties.Resources.Bild06, GubbenSomHänger.Properties.Resources.Bild07, GubbenSomHänger.Properties.Resources.Bild08, GubbenSomHänger.Properties.Resources.Bild09, };
        //Denna använder jag för att hämta bildrna från min resurs fil som ligger på min forms egenskaper.
        private int Fel = 0;
        private string RndOrd = "";
        int FelGissning = 8;
        List<Label> HmlRndOrd = new List<Label>();
          //står för "hemligt random ord" och används till att från min cvs fil hämta ett slumpvalt ord som sätts bakom _ tecken.

        public HangaGubbe() //Den här laddar in komponenterna som det random ordet och lablen med hur många liv jag har kvar när man startar programmet.
        {
            InitializeComponent();
            HangmanLexicon();
            LivKvar();
            Liv.BringToFront();
        }

        void LivKvar() //bara lablen med antal liv.
        {
            Liv.Text = ("Du har: " + FelGissning + " Liv kvar!");
        }

        private string OrdLista() //Den här läser av cvs filen och delar upp orden så att jag lätt kan få tag på dem eftersom dem nu är uppdelade.
        {
            string OrdLista = File.ReadAllText("hangmanlexicon.cvs", Encoding.Default);
            string[] Ord = OrdLista.Split('\n');
            Random Rnd = new Random();
            return Ord[Rnd.Next(0, Ord.Length)];
        }


        void HangmanLexicon() //Här är min medot för att hämta ett slumpmässigt ord från en cvs list a som jag har i bin filen på mitt projekt.
        {
            RndOrd = OrdLista();
            char[] chars = RndOrd.ToCharArray();
            int mellanrum = 220 / chars.Length - 1;
            for (int i = 0; i < chars.Length - 2; i++)
            {
                HmlRndOrd.Add(new Label());
                HmlRndOrd[i].Location = new Point((i * mellanrum) + 10, 290);
                HmlRndOrd[i].Text = "_";
                HmlRndOrd[i].Parent = groupBox1;
                HmlRndOrd[i].BringToFront();
                HmlRndOrd[i].CreateControl();

            }
       
        }


        private void Bokstav_Click(object sender, EventArgs e) //Denna registrerar varje gång jag clickar på en av bokstavsknapparna och gör så att den text som står på varje knapp är det som registreras när jag trycker på den altså trycker jag på [S] registreras det ett s.
        {
            Button Bokstav = sender as Button;
            Bokstav.Enabled = false;
            
            char GömdBokstav = Bokstav.Text.ToLower().ToCharArray()[0];

            if (RndOrd.Contains(GömdBokstav)) //Denna kontrollerar iall bokstaven jag klickat på är den samma som den bokstav soim är "under" _ tecket så byter den ut _ tecknet till den bokstav jag matade in.
            {
                char[] Bokstaver = RndOrd.ToCharArray();
                for (int i = 0; i < Bokstaver.Length; i++)
                {
                    if (Bokstaver[i] == GömdBokstav)
                        HmlRndOrd[i].Text = GömdBokstav.ToString();

                }

            }
            else //Här är ifall den inte matchade och då registreras det som ett fel.
            {
                Fel++;
                MessageBox.Show("FEL!, Gissa bättre.");
                FelBokstaver.Text += " " + GömdBokstav.ToString() + ",";
                FelGissning--;
                LivKvar();
            }
            if (FelGissning >= 1)
            {
                Bild.Image = Bilder[Fel];
            }
            else
            {
                Bild.Image = Bilder[Fel];
                Förlorat();
            }
            foreach (Label l in HmlRndOrd)
                if (l.Text == "_") return;
            Vunnit();



            //MessageBox.Show(OrdLista()); // Kontroll ifall Ordlista fungerar
        }
        void Vunnit() //Här är vad sopm händer ifall min kod inte längre kan hitta ett _ tecken och då görs alla knappar otillgängliga och jag får upp ett vinst meddelande.
        {
            MessageBox.Show("Woo, du vann!");
            SpelaOm.Text = "Nice du satte det!  Försök Igen! -->";
            foreach (Control Bokstav in this.Controls)
            {
                if (Bokstav is Button)
                {
                    Bokstav.Enabled = false;
                }
            }
        }
        void Förlorat() // Denna utlöses ifall felgissningar blivit fler än 10 och då blir alla knappar igen otillgängliga och du får upp ett meddelande om att du har förlorat
        {
            FörsökIgen.Text = "Ordet var: " + RndOrd + "Försök Igen! -->";
            foreach (Control Bokstav in this.Controls)
            {
                if (Bokstav is Button)
                {
                    Bokstav.Enabled = false;
                }
            }
        }

        private void StartaOm_Click(object sender, EventArgs e) //Den här knappen bara nollställer allting till deras värde från början och väljer ett nytt random ord.
        {
            HangmanLexicon();
            SpelaOm.Text = "";
            FörsökIgen.Text = "";
            FelBokstaver.Text = "";
            Fel = 0;
            FelGissning = 8;
            LivKvar();
            Bild.Image = Bilder[Fel];
            this.button1.Enabled = true;
            foreach (Control Bokstav in this.Controls)
            {
                if (Bokstav is Button)
                {
                    Bokstav.Enabled = true;
                }
            }
        }

        private void HangaGubbe_Load(object sender, EventArgs e)
        {

        }
    }
}


