using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GörselProje
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=DESKTOP-JS1BKMK\\SQLEXPRESS;Initial Catalog=hastakayit;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True;Integrated Security=True;";
        SqlConnection baglanti = new SqlConnection();
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void ListeyiYukle()
        {
            // Öncelikle ListBox'ı temizle
            listBox1.Items.Clear();

            // SQL bağlantısını kullanarak veriyi çek
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                try
                {
                    connect.Open();

                    // SQL SELECT sorgusu
                    string query = "SELECT hasta_adi, hasta_soyadi, tcno, cinsiyet, kan_grubu FROM hastakayit";

                    // SqlCommand ve SqlDataReader kullanarak veriyi oku
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Her bir satırı uygun formatta listBox'a ekle
                                string hastaAdi = reader["hasta_adi"].ToString();
                                string hastaSoyadi = reader["hasta_soyadi"].ToString();
                                string tcNo = reader["tcno"].ToString();
                                string cinsiyet = reader["cinsiyet"].ToString();
                                string kanGrubu = reader["kan_grubu"].ToString();

                                // ListBox'a eklenen format
                                listBox1.Items.Add($"Adı Soyadı: {hastaAdi} {hastaSoyadi}, " +
                                                    $"TC No: {tcNo}, " +
                                                    $"Cinsiyet: {cinsiyet}, " +
                                                    $"Kan Grubu: {kanGrubu}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}");
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

       private void button3_Click(object sender, EventArgs e)
{
    // TextBox'lardan alınan hasta bilgileri
    string hastaAdi = textBox1.Text;
    string hastaSoyadi = textBox2.Text;
    string tcNo = textBox3.Text;
    string cinsiyet = "";

    if (checkBox1.Checked) cinsiyet = "Erkek";
    if (checkBox2.Checked) cinsiyet = "Kadın";

    string kanGrubu = "";
    foreach (var item in checkedListBox1.CheckedItems)
    {
        kanGrubu += item.ToString() + " ";
    }

    // Gerekli alanların boş olup olmadığını kontrol et
    if (string.IsNullOrEmpty(hastaAdi) || string.IsNullOrEmpty(hastaSoyadi) ||
        string.IsNullOrEmpty(tcNo) || string.IsNullOrEmpty(cinsiyet) ||
        string.IsNullOrEmpty(kanGrubu))
    {
        MessageBox.Show("Lütfen tüm alanları doldurduğunuzdan emin olun.");
        return;
    }

    // Hasta bilgilerini birleştiriyoruz
    string hastaBilgileri = $"Adı Soyadı: {hastaAdi} {hastaSoyadi}, " +
                             $"TC No: {tcNo}, " +
                             $"Cinsiyet: {cinsiyet}, " +
                             $"Kan Grubu: {kanGrubu}, " +
                             $"VT KAYDEDİLMEDİ !";

    // ListBox'a hasta bilgilerini ekliyoruz (Form1'in listBox1)
    listBox1.Items.Add(hastaBilgileri);

    // Form2'e hasta bilgilerini ekliyoruz
    Form2 form2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();
    if (form2 != null)
    {
        form2.AddPatientToList(hastaBilgileri);
    }

    // Formu temizliyoruz
    textBox1.Clear();
    textBox2.Clear();
    textBox3.Clear();
    checkedListBox1.ClearSelected();
    checkBox1.Checked = false;
    checkBox2.Checked = false;
}


        private void button2_Click(object sender, EventArgs e)
        {
            // ListBox'dan seçilen öğeyi alalım
            if (listBox1.SelectedItem != null)
            {
                // Seçilen öğeyi al
                string selectedItem = listBox1.SelectedItem.ToString();

                // TC No'yu seçilen öğeden ayıralım (Örnek format: "Adı Soyadı: Ahmet Yılmaz, TC No: 12345678901, ...")
                string tcNo = ExtractTcNoFromString(selectedItem);

                // Eğer TC No null değilse, veritabanından ve ListBox'tan silme işlemi yapalım
                if (!string.IsNullOrEmpty(tcNo))
                {
                    // Veritabanından silme işlemi
                    DeletePatientFromDatabase(tcNo);

                    // ListBox'tan silme işlemi
                    listBox1.Items.Remove(listBox1.SelectedItem);

                    MessageBox.Show("Hasta başarıyla silindi.");
                }
                else
                {
                    MessageBox.Show("Geçersiz hasta bilgisi.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek hastayı seçin.");
            }
        }

        private string ExtractTcNoFromString(string patientInfo)
        {
            // "TC No: 12345678901" formatındaki bilgiyi ayıklayalım
            string tcNo = string.Empty;

            // TC No'yu almak için, "TC No: " ifadesinden sonrasını alıyoruz
            int startIndex = patientInfo.IndexOf("TC No: ") + 7; // "TC No: " ifadesinin sonrasından başla
            if (startIndex > 6) // Eğer "TC No: " ifadesi bulunmuşsa
            {
                tcNo = patientInfo.Substring(startIndex).Split(',')[0].Trim(); // TC No'yu ayıkla
            }

            return tcNo;
        }

        private void DeletePatientFromDatabase(string tcNo)
        {
            // SQL DELETE sorgusunu hazırlıyoruz
            string query = "DELETE FROM hastakayit WHERE tcno = @tcno";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@tcno", tcNo);

                        // Komutu çalıştırıyoruz
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Veritabanında işlem başarılı mı kontrol ediyoruz
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Veritabanında hasta bulunamadı.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'hastaneDataSet.hasta_kayit' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            ListeyiYukle();

        }

        private void button1_Click(object sender, EventArgs e)

        {

            // Textbox'lardan alınan hasta bilgileri
            string hastaAdi = textBox1.Text;
            string hastaSoyadi = textBox2.Text;
            string tcNo = textBox3.Text;
            string cinsiyet = "";

            if (checkBox1.Checked) cinsiyet = "Erkek";
            if (checkBox2.Checked) cinsiyet = "Kadın";

            string kanGrubu = "";
            foreach (var item in checkedListBox1.CheckedItems)
            {
                kanGrubu += item.ToString() + " ";
            }

            // Gerekli alanların boş olup olmadığını kontrol et
            if (string.IsNullOrEmpty(hastaAdi) || string.IsNullOrEmpty(hastaSoyadi) ||
                string.IsNullOrEmpty(tcNo) || string.IsNullOrEmpty(cinsiyet) ||
                string.IsNullOrEmpty(kanGrubu))
            {
                MessageBox.Show("Lütfen tüm alanları doldurduğunuzdan emin olun.");
                return;
            }

            // SQL sorgusunu hazırlıyoruz
            string query = "INSERT INTO hastakayit (hasta_adi,hasta_soyadi,tcno,cinsiyet, kan_grubu) " +
                           "VALUES (@hasta_adi, @hasta_soyadi, @tcno, @cinsiyet, @kan_grubu)";

            try
            {
                // Veritabanı bağlantısını açıyoruz
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL komutunu hazırlıyoruz
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // SQL enjeksiyonunu önlemek için parametreler ekliyoruz
                        cmd.Parameters.AddWithValue("@hasta_adi", hastaAdi);
                        cmd.Parameters.AddWithValue("@hasta_soyadi", hastaSoyadi);
                        cmd.Parameters.AddWithValue("@tcno", tcNo);
                        cmd.Parameters.AddWithValue("@cinsiyet", cinsiyet);
                        cmd.Parameters.AddWithValue("@kan_grubu", kanGrubu);

                        // Komutu çalıştırıyoruz
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Veritabanına başarılı bir şekilde veri eklendi mi kontrol ediyoruz
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Hasta başarıyla kaydedildi.");
                            ListeyiYukle();

                        }
                        else
                        {
                            MessageBox.Show("Bir hata oluştu. Hasta kaydedilemedi.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya hata mesajı gösteriyoruz
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sıralama.Azsıralama(listBox1);
        }
    }
}

