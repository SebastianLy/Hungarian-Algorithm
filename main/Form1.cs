using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace main
{
    
    public partial class Form1 : Form
    {

        public Form1()
        {
            
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                //Restart flag
                Flagi.Krok1_zrobiony = false;
                Flagi.Krok2_zrobiony = false;
                Flagi.Krok3_zrobiony= false;
                Flagi.Krok4_zrobiony = false;
                Flagi.Krok3_Krok4_petla_skonczona = false;
                if (dataGridView1 != null)
                {
                    button3.Visible = true;
                    button4.Visible = true;
                }
                int[,] macierzLiczb = OdczytajPlikTekstowy(openFileDialog1.FileName);
                GlobalneZmienne.zapisanaMacierz = macierzLiczb;
                int wymiar = macierzLiczb.GetLength(0);
                int[,] macierzLinii = new int[wymiar, wymiar];
                SetupDataGridView(wymiar);
                //Wyswietl macierz liczb na gridboxie
                Wyswietl(macierzLiczb);               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Flagi.Krok1_zrobiony = false;
            Flagi.Krok2_zrobiony = false;
            Flagi.Krok3_zrobiony = false;
            Flagi.Krok4_zrobiony = false;
            Flagi.Krok3_Krok4_petla_skonczona = false;
           
            int[,] macierzLiczb = GenerujTablice();
            int wymiar = macierzLiczb.GetLength(0);
            GlobalneZmienne.zapisanaMacierz = macierzLiczb;
            //int[,] macierzLinii = new int[wymiar, wymiar];
            SetupDataGridView(wymiar);
            //Wyswietl macierz liczb na gridboxie
            Wyswietl(macierzLiczb);
            if (dataGridView1 != null)
            {
                button3.Visible = true;
                button4.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[,] macierzLiczb = Czytaj();
            int wymiar = macierzLiczb.GetLength(0);
            int[][,] macierz3=new int[2][,];
            int[,] macierzLinii = new int[wymiar, wymiar];
            int[,] macierzPrzypisanych = new int[wymiar, wymiar];
            int[,] macierzPrzeciec = new int[wymiar, wymiar];

            if (!Flagi.Krok1_zrobiony)
            {
                OdejmijMinWiersze(macierzLiczb);
                Wyswietl(macierzLiczb);
                RysujCzerwoneZera(macierzLiczb);
                Flagi.Krok1_zrobiony = true;
            }

            else if (!Flagi.Krok2_zrobiony)
            {
                OdejmijMinKolumny(macierzLiczb);
                Wyswietl(macierzLiczb);
                RysujCzerwoneZera(macierzLiczb);
                Flagi.Krok2_zrobiony = true;
            }
            //dataGridView1.Update(); // update musi byc zeby sleep zadzialal
            //Thread.Sleep(2000);
            else if (!Flagi.Krok3_Krok4_petla_skonczona)
            {
                while (!Flagi.Krok3_Krok4_petla_skonczona)
                { 
                    if (!Flagi.Krok3_zrobiony)
                    {
                        macierz3 = Krok3(macierzLiczb);
                        macierzLinii = macierz3[0];
                        GlobalneZmienne.zapisanaMacierzPrzypisanych = macierz3[1];
                        Console.WriteLine();
                        test(GlobalneZmienne.zapisanaMacierzPrzypisanych);
                        macierzPrzeciec = macierz3[2];
                        RysujLinie(macierzLinii);
                        ZaznaczPunktyKoncowe(GlobalneZmienne.zapisanaMacierzPrzypisanych);
                        dataGridView1.Update();
                        Thread.Sleep(250);
                        if (wymiar == LiczbaJedynek(GlobalneZmienne.zapisanaMacierzPrzypisanych))
                        {
                            Flagi.Krok3_Krok4_petla_skonczona = true;
                            Flagi.Krok4_zrobiony = true;
                            Flagi.Krok3_zrobiony = true;
                        }
                    }
                    if (!Flagi.Krok4_zrobiony)
                    {
                        macierzLiczb = Krok4(macierzLiczb, macierzLinii, macierzPrzeciec);
                        Wyswietl(macierzLiczb);
                    }
                }
            }
            else
            {
                Wyswietl(GlobalneZmienne.zapisanaMacierz);
                ZaznaczPunktyKoncowe(GlobalneZmienne.zapisanaMacierzPrzypisanych);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int[,] macierzLiczb = Czytaj();
            int wymiar = macierzLiczb.GetLength(0);
            int[][,] macierz3 = new int[2][,];
            int[,] macierzLinii = new int[wymiar, wymiar];
            int[,] macierzPrzypisanych = new int[wymiar, wymiar];
            int[,] macierzPrzeciec = new int[wymiar, wymiar];
            bool krok3i4 = false;
            bool krok3 = false;
            bool krok4 = false;

            OdejmijMinWiersze(macierzLiczb);
            OdejmijMinKolumny(macierzLiczb);

            while (!krok3i4)
            {
                if (!krok3)
                {
                    macierz3 = Krok3(macierzLiczb);
                    macierzLinii = macierz3[0];
                    GlobalneZmienne.zapisanaMacierzPrzypisanych = macierz3[1];
                    macierzPrzeciec = macierz3[2];
                    if (wymiar == LiczbaJedynek(GlobalneZmienne.zapisanaMacierzPrzypisanych))
                    {
                        krok3i4 = true;
                        krok4 = true;
                        krok3 = true;
                    }
                }
                if (!krok4)
                {
                    macierzLiczb = Krok4(macierzLiczb, macierzLinii, macierzPrzeciec);
                }
            }
             Wyswietl(GlobalneZmienne.zapisanaMacierz);
             ZaznaczPunktyKoncowe(GlobalneZmienne.zapisanaMacierzPrzypisanych);
        }

        int[,] OdczytajPlikTekstowy(string file)
        {
            StreamReader sr = new StreamReader(file);

            int numberOfLines = 0;
            while (!sr.EndOfStream)
            {
                sr.ReadLine();
                numberOfLines++;
            }
            sr.Close();

            int[,] macierzLiczb = new int[numberOfLines, numberOfLines];
            sr = new StreamReader(file);
            try
            {
                int i = 0;
                while (!sr.EndOfStream)
                {
                    string[] row = sr.ReadLine().Split(' ');
                    for (int j = 0; j < row.Length; j++)
                    {
                        macierzLiczb[i, j] = Convert.ToInt32(row[j]);
                    }
                    i++;
                }
            }
            catch (Exception)
            {
                button3.Visible = false;
                button4.Visible = false;
                MessageBox.Show("Liczba wierszy nie jest równa liczbie kolumn!");               
                return new int[0, 0];
            }
            sr.Close();
            return macierzLiczb;
        }

        void Wyswietl(int[,] macierz)
        {
            dataGridView1.Rows.Clear();
            int wymiar = macierz.GetLength(0);
            // Odczytaj linia po linii numery z pliku i wyświetl 
            for (int i = 0; i < macierz.GetLength(0); i++)
            {
                string[] row = new string[wymiar];
                for (int j = 0; j < wymiar; j++)
                {
                    row[j] = Convert.ToString(macierz[i, j]);
                }
                dataGridView1.Rows.Add(row);
            }
            //oznaczanie komórek niepotrzebne
            dataGridView1.ClearSelection();
        }

        void SetupDataGridView(int wymiar)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = wymiar;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].Width = 30;
            }
        }

        public int[,] GenerujTablice()
        {
            Random rd = new Random();
            Random rd2 = new Random();
            int n;
            do
            {
                n = rd2.Next(40);
            }
            while (n == 0);
            int[,] tablica = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    tablica[i, j] = rd.Next(1000);
                }
            }
            return tablica;
        }

        public void RysujCzerwoneZera(int[,] macierzLiczb)
        {
            int wymiar = macierzLiczb.GetLength(0);
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    if (macierzLiczb[i, j] == 0)
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                }
            }
        }

        public void RysujLinie(int[,] macierzLinii)
        {
            int wymiar = macierzLinii.GetLength(0);
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    if (macierzLinii[i, j] == 1)
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.MediumTurquoise;
                }
            }
        }

        public void ZaznaczPunktyKoncowe(int[,] macierzLiczb)
        {
            int wymiar = macierzLiczb.GetLength(0);
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    if (macierzLiczb[i, j] == 1)
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.ForestGreen;
                }
            }
        }

        private void pomocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Readme.pdf");
        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Info.pdf");
        }

        private void wyjdźToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private int[,] Czytaj()
        {
            int[,] macierzLiczb = new int[dataGridView1.RowCount, dataGridView1.ColumnCount];

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    macierzLiczb[i, j] = Convert.ToInt32(dataGridView1[j, i].Value);
                }
            }
            return macierzLiczb;
        }
        //Ze wzgledu na problemy z dostepnoscia do funkcji Formsa przenosze kod algorytmu tutaj

        public static void OdejmijMinWiersze(int[,] macierzLiczb)
        {
            int wymiar = macierzLiczb.GetLength(0);
            for (int i = 0; i < wymiar; i++)
            {
                //znajduje najmniejsza liczbe z wiersza
                int min = macierzLiczb[i, 0];
                for (int j = 0; j < wymiar; j++)
                {
                    if (min > macierzLiczb[i, j])
                    {
                        min = macierzLiczb[i, j];
                    }
                }
                //odejmuje najmniejsza liczbe od wszystkich liczb w wierszu
                for (int j = 0; j < wymiar; j++)
                {
                    macierzLiczb[i, j] = macierzLiczb[i, j] - min;
                }
            }
        }
        public static void OdejmijMinKolumny(int[,] macierzLiczb)
        {
            int wymiar = macierzLiczb.GetLength(0);
            for (int i = 0; i < wymiar; i++)
            {
                //znajduje najmniejsza liczbe z kolumny
                int min = macierzLiczb[0, i];
                for (int j = 0; j < wymiar; j++)
                {
                    if (min > macierzLiczb[j, i])
                    {
                        min = macierzLiczb[j, i];
                    }
                }
                //odejmuje najmniejsza liczbe od wszystkich liczb w kolumnie
                for (int j = 0; j < wymiar; j++)
                {
                    macierzLiczb[j, i] = macierzLiczb[j, i] - min;
                }
            }
        }


        public static int LiczbaJedynek(int[,] tab)
        {
            int wymiar = tab.GetLength(0);
            int liczba = 0;
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    if (tab[i, j] == 1)
                        liczba++;
                }
            }
            return liczba;
        }

        public static bool[,] StworzMacierzBool(int[,] macierzLiczb)
        {
            int wymiar = macierzLiczb.GetLength(0);
            bool[,] macierzBool = new bool[wymiar, wymiar];
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    if (macierzLiczb[i, j] == 0)
                        macierzBool[i, j] = true;
                    else
                        macierzBool[i, j] = false;
                }
            }
            return macierzBool;
        }

        // A DFS based recursive function  
        // that returns true if a matching  
        // for vertex u is possible 
        static bool bpm(bool[,] bpGraph, int u,
                 bool[] seen, int[] matchR, int wymiar)
        {
            // Try every job one by one 
            for (int v = 0; v < wymiar; v++)
            {
                // If applicant u is interested  
                // in job v and v is not visited 
                if (bpGraph[u, v] && !seen[v])
                {
                    // Mark v as visited 
                    seen[v] = true;

                    // If job 'v' is not assigned to 
                    // an applicant OR previously assigned  
                    // applicant for job v (which is matchR[v]) 
                    // has an alternate job available. 
                    // Since v is marked as visited in the above  
                    // line, matchR[v] in the following recursive  
                    // call will not get job 'v' again 
                    if (matchR[v] < 0 || bpm(bpGraph, matchR[v],
                                             seen, matchR, wymiar))
                    {

                        matchR[v] = u;

                        return true;
                    }
                }
            }
            return false;
        }

        // Returns maximum number of  
        // matching from M to N 
        static int maxBPM(bool[,] bpGraph, int[,] macierzPrzypisanych, int wymiar)
        {
            // An array to keep track of the  
            // applicants assigned to jobs.  
            // The value of matchR[i] is the  
            // applicant number assigned to job i,  
            // the value -1 indicates nobody is assigned. 
            int[] matchR = new int[wymiar];

            // Initially all jobs are available 
            for (int i = 0; i < wymiar; ++i)
                matchR[i] = -1;

            // Count of jobs assigned to applicants 
            int result = 0;
            for (int u = 0; u < wymiar; u++)
            {
                // Mark all jobs as not 
                // seen for next applicant. 
                bool[] seen = new bool[wymiar];
                for (int i = 0; i < wymiar; ++i)
                    seen[i] = false;

                // Find if the applicant  
                // 'u' can get a job 
                if (bpm(bpGraph, u, seen, matchR, wymiar))
                {
                    result++;
                }
            }
            for (int u = 0; u < wymiar; u++)
            {

                if (matchR[u] >= 0)
                    macierzPrzypisanych[matchR[u], u] = 1;
            }

            return result;
        }

        public static int [][,] Krok3(int[,] macierzLiczb)
        {
            int wymiar = macierzLiczb.GetLength(0);
            int[,] macierzPrzypisanych = new int[wymiar, wymiar];
            int[] kolumnyPrzypisane = new int[wymiar];
            int[] wierszePrzypisane = new int[wymiar];

            //First, assign as many tasks as possible
            bool[,] bpGraph = StworzMacierzBool(macierzLiczb);
            maxBPM(bpGraph, macierzPrzypisanych, wymiar);
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    if (macierzPrzypisanych[i, j] == 1)
                    {
                        kolumnyPrzypisane[j] = 1;
                        wierszePrzypisane[i] = 1;
                    }
                }
            }

            int[] wierszeOznaczone = new int[wymiar];
            int[] kolumnyOznaczone = new int[wymiar];
            //Mark all rows having no assignments
            for (int i = 0; i < wymiar; i++)
            {
                if (wierszePrzypisane[i] == 0)
                {
                    wierszeOznaczone[i] = 1;
                }
            }

            //Mark all (unmarked) columns having zeros in newly marked row(s)
            int[,] macierzLinii = new int[wymiar, wymiar];
            int[] rysujKolumne = new int[wymiar];
            int[] rysujWiersz = new int[wymiar];
            do
            {
                for (int i = 0; i < wymiar; i++)
                {
                    if (wierszeOznaczone[i] == 1)
                        for (int j = 0; j < wymiar; j++)
                        {
                            if (macierzLiczb[i, j] == 0)
                                kolumnyOznaczone[j] = 1;
                        }
                }
                //Mark all rows having assignments in newly marked columns 
                for (int i = 0; i < wymiar; i++)
                {
                    for (int j = 0; j < wymiar; j++)
                    {
                        if (kolumnyOznaczone[j] == 1)
                            if (macierzPrzypisanych[i, j] == 1)
                                wierszeOznaczone[i] = 1;
                    }
                }
                //Now draw lines through all marked columns and unmarked rows
                rysujKolumne = kolumnyOznaczone;
                
                for (int i = 0; i < wymiar; i++)
                {
                    if (wierszeOznaczone[i] == 0)
                        rysujWiersz[i] = 1;
                }

                for (int i = 0; i < wymiar; i++)
                {
                    for (int j = 0; j < wymiar; j++)
                    {
                        if (rysujWiersz[i] == 1 || rysujKolumne[j] == 1)
                            macierzLinii[i, j] = 1;
                    }
                }
            } while (MaNiezaznaczoneZera(macierzLinii, macierzLiczb));
            
            int[,] macierzPrzeciec = new int[wymiar, wymiar];

            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    if (rysujWiersz[i]==1 &&rysujKolumne[j]==1)
                    {
                        macierzPrzeciec[i, j] = 1;
                    }
                }
            }
            
            int[][,] macierz3 = new int[3][,];
            macierz3[0] = macierzLinii;
            macierz3[1] = macierzPrzypisanych;
            macierz3[2] = macierzPrzeciec;

            return macierz3;
        }

        public static int[,] Krok4(int[,] macierzLiczb, int[,] macierzLinii, int[,] macierzPrzeciec)
        {
            //znajdz pozycje przeciecia linii 1- przeciecie, 0- brak
            int wymiar = macierzLiczb.GetLength(0);
            int min = 0;

            //znajdz najmniejsza liczbe w macierzy
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    //poczatkowe przypisanie min
                    if (min==0 && macierzLinii[i,j]==0)
                    {
                        min = macierzLiczb[i, j];
                    }
                    if (macierzLiczb[i,j]<min && macierzLinii[i,j]==0)
                    {
                        min = macierzLiczb[i, j];
                    }
                }
            }

            //dodaj min do przeciec, odejmij od reszty niezaznaczonych
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    if (macierzLinii[i, j] == 0)
                    {
                        macierzLiczb[i, j] -= min;
                    }
                    if (macierzPrzeciec[i,j]==1)
                    {
                        macierzLiczb[i, j] += min;
                    }
                }
            }
            return macierzLiczb;
        }

        public static void test(int[,] macierzLiczb)
        {
            int wymiar = macierzLiczb.GetLength(0);
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    Console.Write(macierzLiczb[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        public static bool MaNiezaznaczoneZera(int[,] macierzLinii, int[,] macierzLiczb)
        {
            int wymiar = macierzLiczb.GetLength(0);
            for (int i = 0; i < wymiar; i++)
            {
                for (int j = 0; j < wymiar; j++)
                {
                    if (macierzLiczb[i, j] == 0 && macierzLinii[i, j] != 1)
                        return true;
                }
            }
            return false;
        }

        public int LiczbaLinii(int[,] macierzLinii)
        {
            int wymiar = macierzLinii.GetLength(0);
            int liczbaLinii=0;

            for (int i = 0; i < wymiar; i++)
            {
                int dlugosc = 0;
                for (int j = 0; j < wymiar; j++)
                {
                    if(macierzLinii[i,j]==1)
                    {
                        dlugosc++;
                        if (dlugosc == wymiar && liczbaLinii!=wymiar) 
                        {
                            liczbaLinii++;
                        }
                    }
                }
            }

            for (int i = 0; i < wymiar; i++)
            {
                int dlugosc = 0;
                for (int j = 0; j < wymiar; j++)
                {
                    if (macierzLinii[j, i] == 1)
                    {
                        dlugosc++;
                        if (dlugosc == wymiar && liczbaLinii != wymiar)
                        {
                            liczbaLinii++;
                        }
                    }
                }
            }
            return liczbaLinii;
        }

        private void wyczyśćToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupDataGridView(0);
            button3.Visible = false;
            button4.Visible = false;
        }
    }
    //globalne flagi potrzebne w przyciku krokowym
    public class Flagi
    {
        public static bool Krok1_zrobiony = false;
        public static bool Krok2_zrobiony = false;
        public static bool Krok3_zrobiony = false;
        public static bool Krok4_zrobiony = false;
        public static bool Krok3_Krok4_petla_skonczona = false;

    }
    public class GlobalneZmienne
    {
        public static int[,] zapisanaMacierz;
        public static int[,] zapisanaMacierzPrzypisanych;
    }
}

