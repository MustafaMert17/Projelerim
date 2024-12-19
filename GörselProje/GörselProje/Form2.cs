using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GörselProje
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=DESKTOP-JS1BKMK\\SQLEXPRESS;Initial Catalog=hastakayit;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True;Integrated Security=True;";

        public void LoadPatientData()
        {
            // Clear existing items
            listBox1.Items.Clear();

            // SQL connection to fetch data
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                try
                {
                    connect.Open();

                    // SQL query to select data
                    string query = "SELECT hasta_adi, hasta_soyadi, tcno, cinsiyet, kan_grubu FROM hastakayit";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string hastaAdi = reader["hasta_adi"].ToString();
                                string hastaSoyadi = reader["hasta_soyadi"].ToString();
                                string tcNo = reader["tcno"].ToString();
                                string cinsiyet = reader["cinsiyet"].ToString();
                                string kanGrubu = reader["kan_grubu"].ToString();

                                // Add formatted patient info to ListBox
                                listBox1.Items.Add($"Adı Soyadı: {hastaAdi} {hastaSoyadi}, TC No: {tcNo}, Cinsiyet: {cinsiyet}, Kan Grubu: {kanGrubu}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        public void AddPatientToList(string patientInfo)
        {
            listBox1.Items.Add(patientInfo); // ListBox'a hasta bilgisini ekler
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
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
        private void Form2_Load(object sender, EventArgs e)
        {
            ListeyiYukle();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string searchTc = textBox7.Text.Trim();

            // If the search box is empty, display an error message
            if (string.IsNullOrEmpty(searchTc))
            {
                MessageBox.Show("Lütfen TC No girin.");
                return;
            }

            // Clear the ListBox to show the new results
            listBox1.Items.Clear();

            // Search for the patient with the given TC number
            SearchPatientByTc(searchTc);
        }

        private void SearchPatientByTc(string tcNo)
        {
            // SQL connection to fetch data
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                try
                {
                    connect.Open();

                    // SQL query to select data where TC No matches
                    string query = "SELECT hasta_adi, hasta_soyadi, tcno, cinsiyet, kan_grubu FROM hastakayit WHERE tcno = @tcno";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@tcno", tcNo); // Use exact match for TC No

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // If the patient exists, add them to ListBox
                            bool patientFound = false;
                            while (reader.Read())
                            {
                                patientFound = true;
                                string hastaAdi = reader["hasta_adi"].ToString();
                                string hastaSoyadi = reader["hasta_soyadi"].ToString();
                                string tcno = reader["tcno"].ToString();
                                string cinsiyet = reader["cinsiyet"].ToString();
                                string kanGrubu = reader["kan_grubu"].ToString();

                                // Add the matching patient info to ListBox
                                listBox1.Items.Add($"Adı Soyadı: {hastaAdi} {hastaSoyadi}, TC No: {tcno}, Cinsiyet: {cinsiyet}, Kan Grubu: {kanGrubu}");
                            }

                            // If no patient was found
                            if (!patientFound)
                            {
                                MessageBox.Show("Bu TC No'ya ait hasta bulunamadı.");
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

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sıralama.Azsıralama(listBox1);
        }
    }
}
