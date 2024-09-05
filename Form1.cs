using Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Bankomat
{
    public partial class Form1 : Form
    {
        Bitmap bitmap; //Bitmapa, wykorzystywana przy podświetlaniu kontrolek
        string jezyk = "PL"; //Wykrywanie obecnie używanego języka
        string kwotaBLIK = ""; //Zapamiętanie kwoty wpisanej przez użytkownika 
        string label = ""; //Przechowywanie wygenerowanego kodu BLIK
        string waluta = ""; //Wykrywanie w jakiej walucie wypłacono pieniądze
        bool cardInsert = false; //Wykrywanie czy, karta została włożona
        bool zalogowanoPomyslnie = false;
        bool wpłata = false; //Wykrywanie wpłaty
        int clickCounter = 0; //Liczenie kliknięć przycisku, przydatne przy nawigacji, gdy trzeba kliknąć ten sam przycisk aby przejść dalej
        int countEmptyLabel = 5; //Zmienna kontrolująca ile zostało pustych miejsc do przechowywania historii transakcji

        //Sztywne kursy, z ok. 12.2023r.
        double kursCZK = 0.18;
        double kursEUR = 4.33;
        double kursUSD = 3.96;

        List<TextBox> controlList = new List<TextBox>() { }; //lista przechowująca kontrolki do wprowadzania danych początkowych
        List<Label> controlList2 = new List<Label>() { }; //lista przechowująca kontrolki wykorzystywanych w historii transakcji

        private Rectangle button1OriginalRectangle;
        private Rectangle button2OriginalRectangle;
        private Rectangle button3OriginalRectangle;
        private Rectangle button4OriginalRectangle;
        private Rectangle button5OriginalRectangle;
        private Rectangle button6OriginalRectangle;
        private Rectangle button7OriginalRectangle;
        private Rectangle button8OriginalRectangle;
        private Rectangle button9OriginalRectangle;
        private Rectangle button10OriginalRectangle;
        private Rectangle button11OriginalRectangle;
        private Rectangle button12OriginalRectangle;
        private Rectangle button13OriginalRectangle;
        private Rectangle button14OriginalRectangle;
        private Rectangle button15OriginalRectangle;
        private Rectangle button16OriginalRectangle;
        private Rectangle button17OriginalRectangle;
        private Rectangle button18OriginalRectangle;
        private Rectangle button19OriginalRectangle;
        private Rectangle button20OriginalRectangle;
        private Rectangle button21OriginalRectangle;
        private Rectangle button22OriginalRectangle;
        private Rectangle button23OriginalRectangle;

        private Rectangle pictureBox1OriginalRectangle;
        private Rectangle pictureBox2OriginalRectangle;

        private Rectangle textBox1OriginalRectangle;

        private Rectangle label1OriginalRectangle;
        private Rectangle label8OriginalRectangle;
        private Rectangle label9OriginalRectangle;
        private Rectangle label10OriginalRectangle;
        private Rectangle label11OriginalRectangle;
        private Rectangle label12OriginalRectangle;


        private Rectangle originalFormSize;

        private Konto konto;
        private Karta karta;

        public Form1()
        {
            InitializeComponent();

            foreach (Control t in Controls) //dodawanie kontrolek używanych do wprowadzania danych początkowych
            {
                TextBox textbox = t as TextBox;

                if (textbox != null)
                {
                    if (textbox.Name != "textBox1")
                        controlList.Add(textbox);
                }
            }

            foreach (Control l in Controls) //dodawanie kontrolek służących do wyświetlania historii transakcji
            {
                Label label = l as Label;

                if (label != null)
                {
                    if (label.Name == "label8" || label.Name == "label9" || label.Name == "label10" || label.Name == "label11" || label.Name == "label12")
                        controlList2.Add(label);
                }
            }

            foreach (Control c in Controls) //Dodawanie kontrolek do podświetlania przycisków
            {
                Button b = c as Button;
                if (b != null)
                {
                    if (b.Name != "button23" && b.Name != "button24")
                    {
                        b.MouseLeave += AllButtons_MouseLeave;
                        b.MouseUp += Allbuttons_MouseUp;
                        b.MouseEnter += AllButtons_MouseEnter;
                        b.MouseDown += Allbuttons_MouseDown;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            originalFormSize = new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);
            button1OriginalRectangle = new Rectangle(button1.Location.X, button1.Location.Y, button1.Width, button1.Height);
            button2OriginalRectangle = new Rectangle(button2.Location.X, button2.Location.Y, button2.Width, button2.Height);
            button3OriginalRectangle = new Rectangle(button3.Location.X, button3.Location.Y, button3.Width, button3.Height);
            button4OriginalRectangle = new Rectangle(button4.Location.X, button4.Location.Y, button4.Width, button4.Height);
            button5OriginalRectangle = new Rectangle(button5.Location.X, button5.Location.Y, button5.Width, button5.Height);
            button6OriginalRectangle = new Rectangle(button6.Location.X, button6.Location.Y, button6.Width, button6.Height);
            button7OriginalRectangle = new Rectangle(button7.Location.X, button7.Location.Y, button7.Width, button7.Height);
            button8OriginalRectangle = new Rectangle(button8.Location.X, button8.Location.Y, button8.Width, button8.Height);
            button9OriginalRectangle = new Rectangle(button9.Location.X, button9.Location.Y, button9.Width, button9.Height);
            button10OriginalRectangle = new Rectangle(button10.Location.X, button10.Location.Y, button10.Width, button10.Height);
            button11OriginalRectangle = new Rectangle(button11.Location.X, button11.Location.Y, button11.Width, button11.Height);
            button12OriginalRectangle = new Rectangle(button12.Location.X, button12.Location.Y, button12.Width, button12.Height);
            button13OriginalRectangle = new Rectangle(button13.Location.X, button13.Location.Y, button13.Width, button13.Height);
            button14OriginalRectangle = new Rectangle(button14.Location.X, button14.Location.Y, button14.Width, button14.Height);
            button15OriginalRectangle = new Rectangle(button15.Location.X, button15.Location.Y, button15.Width, button15.Height);
            button16OriginalRectangle = new Rectangle(button16.Location.X, button16.Location.Y, button16.Width, button16.Height);
            button17OriginalRectangle = new Rectangle(button17.Location.X, button17.Location.Y, button17.Width, button17.Height);
            button18OriginalRectangle = new Rectangle(button18.Location.X, button18.Location.Y, button18.Width, button18.Height);
            button19OriginalRectangle = new Rectangle(button19.Location.X, button19.Location.Y, button19.Width, button19.Height);
            button20OriginalRectangle = new Rectangle(button20.Location.X, button20.Location.Y, button20.Width, button20.Height);
            button21OriginalRectangle = new Rectangle(button21.Location.X, button21.Location.Y, button21.Width, button21.Height);
            button22OriginalRectangle = new Rectangle(button22.Location.X, button22.Location.Y, button22.Width, button22.Height);
            button23OriginalRectangle = new Rectangle(button23.Location.X, button23.Location.Y, button23.Width, button23.Height);

            pictureBox1OriginalRectangle = new Rectangle(pictureBox1.Location.X, pictureBox1.Location.Y, pictureBox1.Width, pictureBox1.Height);
            pictureBox2OriginalRectangle = new Rectangle(pictureBox2.Location.X, pictureBox2.Location.Y, pictureBox2.Width, pictureBox2.Height);

            textBox1OriginalRectangle = new Rectangle(textBox1.Location.X, textBox1.Location.Y, textBox1.Width, textBox1.Height);

            label1OriginalRectangle = new Rectangle(label1.Location.X, label1.Location.Y, label1.Width, label1.Height);
            label8OriginalRectangle = new Rectangle(label8.Location.X, label8.Location.Y, label8.Width, label8.Height);
            label9OriginalRectangle = new Rectangle(label9.Location.X, label9.Location.Y, label9.Width, label9.Height);
            label10OriginalRectangle = new Rectangle(label10.Location.X, label10.Location.Y, label10.Width, label10.Height);
            label11OriginalRectangle = new Rectangle(label11.Location.X, label11.Location.Y, label11.Width, label11.Height);
            label12OriginalRectangle = new Rectangle(label12.Location.X, label12.Location.Y, label12.Width, label12.Height);
        }

        private void resizeControl(Rectangle r, Control c) //zmienianie rozmiaru kontrolek wraz ze zmianą rozmiaru okna
        {
            float xRatio = (float)(this.Width) / (float)(originalFormSize.Width);
            float yRatio = (float)(this.Height) / (float)(originalFormSize.Height);

            int newX = (int)(r.Location.X * xRatio);
            int newY = (int)(r.Location.Y * yRatio);

            int newWidth = (int)(r.Width * xRatio);
            int newHeight = (int)(r.Height * yRatio);

            c.Location = new Point(newX, newY);
            c.Size = new Size(newWidth, newHeight);
        }

        private Bitmap AdjustBrightness(Image image, float brightness) //Podświetlanie przycisków
        {
            float b = brightness;
            ColorMatrix cm = new ColorMatrix(new float[][]
                {
                    new float[] {b, 0, 0, 0, 0},
                    new float[] {0, b, 0, 0, 0},
                    new float[] {0, 0, b, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(cm);

            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect, GraphicsUnit.Pixel, attributes);
            }

            return bm;
        }

        private void langChange(string screenName) //zmienianie języka w zależności od stanu zmiennej język
        {
            if (jezyk == "PL")
                pictureBox1.Image = Image.FromFile($"./gfx/ekrany/{screenName}.png");
            else if (jezyk == "ENG")
                pictureBox1.Image = Image.FromFile($"./gfx/ekrany/{screenName}_ENG.png");
            else
                pictureBox1.Image = Image.FromFile($"./gfx/ekrany/{screenName}_GER.png");
        }

        private void balanceChange(decimal PLN, decimal CZK, decimal EUR, decimal USD) //zmiana stanu konta i przewalutowanie
        {
            if (pictureBox1.Tag.ToString() == "ekran_wpłata" || pictureBox1.Tag.ToString().IndexOf("PLN") != -1)
            {
                waluta = "PLN";

                if (wpłata == false)
                {
                    if (konto.Balance >= PLN)
                    {
                        konto.Withdraw(PLN);
                        historiaTransakcji(wpłata, PLN, waluta);
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/potwierdzenie_wykonania_operacji.png");
                        langChange("potwierdzenie_wykonania_operacji");
                        pictureBox1.Tag = "potwierdzenie_wykonania_operacji";
                    }
                    else
                    {
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/brak_środków.png");
                        langChange("brak_środków");
                        pictureBox1.Tag = "brak_środków";
                    }
                }
                else
                {
                    konto.Deposit(PLN);
                    historiaTransakcji(wpłata, PLN, waluta);
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/potwierdzenie_wykonania_operacji.png");
                    langChange("potwierdzenie_wykonania_operacji");
                    pictureBox1.Tag = "potwierdzenie_wykonania_operacji";
                }
            }
            else if (pictureBox1.Tag.ToString().IndexOf("USD") != -1)
            {
                waluta = "USD";

                if (konto.Balance >= USD * (decimal)kursUSD)
                {
                    konto.Withdraw(USD * (decimal)kursUSD);
                    historiaTransakcji(wpłata, USD, waluta);
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/potwierdzenie_wykonania_operacji.png");
                    langChange("potwierdzenie_wykonania_operacji");
                    pictureBox1.Tag = "potwierdzenie_wykonania_operacji";
                }
                else
                {
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/brak_środków.png");
                    langChange("brak_środków");
                    pictureBox1.Tag = "brak_środków";
                }
            }
            else if (pictureBox1.Tag.ToString().IndexOf("EUR") != -1)
            {
                waluta = "EUR";

                if (konto.Balance >= EUR * (decimal)kursEUR)
                {
                    konto.Withdraw(EUR * (decimal)kursEUR);
                    historiaTransakcji(wpłata, EUR, waluta);
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/potwierdzenie_wykonania_operacji.png");
                    langChange("potwierdzenie_wykonania_operacji");
                    pictureBox1.Tag = "potwierdzenie_wykonania_operacji";
                }
                else
                {
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/brak_środków.png");
                    langChange("brak_środków");
                    pictureBox1.Tag = "brak_środków";
                }
            }
            else
            {
                waluta = "CZK";

                if (konto.Balance >= CZK * (decimal)kursCZK)
                {
                    konto.Withdraw(CZK * (decimal)kursCZK);
                    historiaTransakcji(wpłata, CZK, waluta);
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/potwierdzenie_wykonania_operacji.png");
                    langChange("potwierdzenie_wykonania_operacji");
                    pictureBox1.Tag = "potwierdzenie_wykonania_operacji";
                }
                else
                {
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/brak_środków.png");
                    langChange("brak_środków");
                    pictureBox1.Tag = "brak_środków";
                }
            }
        }

        private string generatorBLIK() //generator kodów BLIK
        {
            var random = new Random();
            var rng = random.Next(100000, 999999);

            string kod = rng.ToString();

            return kod;
        }

        private void historiaTransakcji(bool czy_wpłata, decimal kwota, string waluta)
        {
            if (countEmptyLabel == 0)
            {
                foreach (Label control in controlList2)
                {
                    control.Text = string.Empty;
                }
            }

            foreach (Label control in controlList2)
            {
                if (control.Text == string.Empty)
                {
                    if (czy_wpłata == true)
                    {
                        if (jezyk == "PL")
                            control.Text = $"Wpłata {kwota} PLN";
                        if (jezyk == "GER")
                            control.Text = $"Einzahlung {kwota} PLN";
                        if (jezyk == "ENG")
                            control.Text = $"Deposit {kwota} PLN";
                    }
                    else
                    {
                        if (jezyk == "PL")
                            control.Text = $"Wypłata {kwota} {waluta}";
                        if (jezyk == "GER")
                            control.Text = $"Abhebung {kwota} {waluta}";
                        if (jezyk == "ENG")
                            control.Text = $"Withdraw {kwota} {waluta}";
                    }
                    countEmptyLabel--;
                    break;
                }
                else
                    continue;
            }
        }

        private void Form1_Resize(object sender, EventArgs e) //przeskalowywanie kontrolek
        {
            resizeControl(button1OriginalRectangle, button1);
            resizeControl(button2OriginalRectangle, button2);
            resizeControl(button3OriginalRectangle, button3);
            resizeControl(button4OriginalRectangle, button4);
            resizeControl(button5OriginalRectangle, button5);
            resizeControl(button6OriginalRectangle, button6);
            resizeControl(button7OriginalRectangle, button7);
            resizeControl(button8OriginalRectangle, button8);
            resizeControl(button9OriginalRectangle, button9);
            resizeControl(button10OriginalRectangle, button10);
            resizeControl(button11OriginalRectangle, button11);
            resizeControl(button12OriginalRectangle, button12);
            resizeControl(button13OriginalRectangle, button13);
            resizeControl(button14OriginalRectangle, button14);
            resizeControl(button15OriginalRectangle, button15);
            resizeControl(button16OriginalRectangle, button16);
            resizeControl(button17OriginalRectangle, button17);
            resizeControl(button18OriginalRectangle, button18);
            resizeControl(button19OriginalRectangle, button19);
            resizeControl(button20OriginalRectangle, button20);
            resizeControl(button21OriginalRectangle, button21);
            resizeControl(button22OriginalRectangle, button22);
            resizeControl(button23OriginalRectangle, button23);

            resizeControl(pictureBox1OriginalRectangle, pictureBox1);
            resizeControl(textBox1OriginalRectangle, textBox1);
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * *         
         * <button19>                           <button15> *
         *                                                 *
         *                                                 * 
         * <button20>                           <button16> *
         *                                                 *
         *                    ekran                        *
         * <button21>                           <button17> *
         *                                                 * 
         *                                                 *
         * <button22>                           <button18> *
         * * * * * * * * * * * * * * * * * * * * * * * * * */


        /*
         * button10 - Enter - zatwierdzenie wprowadzonych danych
         * button12 - Cancel - anulowanie i powrót do ekranu głównego, w przypadku braku włożonej karty, powrót do ekranu tytułowego
         * button11 - Clear - Czyszczenie textBoxa
         * button13 - 000 - Ułatwienie wpisywania dużych liczb
         */

        private void button23_Click(object sender, EventArgs e) //Czytnik karty
        {
            clickCounter = 0;

            if (cardInsert == false)
            {
                Form1.ActiveForm.BackgroundImage = Image.FromFile("./gfx/włożona_karta.jpg");
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/wpisz_PIN.png");
                langChange("wpisz_PIN");
                pictureBox1.Tag = "wpisz_PIN";
                textBox1.Visible = true;
                textBox1.Clear();
                textBox1.MaxLength = 3;
                pictureBox2.Hide();
                label1.Hide();
                cardInsert = true;
            }
            else
            {
                Form1.ActiveForm.BackgroundImage = Image.FromFile("./gfx/bez_karty.jpg");
                langChange("ekran_tytułowy");
                pictureBox1.Tag = "ekran_tytułowy";
                textBox1.Visible = false;
                textBox1.Clear();
                cardInsert = false;
                zalogowanoPomyslnie = false;
            }
        }
        private void button20_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Tag.ToString() == "ekran_główny")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_1.png");
                langChange("ekran_wypłata_1");
                pictureBox1.Tag = "ekran_wypłata_1";
            }

            if (pictureBox1.Tag.ToString().IndexOf("wypłata_2") != -1 || pictureBox1.Tag.ToString().IndexOf("wpłata") != -1)
            {
                balanceChange(100, 200, 60, 50);
                textBox1.Visible = false;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Tag.ToString() == "stan_konta")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/historia_trans.png");
                pictureBox1.Tag = "historia_trans";
                textBox1.Visible = false;
                label8.Visible = true; label9.Visible = true; label10.Visible = true; label11.Visible = true; label12.Visible = true;
            }

            if (pictureBox1.Tag.ToString().IndexOf("wypłata_2") != -1 || pictureBox1.Tag.ToString().IndexOf("wpłata") != -1)
            {
                balanceChange(50, 100, 20, 20);
                textBox1.Visible = false;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            clickCounter++;

            if (pictureBox1.Tag.ToString() == "ekran_główny")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wpłata.png");
                langChange("ekran_wpłata");
                pictureBox1.Tag = "ekran_wpłata";
                textBox1.Visible = false;
                wpłata = true;
            }

            if (pictureBox1.Tag.ToString() == "ekran_wypłata_1")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_2_PLN.png");
                pictureBox1.Tag = "ekran_wypłata_2_PLN";
                langChange("ekran_wypłata_2_PLN");
                textBox1.Visible = false;
                waluta = "PLN";
            }

            if ((pictureBox1.Tag.ToString().IndexOf("wypłata_2") != -1 || pictureBox1.Tag.ToString().IndexOf("wpłata") != -1) && clickCounter == 2)
            {
                balanceChange(200, 500, 100, 100);
                textBox1.Visible = false;
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString() == "wybór_języka")
            {
                jezyk = "PL";
                langChange("ekran_główny");
                pictureBox1.Tag = "ekran_główny";
                clickCounter = 0;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            clickCounter++;

            if (pictureBox1.Tag.ToString() == "ekran_główny")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/stan_konta.png");
                langChange("stan_konta");
                pictureBox1.Tag = "stan_konta";
                textBox1.Location = new Point(584, 430);
                textBox1.Text = konto.Balance.ToString() + " PLN";
                textBox1.Visible = true;
                textBox1.MaxLength = 10;
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString() == "ekran_wypłata_1")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_2_USD.png");
                langChange("ekran_wypłata_2_USD");
                pictureBox1.Tag = "ekran_wypłata_2_USD";
                textBox1.Visible = false;
                waluta = "USD";

            }

            if ((pictureBox1.Tag.ToString().IndexOf("wypłata_2") != -1 || pictureBox1.Tag.ToString().IndexOf("wpłata") != -1) && clickCounter == 2)
            {
                balanceChange(500, 1000, 200, 200);
                textBox1.Visible = false;
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString() == "wybór_języka")
            {
                jezyk = "ENG";
                langChange("ekran_główny");
                pictureBox1.Tag = "ekran_główny";
                clickCounter = 0;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Tag.ToString().IndexOf("wypłata_2") != -1 || pictureBox1.Tag.ToString().IndexOf("wpłata") != -1)
            {
                balanceChange(1000, 2000, 500, 500);
                textBox1.Visible = false;
                clickCounter = 0;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Tag.ToString().IndexOf("wypłata_2") != -1 || pictureBox1.Tag.ToString().IndexOf("wpłata") != -1)
            {
                balanceChange(1500, 3500, 1000, 1000);
                textBox1.Visible = false;
                clickCounter = 0;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            clickCounter++;

            if (pictureBox1.Tag.ToString() == "ekran_główny")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/wpisz_PIN.png");
                langChange("wpisz_PIN");
                pictureBox1.Tag = "wpisz_PIN";
                textBox1.Visible = true;
                textBox1.Location = new Point(584, 414);
                textBox1.MaxLength = 3;
                textBox1.Clear();
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString() == "ekran_wypłata_1")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_2_EUR.png");
                langChange("ekran_wypłata_2_EUR");
                pictureBox1.Tag = "ekran_wypłata_2_EUR";
                textBox1.Visible = false;
                waluta = "EUR";
            }

            if ((pictureBox1.Tag.ToString().IndexOf("wypłata_2") != -1 && clickCounter == 2) || pictureBox1.Tag.ToString().IndexOf("wpłata") != -1)
            {
                balanceChange(2000, 5000, 1500, 1500);
                textBox1.Visible = false;
                clickCounter = 0;

            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            clickCounter++;

            if (pictureBox1.Tag.ToString() == "ekran_główny")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/wybór_języka.png");
                pictureBox1.Tag = "wybór_języka";
                textBox1.Visible = false;
            }

            if (pictureBox1.Tag.ToString() == "ekran_tytułowy")
            {
                waluta = "PLN";
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_3_BLIK.png");
                langChange("ekran_wypłata_3_BLIK");
                pictureBox1.Tag = "ekran_wypłata_3_BLIK";
                textBox1.MaxLength = 8;
                textBox1.Visible = true;
            }

            if (pictureBox1.Tag.ToString().IndexOf("PLN") != -1 || pictureBox1.Tag.ToString().IndexOf("EUR") != -1 || pictureBox1.Tag.ToString().IndexOf("USD") != -1)
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_3_Inna_kwota.png");
                langChange("ekran_wypłata_3_Inna_kwota");
                pictureBox1.Tag = "ekran_wypłata_3_Inna_kwota";
                textBox1.Visible = true;
                textBox1.Location = new Point(584, 414);
                textBox1.MaxLength = 8;
                textBox1.Clear();
            }

            if (pictureBox1.Tag.ToString() == "ekran_wypłata_2_CZK" && clickCounter == 2)
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_3_Inna_kwota.png");
                langChange("ekran_wypłata_3_Inna_kwota");
                pictureBox1.Tag = "ekran_wypłata_3_Inna_kwota";
                textBox1.Visible = true;
                textBox1.Location = new Point(584, 414);
                textBox1.MaxLength = 8;
                textBox1.Clear();
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString() == "ekran_wypłata_1")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_2_CZK.png");
                langChange("ekran_wypłata_2_CZK");
                pictureBox1.Tag = "ekran_wypłata_2_CZK";
                textBox1.Visible = false;
                waluta = "CZK";
            }

            if (pictureBox1.Tag.ToString() == "ekran_wpłata")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wpłata_2_Inna_kwota.png");
                langChange("ekran_wpłata_2_Inna_kwota");
                pictureBox1.Tag = "ekran_wpłata_2_Inna_kwota";
                textBox1.Visible = true;
                textBox1.Location = new Point(584, 414);
                textBox1.MaxLength = 8;
                textBox1.Clear();
            }

            if (pictureBox1.Tag.ToString() == "potwierdzenie_zmiany_PINU")
            {
                langChange("ekran_główny");
                pictureBox1.Tag = "ekran_główny";
                textBox1.Visible = false;
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString() == "potwierdzenie_wykonania_operacji")
            {
                clickCounter = 0;

                if (cardInsert == true)
                {
                    langChange("ekran_główny");
                    pictureBox1.Tag = "ekran_główny";
                    textBox1.Visible = false;
                    wpłata = false;
                }
                else
                {
                    langChange("ekran_tytułowy");
                    pictureBox1.Tag = "ekran_tytułowy";
                    textBox1.Visible = false;
                }
            }

            if (pictureBox1.Tag.ToString() == "wybór_języka" && clickCounter == 2)
            {
                jezyk = "GER";
                langChange("ekran_główny");
                pictureBox1.Tag = "ekran_główny";
                wpłata = false;
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString() == "PIN_niepoprawny")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/wpisz_PIN.png");
                langChange("wpisz_PIN");
                pictureBox1.Tag = "wpisz_PIN";
                textBox1.Visible = true;
                textBox1.Clear();
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString().IndexOf("brak") != -1)
            {
                if (zalogowanoPomyslnie == true)
                {
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_główny.png");
                    langChange("ekran_główny");
                    pictureBox1.Tag = "ekran_główny";
                }
                else
                {
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_tytułowy.png");
                    langChange("ekran_tytułowy");
                    pictureBox1.Tag = "ekran_tytułowy";
                }
                clickCounter = 0;

            }

            if (pictureBox1.Tag.ToString() == "BLIK_niepoprawny")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_4_BLIK.png");
                langChange("ekran_wypłata_4_BLIK");
                pictureBox1.Tag = "ekran_wypłata_4_BLIK";
                pictureBox2.Visible = true;
                label1.Visible = true;
                textBox1.MaxLength = 5;
                textBox1.Visible = true;
                textBox1.Clear();
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString() == "historia_trans")
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/stan_konta.png");
                langChange("stan_konta");
                pictureBox1.Tag = "stan_konta";
                label8.Visible = false; label9.Visible = false; label10.Visible = false; label11.Visible = false; label12.Visible = false;
                textBox1.Visible = true;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            wpłata = false;
            pictureBox2.Hide();
            label1.Hide();

            if (pictureBox1.Tag.ToString() == "historia_trans")
            {
                label8.Visible = false; label9.Visible = false; label10.Visible = false; label11.Visible = false; label12.Visible = false;
            }

            if (cardInsert == true && zalogowanoPomyslnie == true)
            {
                langChange("ekran_główny");
                pictureBox1.Tag = "ekran_główny";
                textBox1.Visible = false;
                wpłata = false;
                clickCounter = 0;
            }
            else
            {
                langChange("ekran_tytułowy");
                pictureBox1.Tag = "ekran_tytułowy";
                textBox1.Visible = false;
                clickCounter = 0;
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            clickCounter++;

            if (pictureBox1.Tag.ToString() == "wpisz_PIN")
            {
                if (zalogowanoPomyslnie == true)
                {
                    if (textBox1.Text.ToString() == karta.pinNum.ToString())
                    {
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/wpisz_nowy_PIN.png");
                        langChange("wpisz_nowy_PIN");
                        pictureBox1.Tag = "wpisz_nowy_PIN";
                        textBox1.Visible = true;
                        textBox1.Location = new Point(584, 414);
                        textBox1.MaxLength = 3;
                        textBox1.Clear();
                    }
                    else
                    {
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/PIN_niepoprawny.png");
                        langChange("PIN_niepoprawny");
                        pictureBox1.Tag = "PIN_niepoprawny";
                        textBox1.Visible = false;
                    }
                }
                else
                {
                    if (textBox1.Text.ToString() == karta.pinNum.ToString())
                    {
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_główny.png");
                        langChange("ekran_główny");
                        pictureBox1.Tag = "ekran_główny";
                        textBox1.Visible = false;
                        zalogowanoPomyslnie = true;
                        wpłata = false;
                    }
                    else
                    {
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/PIN_niepoprawny.png");
                        langChange("PIN_niepoprawny");
                        pictureBox1.Tag = "PIN_niepoprawny";
                        textBox1.Visible = false;
                        zalogowanoPomyslnie = false;
                    }
                    clickCounter = 0;
                }
            }

            if (pictureBox1.Tag.ToString() == "wpisz_nowy_PIN" && clickCounter == 2)
            {
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/potwierdzenie_zmiany_PINU.png");
                langChange("potwierdzenie_zmiany_PINU");
                pictureBox1.Tag = "potwierdzenie_zmiany_PINU";
                karta.pinNum = textBox1.Text.ToString();
                textBox1.Visible = false;
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString().IndexOf("Inna") != -1)
            {
                decimal kurs;
                decimal kwota;
                kwota = Convert.ToDecimal(textBox1.Text);

                if (waluta == "PLN")
                {
                    kurs = 1;
                }
                else if (waluta == "EUR")
                {
                    kurs = (decimal)kursEUR;
                }
                else if (waluta == "USD")
                {
                    kurs = (decimal)kursUSD;
                }
                else
                {
                    kurs = (decimal)kursCZK;
                }

                if (wpłata == false)
                {
                    if (konto.Balance >= kwota * kurs)
                    {
                        konto.Withdraw(kwota * kurs);
                        historiaTransakcji(wpłata, kwota, waluta);
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/potwierdzenie_wykonania_operacji.png");
                        langChange("potwierdzenie_wykonania_operacji");
                        pictureBox1.Tag = "potwierdzenie_wykonania_operacji";
                    }
                    else
                    {
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/brak_środków.png");
                        langChange("brak_środków");
                        pictureBox1.Tag = "brak_środków";
                    }
                }
                else
                {
                    konto.Deposit(kwota);
                    historiaTransakcji(wpłata, kwota, waluta);
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/potwierdzenie_wykonania_operacji.png");
                    langChange("potwierdzenie_wykonania_operacji");
                    pictureBox1.Tag = "potwierdzenie_wykonania_operacji";
                }
                clickCounter = 0;
                textBox1.Visible = false;
                textBox1.Clear();

            }

            if (pictureBox1.Tag.ToString().IndexOf("4_BLIK") != -1)
            {
                if (label == textBox1.Text)
                {
                    decimal kwota;
                    kwota = Convert.ToDecimal(kwotaBLIK);
                    if (konto.Balance >= kwota)
                    {
                        konto.Withdraw(kwota);
                        historiaTransakcji(wpłata, kwota, waluta);
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/potwierdzenie_wykonania_operacji.png");
                        langChange("potwierdzenie_wykonania_operacji");
                        pictureBox1.Tag = "potwierdzenie_wykonania_operacji";
                        textBox1.Visible = false;
                        pictureBox2.Visible = false;
                        label1.Visible = false;
                    }
                    else
                    {
                        pictureBox1.Image = Image.FromFile("./gfx/ekrany/brak_środków.png");
                        langChange("brak_środków");
                        pictureBox1.Tag = "brak_środków";
                        textBox1.Visible = false;
                        pictureBox2.Visible = false;
                        label1.Visible = false;
                    }
                    textBox1.Clear();
                }
                else
                {
                    pictureBox1.Image = Image.FromFile("./gfx/ekrany/BLIK_niepoprawny.png");
                    langChange("BLIK_niepoprawny");
                    pictureBox1.Tag = "BLIK_niepoprawny";
                    textBox1.Visible = false;
                    pictureBox2.Visible = false;
                    label1.Visible = false;
                }
                clickCounter = 0;
            }

            if (pictureBox1.Tag.ToString().IndexOf("wypłata_3_BLIK") != -1)
            {
                string kod = generatorBLIK();
                label = kod;
                label1.Text = kod.Insert(3, " ");
                kwotaBLIK = textBox1.Text;
                pictureBox1.Image = Image.FromFile("./gfx/ekrany/ekran_wypłata_4_BLIK.png");
                langChange("ekran_wypłata_4_BLIK");
                pictureBox1.Tag = "ekran_wypłata_4_BLIK";
                pictureBox2.Visible = true;
                label1.Visible = true;
                textBox1.Clear();
                textBox1.MaxLength = 5;
                textBox1.Visible = true;
                clickCounter = 0;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("2");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("3");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("5");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("6");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("7");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("8");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("9");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("0");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Tag.ToString().IndexOf("PIN") > -1 && (textBox1.Text.Length + 3) <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
            {
                textBox1.AppendText("000");
            }
            else if (pictureBox1.Tag.ToString().IndexOf("PIN") == -1 && textBox1.Text.Length <= textBox1.MaxLength && pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.AppendText("000");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Tag.ToString() != "stan_konta")
                textBox1.Clear();
        }

        private void AllButtons_MouseLeave(object sender, EventArgs e)
        {
            Button bt = sender as Button;

            bt.BackgroundImage = bitmap;
        }

        private void Allbuttons_MouseUp(object sender, MouseEventArgs e)
        {
            Button bt = sender as Button;

            bt.BackgroundImage = bitmap;
            bt.BackgroundImage = AdjustBrightness(bitmap, (float)1.1);
        }

        private void Allbuttons_MouseDown(object sender, MouseEventArgs e)
        {
            Button bt = sender as Button;

            bt.BackgroundImage = AdjustBrightness(bitmap, (float)1.2);
        }

        private void AllButtons_MouseEnter(object sender, EventArgs e)
        {
            Button bt = sender as Button;

            bitmap = (Bitmap)bt.BackgroundImage;
            bt.BackgroundImage = AdjustBrightness(bitmap, (float)1.1);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            bool dane_wprowadzone = true;

            foreach (TextBox control in controlList)
            {
                if (control.Name != "textBox2" && control.Name != "textBox4")
                {
                    if (control.MaxLength > control.Text.Length)
                    {
                        dane_wprowadzone = false;
                        break;
                    }
                }
                else
                {
                    if (control.Text == string.Empty)
                    {
                        dane_wprowadzone = false;
                        break;
                    }
                }
            }

            if (dane_wprowadzone)
            {
                if (decimal.TryParse(textBox2.Text, out decimal stan_konta))
                {
                    konto = new Konto(textBox4.Text, stan_konta, textBox3.Text);
                    karta = new Karta(textBox6.Text, textBox5.Text);

                    pictureBox3.Hide();
                    textBox2.Hide();
                    textBox3.Hide();
                    textBox4.Hide();
                    textBox5.Hide();
                    textBox6.Hide();
                    label2.Hide();
                    label3.Hide();
                    label4.Hide();
                    label5.Hide();
                    label6.Hide();
                    label7.Hide();
                    button24.Hide();
                }
            }
            else
            {
                MessageBox.Show("Nieprawidłowe dane wejściowe");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if (textBox3.TextLength == 2 || textBox3.TextLength == 7 || textBox3.TextLength == 12 || textBox3.TextLength == 17 || textBox3.TextLength == 22 || textBox3.TextLength == 27)
                textBox3.AppendText(" ");
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if (textBox5.TextLength == 4 || textBox5.TextLength == 9 || textBox5.TextLength == 14)
                textBox5.AppendText(" ");
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
