using System.Security.Cryptography.X509Certificates; // X.509 sertifikalarýyla çalýþmak için gereken sýnýflarý içerir.
using System.Windows.Forms; // Windows Forms uygulamalarý oluþturmak için gereken UI bileþenlerini saðlar.
using static System.Windows.Forms.VisualStyles.VisualStyleElement; // Görsel stilleri temsil eden öðeleri kullanarak özelleþtirilmiþ denetimler oluþturmayý saðlar.
using System.IO; // Dosya ve klasör iþlemleri için gerekli olan sýnýflarý içerir.
using System.Text; // Metin iþlemleri, kodlama dönüþümleri ve diðer metin tabanlý iþlemler için gerekli olan sýnýflarý içerir.
using System.Drawing.Printing; // Yazýcýlarla etkileþim kurmak ve belge yazdýrma iþlemleri yapmak için gerekli olan sýnýflarý içerir.
using System.IO.Packaging; // Paketlenmiþ dosyalarla çalýþmak için gerekli olan sýnýflarý içerir (örneðin, Open XML dosyalarý).
using System.Drawing; // Grafiklerle çalýþmak için gerekli olan sýnýflarý içerir (örneðin, resim oluþturma ve çizim iþlemleri).
using System.Drawing.Imaging; // Resim iþlemleri için gerekli olan sýnýflarý içerir (örneðin, resim dosyalarýný farklý biçimlerde kaydetme).
using System.Drawing.Text; // TrueType ve OpenType fontlarýný yönetmek ve kullanmak için gerekli olan sýnýflarý içerir.
using System.ComponentModel; // Bileþen tabanlý programlama modelinde kullanýlan bileþen sýnýflarýný içerir.
using System.Linq; // LINQ (Language Integrated Query) sorgularý için geniþletilmiþ yöntemler ve diðer araçlarý saðlar.
using System.Threading.Tasks; // Asenkron ve paralel programlama için Task sýnýflarý ve diðer araçlarý içerir.
using Microsoft.CSharp; // C# dilinde kod derlemesi ve yürütmesi için gereken sýnýflarý içerir.
using System.CodeDom.Compiler; // Kod derleme ve yürütme iþlemlerini gerçekleþtirmek için gerekli olan sýnýflarý içerir.
using System.CodeDom; // CodeDOM (Code Document Object Model) API'sine eriþim saðlar ve kod üretimi ve analizi için kullanýlýr.
using ScintillaNET; // Scintilla metin düzenleyicisinin .NET baðlamýnda kullanýlabilir hale getirilmesini saðlar.
using System.Text.RegularExpressions; // Düzenli ifadelerle eþleþme ve arama iþlemleri için gerekli olan sýnýflarý içerir.



//Metin Mert Býyýkoðlu -E235013169





namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Yazý tipi seçim ile ilgili iletiþim kutusu
        FontDialog fontDialog = new FontDialog();

        // dosya iþlemleri için kullanýlacak nesneler.
        private bool dosyaKayitli;
        private bool dosyaGuncelle;
        private string seciliDosyaIsmi;

        // Yazdýrma iþlemlerini yönetmek için kullanýlan nesneler.
        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;

        public Form1()
        {

            InitializeComponent();

            // Baþlangýçta "Dosyayý Kaydediniz." metnini göster
            toolStripStatusLabel1.Text = "Dosyayý Kaydediniz.";

            // Yazdýrma belgesi oluþtur
            printDocument = new PrintDocument();

            // Yazdýrma belgesi olaylarý
            printDocument.PrintPage += printDocument1_PrintPage;

            // Yazdýrma önizleme iletiþim kutusunu oluþtur ve belgeyi ata
            printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;


        }

        private void kesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Metni kesme iþlemi
            richTextBox1.Cut();
        }

        private void kopyalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Metni kopyalama iþlemi
            richTextBox1.Copy();
        }

        private void yapýþtýrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Metni yapýþtýrma iþlemi
            richTextBox1.Paste();
        }
        private void yazýÇeþitiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Yazý tipi seçim ile ilgili iletiþim kutusunu aç

            fontDialog.ShowApply = true; // Uygula butonunu göster
            fontDialog.Apply += new System.EventHandler(font_Uygula_Dialog); // Uygula butonu týklandýðýnda olayý tanýmla

            // Yazý tipi ile ilgili iletiþim kutusunu göster ve kullanýcýnýn seçimini al
            DialogResult result = fontDialog.ShowDialog();

            // Kullanýcý seçimini deðerlendir
            if (result == DialogResult.OK)
            {
                // Metinde bir seçim yapýlmýþ mý kontrol et
                if (richTextBox1.SelectionLength > 0)
                {
                    // Seçili metnin yazý tipini ve rengini deðiþtir
                    richTextBox1.SelectionFont = fontDialog.Font;


                }
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Metni kesme iþlemi
            richTextBox1.Cut();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // Metni yapýþtýrma iþlemi
            richTextBox1.Copy();
        }

        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Geri alma ve ileri alma iþlevlerini devre dýþý býrak
            ileriAlToolStripMenuItem.Enabled = false;
            geriAlToolStripMenuItem.Enabled = false;

            // Eðer dosya güncellendi ise
            if (dosyaGuncelle)
            {
                // Kullanýcýya deðiþiklikleri kaydetmek isteyip istemediðini sor
                DialogResult result = MessageBox.Show("Deðiþikleri kaydet?", "Dosyayý Kaydet", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        // Dosyayý kaydetme ve güncelleme iþlemi
                        DosyaKayitGuncelle();
                        break;

                    case DialogResult.No:
                        // Ekraný temizleme iþlemi
                        EkranýTemizle();
                        break;
                }
            }
            else
            {
                // Eðer dosya güncellenmemiþse, ekraný temizle
                EkranýTemizle();
            }

            // Geri alma ve ileri alma iþlevlerini tekrar devre dýþý býrak
            geriAlToolStripMenuItem.Enabled = false;
            ileriAlToolStripMenuItem.Enabled = false;

            // Durum çubuðuna "Yeni Dosya Oluþturuldu." mesajýný göster
            toolStripStatusLabel1.Text = "Yeni Dosya Oluþturuldu.";
        }

        private void dosyaKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Dosyayý kaydetme iþlemi
            DosyaKayitGuncelle();

            // Durum çubuðuna "Dosya Kaydedildi." mesajýný göster
            toolStripStatusLabel1.Text = "Dosya Kaydedildi.";

        }

        private void DosyaKayitGuncelle()
        {
            // Eðer dosya daha önce kaydedilmiþse
            if (dosyaKayitli)
            {
                // Dosya uzantýsýna göre farklý kaydetme iþlemleri yap
                if (Path.GetExtension(seciliDosyaIsmi) == ".txt")
                {
                    // Metni düz metin olarak kaydet
                    richTextBox1.SaveFile(seciliDosyaIsmi, RichTextBoxStreamType.PlainText);
                }
                if (Path.GetExtension(seciliDosyaIsmi) == ".rtf")
                {
                    // Metni zengin metin formatýnda kaydet
                    richTextBox1.SaveFile(seciliDosyaIsmi, RichTextBoxStreamType.RichText);
                }

                // Dosya güncelleme iþlemini tamamla
                dosyaGuncelle = false;

            }
            else
            {
                // Dosya daha önce kaydedilmediyse
                if (dosyaGuncelle)
                {
                    // Dosyayý kaydetme iþlemi
                    DosyaKaydet();
                }
                else
                {
                    // Ekraný temizleme iþlemi
                    EkranýTemizle();
                }

            }
        }

        private void EkranýTemizle()
        {
            // Metin kutusunu temizle
            richTextBox1.Clear();

            // Dosya güncelleme iþaretçisini false olarak ayarla
            dosyaGuncelle = false;

            // Form baþlýðýný varsayýlan olarak ayarla
            this.Text = "Not Defteri";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

            // Geri alma iþlevini etkinleþtir
            geriAlToolStripMenuItem.Enabled = true;

            // Geri alma iþlevini etkinleþtir
            dosyaGuncelle = true;

        }

        private void çýkýþToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Eðer dosya güncellendi ise
            if (dosyaGuncelle)
            {
                // Kullanýcýya deðiþiklikleri kaydetmek isteyip istemediðini sor
                DialogResult result = MessageBox.Show("Deðiþikleri kaydet?", "Dosyayý Kaydet", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:

                        // Dosyayý kaydetme ve çýkýþ iþlemi
                        DosyaKayitGuncelle();
                        break;

                    case DialogResult.No:

                        // Uygulamadan çýk
                        Application.Exit();
                        break;
                }
            }
            else
            {
                // Eðer dosya güncellenmediyse, uygulamadan çýk
                Application.Exit();

            }

        }

        private void yaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Yazý rengi seçme iletiþim kutusunu göster
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowColor = true;
            DialogResult result = fontDialog.ShowDialog();

            // Kullanýcýnýn seçimini deðerlendir
            if (result == DialogResult.OK)
            {
                // Eðer bir metin seçilmiþse
                if (richTextBox1.SelectionLength > 0)
                {
                    // Seçili metnin rengini deðiþtir
                    richTextBox1.SelectionColor = fontDialog.Color;
                }
            }
        }

        private void zeminRengiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Zemin rengi iletiþim kutusunu göster ve kullanýcýnýn seçtiði rengi metin kutusunun arka plan rengi olarak ayarla
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                richTextBox1.BackColor = colorDialog1.Color;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form yüklendiðinde baþlangýç deðerlerini ayarla

            // Dosyanýn kaydedilip kaydedilmediði bilgisini sýfýrla
            dosyaKayitli = false;

            // Dosyanýn güncellenip güncellenmediði bilgisini sýfýrla
            dosyaGuncelle = false;

            // Seçili dosya ismini boþ bir dize olarak ayarla
            seciliDosyaIsmi = "";

            // Caps Lock tuþunun durumunu kontrol et
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                // Eðer Caps Lock açýksa durum çubuðuna "CAPS ON" mesajýný göster
                CapstoolStripStatusLabel2.Text = "CAPS ON";
            }
            else
            {
                // Eðer Caps Lock kapalýysa durum çubuðuna "caps off" mesajýný göster
                CapstoolStripStatusLabel2.Text = "caps off";

            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // Metni yapýþtýr
            richTextBox1.Paste();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Form kapatýlmaya çalýþýldýðýnda dosya güncellenmiþse kullanýcýya kaydetme seçeneði sun


            if (dosyaGuncelle)
            {
                // Dosya güncellenmiþse
                DialogResult result = MessageBox.Show("Deðiþikleri kaydet?", "Dosyayý Kaydet", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        // Dosyayý kaydet ve formu kapat
                        DosyaKayitGuncelle();
                        break;
                    case DialogResult.No:
                        // Kaydetme seçeneði olmadan formu kapat
                        e.Cancel = false;
                        break;
                    
                        
                }
            }
            else
            {
                // Dosya güncellenmediyse formu kapat
                e.Cancel = false;
            }



        }

        private void dosyaAçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Geri alma ve ileri alma iþlevlerini devre dýþý býrak
            ileriAlToolStripMenuItem.Enabled = false;
            geriAlToolStripMenuItem.Enabled = false;

            // Eðer dosya güncellenmiþse
            if (dosyaGuncelle)
            {
                // Kullanýcýya deðiþiklikleri kaydetmek isteyip istemediðini sor
                DialogResult result = MessageBox.Show("Deðiþikleri kaydet?", "Dosyayý Kaydet", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        // Dosyayý kaydet ve dosya açma iþlemine devam et
                        DosyaKayitGuncelle();
                        break;

                    // Dosyayý kaydetmeden dosya açma iþlemine devam et
                    case DialogResult.No:
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Metin Dosyalarý (*.txt)|*.txt|Zengin Metin Dosyalarý (*.rtf)|*.rtf";
                        DialogResult result1 = openFileDialog.ShowDialog();

                        if (result1 == DialogResult.OK)
                        {
                            // Dosya uzantýsýna göre farklý kaydetme iþlemleri yap
                            if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                            {
                                // Metni düz metin olarak kaydet
                                richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                            }
                            if (Path.GetExtension(openFileDialog.FileName) == ".rtf")
                            {
                                // Metni zengin metin formatýnda kaydet
                                richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.RichText);
                            }

                        }
                        // Form baþlýðýný güncelle
                        this.Text = Path.GetFileName(openFileDialog.FileName) + " -Not Defteri";

                        // Dosyanýn kaydedildiði bilgisini güncelle
                        dosyaKayitli = true;

                        // Dosyanýn güncellendiði bilgisini sýfýrla
                        dosyaGuncelle = false;

                        // Seçili dosya adýný güncelle
                        seciliDosyaIsmi = openFileDialog.FileName;

                        // Durum çubuðuna mesaj göster
                        toolStripStatusLabel1.Text = "Dosya Açýldý.";
                        break;
                }
            }
            else
            {

                // Eðer dosya güncellenmediyse dosya açma iþlemine devam et
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Metin Dosyalarý (*.txt)|*.txt|Zengin Metin Dosyalarý (*.rtf)|*.rtf";
                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Dosya uzantýsýna göre farklý kaydetme iþlemleri yap
                    if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                    {
                        // Metni düz metin olarak kaydet
                        richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                    }
                    if (Path.GetExtension(openFileDialog.FileName) == ".rtf")
                    {
                        // Metni zengin metin formatýnda kaydet
                        richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.RichText);
                    }

                }

                // Form baþlýðýný güncelle
                this.Text = Path.GetFileName(openFileDialog.FileName) + " -Not Defteri";

                // Dosyanýn kaydedildiði bilgisini güncelle
                dosyaKayitli = true;

                // Dosyanýn güncellendiði bilgisini sýfýrla
                dosyaGuncelle = false;

                // Seçili dosya adýný güncelle
                seciliDosyaIsmi = openFileDialog.FileName;

                // Durum çubuðuna mesaj göster
                toolStripStatusLabel1.Text = "Dosya Açýldý.";
            }

        }

        private void geriAlToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // "Geri Al" menü öðesine týklandýðýnda

            // Metin kutusunda geri alma iþlemini gerçekleþtir
            richTextBox1.Undo();

            // "Geri Al" öðesini devre dýþý býrak
            geriAlToolStripMenuItem.Enabled = false;

            // "Ýleri Al" öðesini etkinleþtir
            ileriAlToolStripMenuItem.Enabled = true;
        }

        private void ileriAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Ýleri Al" menü öðesine týklandýðýnda

            // Metin kutusunda ileri alma iþlemini gerçekleþtir
            richTextBox1.Redo();

            // "Ýleri Al" öðesini devre dýþý býrak
            ileriAlToolStripMenuItem.Enabled = false;

            // "Geri Al" öðesini etkinleþtir
            geriAlToolStripMenuItem.Enabled = true;
        }

        private void biçimToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            // "Biçim" menüsündeki "Tarih ve Saat Ekle" öðesine týklandýðýnda

            // Metin kutusunun seçili metin kýsmýný mevcut tarih ve saatle deðiþtir
            richTextBox1.SelectedText = DateTime.Now.ToString();
        }
        private void hepsiniSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Hepsini Seç" menü öðesine týklandýðýnda

            // Metin kutusunda tüm metni seç
            richTextBox1.SelectAll();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // "Arka Plan Rengi Deðiþtir" menü öðesine týklandýðýnda

            // Eðer renk seçim iletiþim kutusu baþarýlý bir þekilde tamamlandýysa
            if (colorDialog1.ShowDialog() == DialogResult.OK)

                // Metin kutusunun arka plan rengini seçilen renge ayarla
                richTextBox1.BackColor = colorDialog1.Color;

        }

        private void uygulamaHakkýndaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Uygulama Hakkýnda" menü öðesine týklandýðýnda

            // Kullanýcýya uygulama hakkýnda bilgi veren bir iletiþim kutusu göster
            MessageBox.Show("Bu Uygulama Metin Mert Býyýkoðlu Tarafýndan Geliþtirilmiþtir.", "Not Defteri", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void farklýKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Farklý Kaydet" menü öðesine týklandýðýnda

            // Dosyayý kaydetme iþlemini gerçekleþtir
            DosyaKaydet();


        }

        private void DosyaKaydet()
        {

            // Dosyayý kaydetme iþlemini gerçekleþtiren metod

            // Dosya kaydetme iletiþim kutusunu oluþtur
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Dosya filtrelerini belirle
            saveFileDialog.Filter = "Metin Dosyalarý(*.txt)|*.txt|Zengin Metin Dosyalarý (*.rtf)|*.rtf";

            // Dosya kaydetme iletiþim kutusunu göster ve sonucunu al
            DialogResult savefileresult = saveFileDialog.ShowDialog();


            // Eðer kullanýcý dosyayý kaydetmek isterse
            if (savefileresult == DialogResult.OK)
            {

                // Dosya uzantýsýna göre farklý kaydetme iþlemleri yap
                if (Path.GetExtension(saveFileDialog.FileName) == ".txt")
                {
                    // Metni düz metin olarak kaydet
                    richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                }
                if (Path.GetExtension(saveFileDialog.FileName) == ".rtf")
                {
                    // Metni zengin metin formatýnda kaydet
                    richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);
                }

            }

            // Form baþlýðýný güncelle
            this.Text = Path.GetFileName(saveFileDialog.FileName) + " -Not Defteri";

            // Dosyanýn kaydedildiði bilgisini güncelle
            dosyaKayitli = true;

            // Dosyanýn güncellendiði bilgisini sýfýrla
            dosyaGuncelle = false;

            // Seçili dosya adýný güncelle
            seciliDosyaIsmi = saveFileDialog.FileName;
        }
        private void font_Uygula_Dialog(object sender, EventArgs e)
        {

            // Yazý tipi uygulama iletiþim kutusunun Uygula olayý tetiklendiðinde

            // Eðer metin kutusunda bir metin seçiliyse
            if (richTextBox1.SelectionLength > 0)
            {

                // Seçili metnin yazý tipini ve rengini yazý tipi iletiþim kutusundan gelen deðerlerle ayarla
                richTextBox1.SelectionFont = fontDialog.Font;
                richTextBox1.SelectionColor = fontDialog.Color;
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

            // Metin kutusunda bir tuþa basýldýðýnda

            // Caps Lock tuþu etkinleþtirilmiþse
            if (Control.IsKeyLocked(Keys.CapsLock))
            {

                // Durum çubuðunda "CAPS ON" mesajýný göster
                CapstoolStripStatusLabel2.Text = "CAPS ON";
            }
            else
            {

                // Caps Lock tuþu etkin deðilse
                // Durum çubuðunda "caps off" mesajýný göster
                CapstoolStripStatusLabel2.Text = "caps off";

            }
        }

        private void yazdýrToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // "Yazdýr" menü öðesine týklandýðýnda

            // Yazdýrma iletiþim kutusunu oluþtur ve belgeyi belirle
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            // Yazdýrma iletiþim kutusunu göster ve kullanýcýnýn seçimine göre belgeyi yazdýr
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }

        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Belge yazdýrýlmak üzere ayarlandýðýnda

            // Formun görüntüsünü bir bit eþlemi olarak oluþtur
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));

            // Oluþturulan bit eþlemi üzerine belgeyi çiz
            e.Graphics.DrawImage(bitmap, 0, 0);

            // Kullanýlan kaynaklarý serbest býrak
            bitmap.Dispose();
        }

        private void yazýArkaplanRengiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // "Yazý Arkaplan Rengi" menü öðesine týklandýðýnda

            // Renk iletiþim kutusunu oluþtur
            ColorDialog colorDialog = new ColorDialog();

            // Eðer renk iletiþim kutusu baþarýlý bir þekilde tamamlandýysa
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {

                // Metin kutusunda seçili metnin arka plan rengini seçilen renge ayarla
                richTextBox1.SelectionBackColor = colorDialog.Color;


            }
        }

        private void yazdýrmaÖnizleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // "Yazdýrma Önizleme" menü öðesine týklandýðýnda

            // Yazdýrma önizleme iletiþim kutusunu göster
            printPreviewDialog.ShowDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            // "Kaydet" araç çubuðu düðmesine týklandýðýnda

            // Dosya kaydetme iþlemini gerçekleþtir ve durum çubuðunda bilgilendirme göster
            DosyaKayitGuncelle();
            toolStripStatusLabel1.Text = "Dosya Kaydedildi.";
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

            // "Geri Al" araç çubuðu düðmesine týklandýðýnda

            // Metin kutusunda geri alma iþlemini gerçekleþtir
            richTextBox1.Undo();

            // "Geri Al" düðmesini devre dýþý býrak ve "Ýleri Al" düðmesini etkinleþtir
            geriAlToolStripMenuItem.Enabled = false;
            ileriAlToolStripMenuItem.Enabled = true;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            // "Ýleri Al" araç çubuðu düðmesine týklandýðýnda

            // Metin kutusunda ileri alma iþlemini gerçekleþtir
            richTextBox1.Redo();

            // "Ýleri Al" düðmesini devre dýþý býrak ve "Geri Al" düðmesini etkinleþtir
            ileriAlToolStripMenuItem.Enabled = false;
            geriAlToolStripMenuItem.Enabled = true;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            // "Hepsini Seç" araç çubuðu düðmesine týklandýðýnda

            // Metin kutusunda tüm metni seç
            richTextBox1.SelectAll();
        }

        private void yazýRengiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Yazý Rengi" menü öðesine týklandýðýnda

            // Renk iletiþim kutusunu oluþtur
            ColorDialog colorDialog = new ColorDialog();

            // Eðer renk iletiþim kutusu baþarýlý bir þekilde tamamlandýysa
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {

                // Metin kutusunda seçili metnin rengini seçilen renge ayarla
                richTextBox1.SelectionColor = colorDialog.Color;
            }
        }
       
    }
}
