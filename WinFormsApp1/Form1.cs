using System.Security.Cryptography.X509Certificates; // X.509 sertifikalar�yla �al��mak i�in gereken s�n�flar� i�erir.
using System.Windows.Forms; // Windows Forms uygulamalar� olu�turmak i�in gereken UI bile�enlerini sa�lar.
using static System.Windows.Forms.VisualStyles.VisualStyleElement; // G�rsel stilleri temsil eden ��eleri kullanarak �zelle�tirilmi� denetimler olu�turmay� sa�lar.
using System.IO; // Dosya ve klas�r i�lemleri i�in gerekli olan s�n�flar� i�erir.
using System.Text; // Metin i�lemleri, kodlama d�n���mleri ve di�er metin tabanl� i�lemler i�in gerekli olan s�n�flar� i�erir.
using System.Drawing.Printing; // Yaz�c�larla etkile�im kurmak ve belge yazd�rma i�lemleri yapmak i�in gerekli olan s�n�flar� i�erir.
using System.IO.Packaging; // Paketlenmi� dosyalarla �al��mak i�in gerekli olan s�n�flar� i�erir (�rne�in, Open XML dosyalar�).
using System.Drawing; // Grafiklerle �al��mak i�in gerekli olan s�n�flar� i�erir (�rne�in, resim olu�turma ve �izim i�lemleri).
using System.Drawing.Imaging; // Resim i�lemleri i�in gerekli olan s�n�flar� i�erir (�rne�in, resim dosyalar�n� farkl� bi�imlerde kaydetme).
using System.Drawing.Text; // TrueType ve OpenType fontlar�n� y�netmek ve kullanmak i�in gerekli olan s�n�flar� i�erir.
using System.ComponentModel; // Bile�en tabanl� programlama modelinde kullan�lan bile�en s�n�flar�n� i�erir.
using System.Linq; // LINQ (Language Integrated Query) sorgular� i�in geni�letilmi� y�ntemler ve di�er ara�lar� sa�lar.
using System.Threading.Tasks; // Asenkron ve paralel programlama i�in Task s�n�flar� ve di�er ara�lar� i�erir.
using Microsoft.CSharp; // C# dilinde kod derlemesi ve y�r�tmesi i�in gereken s�n�flar� i�erir.
using System.CodeDom.Compiler; // Kod derleme ve y�r�tme i�lemlerini ger�ekle�tirmek i�in gerekli olan s�n�flar� i�erir.
using System.CodeDom; // CodeDOM (Code Document Object Model) API'sine eri�im sa�lar ve kod �retimi ve analizi i�in kullan�l�r.
using ScintillaNET; // Scintilla metin d�zenleyicisinin .NET ba�lam�nda kullan�labilir hale getirilmesini sa�lar.
using System.Text.RegularExpressions; // D�zenli ifadelerle e�le�me ve arama i�lemleri i�in gerekli olan s�n�flar� i�erir.



//Metin Mert B�y�ko�lu -E235013169





namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Yaz� tipi se�im ile ilgili ileti�im kutusu
        FontDialog fontDialog = new FontDialog();

        // dosya i�lemleri i�in kullan�lacak nesneler.
        private bool dosyaKayitli;
        private bool dosyaGuncelle;
        private string seciliDosyaIsmi;

        // Yazd�rma i�lemlerini y�netmek i�in kullan�lan nesneler.
        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;

        public Form1()
        {

            InitializeComponent();

            // Ba�lang��ta "Dosyay� Kaydediniz." metnini g�ster
            toolStripStatusLabel1.Text = "Dosyay� Kaydediniz.";

            // Yazd�rma belgesi olu�tur
            printDocument = new PrintDocument();

            // Yazd�rma belgesi olaylar�
            printDocument.PrintPage += printDocument1_PrintPage;

            // Yazd�rma �nizleme ileti�im kutusunu olu�tur ve belgeyi ata
            printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;


        }

        private void kesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Metni kesme i�lemi
            richTextBox1.Cut();
        }

        private void kopyalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Metni kopyalama i�lemi
            richTextBox1.Copy();
        }

        private void yap��t�rToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Metni yap��t�rma i�lemi
            richTextBox1.Paste();
        }
        private void yaz��e�itiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Yaz� tipi se�im ile ilgili ileti�im kutusunu a�

            fontDialog.ShowApply = true; // Uygula butonunu g�ster
            fontDialog.Apply += new System.EventHandler(font_Uygula_Dialog); // Uygula butonu t�kland���nda olay� tan�mla

            // Yaz� tipi ile ilgili ileti�im kutusunu g�ster ve kullan�c�n�n se�imini al
            DialogResult result = fontDialog.ShowDialog();

            // Kullan�c� se�imini de�erlendir
            if (result == DialogResult.OK)
            {
                // Metinde bir se�im yap�lm�� m� kontrol et
                if (richTextBox1.SelectionLength > 0)
                {
                    // Se�ili metnin yaz� tipini ve rengini de�i�tir
                    richTextBox1.SelectionFont = fontDialog.Font;


                }
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Metni kesme i�lemi
            richTextBox1.Cut();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // Metni yap��t�rma i�lemi
            richTextBox1.Copy();
        }

        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Geri alma ve ileri alma i�levlerini devre d��� b�rak
            ileriAlToolStripMenuItem.Enabled = false;
            geriAlToolStripMenuItem.Enabled = false;

            // E�er dosya g�ncellendi ise
            if (dosyaGuncelle)
            {
                // Kullan�c�ya de�i�iklikleri kaydetmek isteyip istemedi�ini sor
                DialogResult result = MessageBox.Show("De�i�ikleri kaydet?", "Dosyay� Kaydet", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        // Dosyay� kaydetme ve g�ncelleme i�lemi
                        DosyaKayitGuncelle();
                        break;

                    case DialogResult.No:
                        // Ekran� temizleme i�lemi
                        Ekran�Temizle();
                        break;
                }
            }
            else
            {
                // E�er dosya g�ncellenmemi�se, ekran� temizle
                Ekran�Temizle();
            }

            // Geri alma ve ileri alma i�levlerini tekrar devre d��� b�rak
            geriAlToolStripMenuItem.Enabled = false;
            ileriAlToolStripMenuItem.Enabled = false;

            // Durum �ubu�una "Yeni Dosya Olu�turuldu." mesaj�n� g�ster
            toolStripStatusLabel1.Text = "Yeni Dosya Olu�turuldu.";
        }

        private void dosyaKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Dosyay� kaydetme i�lemi
            DosyaKayitGuncelle();

            // Durum �ubu�una "Dosya Kaydedildi." mesaj�n� g�ster
            toolStripStatusLabel1.Text = "Dosya Kaydedildi.";

        }

        private void DosyaKayitGuncelle()
        {
            // E�er dosya daha �nce kaydedilmi�se
            if (dosyaKayitli)
            {
                // Dosya uzant�s�na g�re farkl� kaydetme i�lemleri yap
                if (Path.GetExtension(seciliDosyaIsmi) == ".txt")
                {
                    // Metni d�z metin olarak kaydet
                    richTextBox1.SaveFile(seciliDosyaIsmi, RichTextBoxStreamType.PlainText);
                }
                if (Path.GetExtension(seciliDosyaIsmi) == ".rtf")
                {
                    // Metni zengin metin format�nda kaydet
                    richTextBox1.SaveFile(seciliDosyaIsmi, RichTextBoxStreamType.RichText);
                }

                // Dosya g�ncelleme i�lemini tamamla
                dosyaGuncelle = false;

            }
            else
            {
                // Dosya daha �nce kaydedilmediyse
                if (dosyaGuncelle)
                {
                    // Dosyay� kaydetme i�lemi
                    DosyaKaydet();
                }
                else
                {
                    // Ekran� temizleme i�lemi
                    Ekran�Temizle();
                }

            }
        }

        private void Ekran�Temizle()
        {
            // Metin kutusunu temizle
            richTextBox1.Clear();

            // Dosya g�ncelleme i�aret�isini false olarak ayarla
            dosyaGuncelle = false;

            // Form ba�l���n� varsay�lan olarak ayarla
            this.Text = "Not Defteri";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

            // Geri alma i�levini etkinle�tir
            geriAlToolStripMenuItem.Enabled = true;

            // Geri alma i�levini etkinle�tir
            dosyaGuncelle = true;

        }

        private void ��k��ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // E�er dosya g�ncellendi ise
            if (dosyaGuncelle)
            {
                // Kullan�c�ya de�i�iklikleri kaydetmek isteyip istemedi�ini sor
                DialogResult result = MessageBox.Show("De�i�ikleri kaydet?", "Dosyay� Kaydet", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:

                        // Dosyay� kaydetme ve ��k�� i�lemi
                        DosyaKayitGuncelle();
                        break;

                    case DialogResult.No:

                        // Uygulamadan ��k
                        Application.Exit();
                        break;
                }
            }
            else
            {
                // E�er dosya g�ncellenmediyse, uygulamadan ��k
                Application.Exit();

            }

        }

        private void yaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Yaz� rengi se�me ileti�im kutusunu g�ster
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowColor = true;
            DialogResult result = fontDialog.ShowDialog();

            // Kullan�c�n�n se�imini de�erlendir
            if (result == DialogResult.OK)
            {
                // E�er bir metin se�ilmi�se
                if (richTextBox1.SelectionLength > 0)
                {
                    // Se�ili metnin rengini de�i�tir
                    richTextBox1.SelectionColor = fontDialog.Color;
                }
            }
        }

        private void zeminRengiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Zemin rengi ileti�im kutusunu g�ster ve kullan�c�n�n se�ti�i rengi metin kutusunun arka plan rengi olarak ayarla
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                richTextBox1.BackColor = colorDialog1.Color;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form y�klendi�inde ba�lang�� de�erlerini ayarla

            // Dosyan�n kaydedilip kaydedilmedi�i bilgisini s�f�rla
            dosyaKayitli = false;

            // Dosyan�n g�ncellenip g�ncellenmedi�i bilgisini s�f�rla
            dosyaGuncelle = false;

            // Se�ili dosya ismini bo� bir dize olarak ayarla
            seciliDosyaIsmi = "";

            // Caps Lock tu�unun durumunu kontrol et
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                // E�er Caps Lock a��ksa durum �ubu�una "CAPS ON" mesaj�n� g�ster
                CapstoolStripStatusLabel2.Text = "CAPS ON";
            }
            else
            {
                // E�er Caps Lock kapal�ysa durum �ubu�una "caps off" mesaj�n� g�ster
                CapstoolStripStatusLabel2.Text = "caps off";

            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // Metni yap��t�r
            richTextBox1.Paste();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Form kapat�lmaya �al���ld���nda dosya g�ncellenmi�se kullan�c�ya kaydetme se�ene�i sun


            if (dosyaGuncelle)
            {
                // Dosya g�ncellenmi�se
                DialogResult result = MessageBox.Show("De�i�ikleri kaydet?", "Dosyay� Kaydet", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        // Dosyay� kaydet ve formu kapat
                        DosyaKayitGuncelle();
                        break;
                    case DialogResult.No:
                        // Kaydetme se�ene�i olmadan formu kapat
                        e.Cancel = false;
                        break;
                    
                        
                }
            }
            else
            {
                // Dosya g�ncellenmediyse formu kapat
                e.Cancel = false;
            }



        }

        private void dosyaA�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Geri alma ve ileri alma i�levlerini devre d��� b�rak
            ileriAlToolStripMenuItem.Enabled = false;
            geriAlToolStripMenuItem.Enabled = false;

            // E�er dosya g�ncellenmi�se
            if (dosyaGuncelle)
            {
                // Kullan�c�ya de�i�iklikleri kaydetmek isteyip istemedi�ini sor
                DialogResult result = MessageBox.Show("De�i�ikleri kaydet?", "Dosyay� Kaydet", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        // Dosyay� kaydet ve dosya a�ma i�lemine devam et
                        DosyaKayitGuncelle();
                        break;

                    // Dosyay� kaydetmeden dosya a�ma i�lemine devam et
                    case DialogResult.No:
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Metin Dosyalar� (*.txt)|*.txt|Zengin Metin Dosyalar� (*.rtf)|*.rtf";
                        DialogResult result1 = openFileDialog.ShowDialog();

                        if (result1 == DialogResult.OK)
                        {
                            // Dosya uzant�s�na g�re farkl� kaydetme i�lemleri yap
                            if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                            {
                                // Metni d�z metin olarak kaydet
                                richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                            }
                            if (Path.GetExtension(openFileDialog.FileName) == ".rtf")
                            {
                                // Metni zengin metin format�nda kaydet
                                richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.RichText);
                            }

                        }
                        // Form ba�l���n� g�ncelle
                        this.Text = Path.GetFileName(openFileDialog.FileName) + " -Not Defteri";

                        // Dosyan�n kaydedildi�i bilgisini g�ncelle
                        dosyaKayitli = true;

                        // Dosyan�n g�ncellendi�i bilgisini s�f�rla
                        dosyaGuncelle = false;

                        // Se�ili dosya ad�n� g�ncelle
                        seciliDosyaIsmi = openFileDialog.FileName;

                        // Durum �ubu�una mesaj g�ster
                        toolStripStatusLabel1.Text = "Dosya A��ld�.";
                        break;
                }
            }
            else
            {

                // E�er dosya g�ncellenmediyse dosya a�ma i�lemine devam et
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Metin Dosyalar� (*.txt)|*.txt|Zengin Metin Dosyalar� (*.rtf)|*.rtf";
                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Dosya uzant�s�na g�re farkl� kaydetme i�lemleri yap
                    if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                    {
                        // Metni d�z metin olarak kaydet
                        richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                    }
                    if (Path.GetExtension(openFileDialog.FileName) == ".rtf")
                    {
                        // Metni zengin metin format�nda kaydet
                        richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.RichText);
                    }

                }

                // Form ba�l���n� g�ncelle
                this.Text = Path.GetFileName(openFileDialog.FileName) + " -Not Defteri";

                // Dosyan�n kaydedildi�i bilgisini g�ncelle
                dosyaKayitli = true;

                // Dosyan�n g�ncellendi�i bilgisini s�f�rla
                dosyaGuncelle = false;

                // Se�ili dosya ad�n� g�ncelle
                seciliDosyaIsmi = openFileDialog.FileName;

                // Durum �ubu�una mesaj g�ster
                toolStripStatusLabel1.Text = "Dosya A��ld�.";
            }

        }

        private void geriAlToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // "Geri Al" men� ��esine t�kland���nda

            // Metin kutusunda geri alma i�lemini ger�ekle�tir
            richTextBox1.Undo();

            // "Geri Al" ��esini devre d��� b�rak
            geriAlToolStripMenuItem.Enabled = false;

            // "�leri Al" ��esini etkinle�tir
            ileriAlToolStripMenuItem.Enabled = true;
        }

        private void ileriAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "�leri Al" men� ��esine t�kland���nda

            // Metin kutusunda ileri alma i�lemini ger�ekle�tir
            richTextBox1.Redo();

            // "�leri Al" ��esini devre d��� b�rak
            ileriAlToolStripMenuItem.Enabled = false;

            // "Geri Al" ��esini etkinle�tir
            geriAlToolStripMenuItem.Enabled = true;
        }

        private void bi�imToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            // "Bi�im" men�s�ndeki "Tarih ve Saat Ekle" ��esine t�kland���nda

            // Metin kutusunun se�ili metin k�sm�n� mevcut tarih ve saatle de�i�tir
            richTextBox1.SelectedText = DateTime.Now.ToString();
        }
        private void hepsiniSe�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Hepsini Se�" men� ��esine t�kland���nda

            // Metin kutusunda t�m metni se�
            richTextBox1.SelectAll();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // "Arka Plan Rengi De�i�tir" men� ��esine t�kland���nda

            // E�er renk se�im ileti�im kutusu ba�ar�l� bir �ekilde tamamland�ysa
            if (colorDialog1.ShowDialog() == DialogResult.OK)

                // Metin kutusunun arka plan rengini se�ilen renge ayarla
                richTextBox1.BackColor = colorDialog1.Color;

        }

        private void uygulamaHakk�ndaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Uygulama Hakk�nda" men� ��esine t�kland���nda

            // Kullan�c�ya uygulama hakk�nda bilgi veren bir ileti�im kutusu g�ster
            MessageBox.Show("Bu Uygulama Metin Mert B�y�ko�lu Taraf�ndan Geli�tirilmi�tir.", "Not Defteri", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void farkl�KaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Farkl� Kaydet" men� ��esine t�kland���nda

            // Dosyay� kaydetme i�lemini ger�ekle�tir
            DosyaKaydet();


        }

        private void DosyaKaydet()
        {

            // Dosyay� kaydetme i�lemini ger�ekle�tiren metod

            // Dosya kaydetme ileti�im kutusunu olu�tur
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Dosya filtrelerini belirle
            saveFileDialog.Filter = "Metin Dosyalar�(*.txt)|*.txt|Zengin Metin Dosyalar� (*.rtf)|*.rtf";

            // Dosya kaydetme ileti�im kutusunu g�ster ve sonucunu al
            DialogResult savefileresult = saveFileDialog.ShowDialog();


            // E�er kullan�c� dosyay� kaydetmek isterse
            if (savefileresult == DialogResult.OK)
            {

                // Dosya uzant�s�na g�re farkl� kaydetme i�lemleri yap
                if (Path.GetExtension(saveFileDialog.FileName) == ".txt")
                {
                    // Metni d�z metin olarak kaydet
                    richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                }
                if (Path.GetExtension(saveFileDialog.FileName) == ".rtf")
                {
                    // Metni zengin metin format�nda kaydet
                    richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);
                }

            }

            // Form ba�l���n� g�ncelle
            this.Text = Path.GetFileName(saveFileDialog.FileName) + " -Not Defteri";

            // Dosyan�n kaydedildi�i bilgisini g�ncelle
            dosyaKayitli = true;

            // Dosyan�n g�ncellendi�i bilgisini s�f�rla
            dosyaGuncelle = false;

            // Se�ili dosya ad�n� g�ncelle
            seciliDosyaIsmi = saveFileDialog.FileName;
        }
        private void font_Uygula_Dialog(object sender, EventArgs e)
        {

            // Yaz� tipi uygulama ileti�im kutusunun Uygula olay� tetiklendi�inde

            // E�er metin kutusunda bir metin se�iliyse
            if (richTextBox1.SelectionLength > 0)
            {

                // Se�ili metnin yaz� tipini ve rengini yaz� tipi ileti�im kutusundan gelen de�erlerle ayarla
                richTextBox1.SelectionFont = fontDialog.Font;
                richTextBox1.SelectionColor = fontDialog.Color;
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

            // Metin kutusunda bir tu�a bas�ld���nda

            // Caps Lock tu�u etkinle�tirilmi�se
            if (Control.IsKeyLocked(Keys.CapsLock))
            {

                // Durum �ubu�unda "CAPS ON" mesaj�n� g�ster
                CapstoolStripStatusLabel2.Text = "CAPS ON";
            }
            else
            {

                // Caps Lock tu�u etkin de�ilse
                // Durum �ubu�unda "caps off" mesaj�n� g�ster
                CapstoolStripStatusLabel2.Text = "caps off";

            }
        }

        private void yazd�rToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // "Yazd�r" men� ��esine t�kland���nda

            // Yazd�rma ileti�im kutusunu olu�tur ve belgeyi belirle
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            // Yazd�rma ileti�im kutusunu g�ster ve kullan�c�n�n se�imine g�re belgeyi yazd�r
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }

        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Belge yazd�r�lmak �zere ayarland���nda

            // Formun g�r�nt�s�n� bir bit e�lemi olarak olu�tur
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));

            // Olu�turulan bit e�lemi �zerine belgeyi �iz
            e.Graphics.DrawImage(bitmap, 0, 0);

            // Kullan�lan kaynaklar� serbest b�rak
            bitmap.Dispose();
        }

        private void yaz�ArkaplanRengiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // "Yaz� Arkaplan Rengi" men� ��esine t�kland���nda

            // Renk ileti�im kutusunu olu�tur
            ColorDialog colorDialog = new ColorDialog();

            // E�er renk ileti�im kutusu ba�ar�l� bir �ekilde tamamland�ysa
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {

                // Metin kutusunda se�ili metnin arka plan rengini se�ilen renge ayarla
                richTextBox1.SelectionBackColor = colorDialog.Color;


            }
        }

        private void yazd�rma�nizleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // "Yazd�rma �nizleme" men� ��esine t�kland���nda

            // Yazd�rma �nizleme ileti�im kutusunu g�ster
            printPreviewDialog.ShowDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            // "Kaydet" ara� �ubu�u d��mesine t�kland���nda

            // Dosya kaydetme i�lemini ger�ekle�tir ve durum �ubu�unda bilgilendirme g�ster
            DosyaKayitGuncelle();
            toolStripStatusLabel1.Text = "Dosya Kaydedildi.";
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

            // "Geri Al" ara� �ubu�u d��mesine t�kland���nda

            // Metin kutusunda geri alma i�lemini ger�ekle�tir
            richTextBox1.Undo();

            // "Geri Al" d��mesini devre d��� b�rak ve "�leri Al" d��mesini etkinle�tir
            geriAlToolStripMenuItem.Enabled = false;
            ileriAlToolStripMenuItem.Enabled = true;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            // "�leri Al" ara� �ubu�u d��mesine t�kland���nda

            // Metin kutusunda ileri alma i�lemini ger�ekle�tir
            richTextBox1.Redo();

            // "�leri Al" d��mesini devre d��� b�rak ve "Geri Al" d��mesini etkinle�tir
            ileriAlToolStripMenuItem.Enabled = false;
            geriAlToolStripMenuItem.Enabled = true;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            // "Hepsini Se�" ara� �ubu�u d��mesine t�kland���nda

            // Metin kutusunda t�m metni se�
            richTextBox1.SelectAll();
        }

        private void yaz�RengiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Yaz� Rengi" men� ��esine t�kland���nda

            // Renk ileti�im kutusunu olu�tur
            ColorDialog colorDialog = new ColorDialog();

            // E�er renk ileti�im kutusu ba�ar�l� bir �ekilde tamamland�ysa
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {

                // Metin kutusunda se�ili metnin rengini se�ilen renge ayarla
                richTextBox1.SelectionColor = colorDialog.Color;
            }
        }
       
    }
}
