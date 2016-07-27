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

namespace AdoNetBasicSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string BaglantiCumlesi = "Server=[ServerName]; Database=[DatabaseName]; User Id=[sql_username]; Password=[sql_password];";

        private void btnVerileriGetir_Click(object sender, EventArgs e)
        {
            lstKisiler.Items.Clear();

            SqlConnection baglanti = new SqlConnection(BaglantiCumlesi);

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "SELECT * FROM Kisiler";

            baglanti.Open();

            // Veritabanında sonuçlar listelenir(sorgu çalışır) ama okuma daha başlamamıştır.
            SqlDataReader okuyucu = komut.ExecuteReader();

            //Veritabanından kayıt okudukça.. Her REad() metodu çalıştığında bir satır okur.
            while (okuyucu.Read())
            {
                Kisi kisi = new Kisi();
                kisi.Id = (int)okuyucu["Id"];
                kisi.Ad = okuyucu["Ad"].ToString();
                kisi.Soyad = okuyucu["Soyad"].ToString();
                kisi.TcNo = okuyucu["TcNo"].ToString();
                kisi.Telefon = okuyucu["Telefon"].ToString();
                kisi.EPosta = okuyucu["EPosta"].ToString();
                kisi.DogumTarihi = (DateTime)okuyucu["DogumTarihi"];
                kisi.Adres = okuyucu["Adres"].ToString();

                lstKisiler.Items.Add(kisi);
            }

            baglanti.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(BaglantiCumlesi);

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "INSERT INTO Kisiler(Ad, Soyad, TcNo, Telefon, EPosta, DogumTarihi, Adres) VALUES(@Ad, @Soyad, @TcNo, @Telefon, @EPosta, @DogumTarihi, @Adres)";

            // Parametre varsa parametre eklenir.
            komut.Parameters.AddWithValue("@Ad", txtAd.Text);
            komut.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            komut.Parameters.AddWithValue("@TcNo", txtTcNo.Text);
            komut.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
            komut.Parameters.AddWithValue("@EPosta", txtEposta.Text);
            komut.Parameters.AddWithValue("@DogumTarihi", dtpDogumTarihi.Value);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);

            baglanti.Open();

            komut.ExecuteNonQuery();    // Sorgu çalıştırılır.

            baglanti.Close();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(BaglantiCumlesi);

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "UPDATE Kisiler SET Ad=@Ad, Soyad=@Soyad, TcNo=@TcNo, Telefon=@Telefon, EPosta=@EPosta, DogumTarihi=@DogumTarihi, Adres=@Adres WHERE Id=@Id";

            // Parametre varsa parametre eklenir.
            komut.Parameters.AddWithValue("@Ad", txtAd.Text);
            komut.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            komut.Parameters.AddWithValue("@TcNo", txtTcNo.Text);
            komut.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
            komut.Parameters.AddWithValue("@EPosta", txtEposta.Text);
            komut.Parameters.AddWithValue("@DogumTarihi", dtpDogumTarihi.Value);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);

            Kisi seciliKisi = lstKisiler.SelectedItem as Kisi;
            komut.Parameters.AddWithValue("@Id", seciliKisi.Id);

            baglanti.Open();

            komut.ExecuteNonQuery();    // Sorgu çalıştırılır.

            baglanti.Close();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(BaglantiCumlesi);

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "DELETE FROM Kisiler WHERE Id=@Id";

            // Parametre varsa parametre eklenir.
            Kisi seciliKisi = lstKisiler.SelectedItem as Kisi;
            komut.Parameters.AddWithValue("@Id", seciliKisi.Id);
            
            baglanti.Open();

            komut.ExecuteNonQuery();    // Sorgu çalıştırılır.

            baglanti.Close();
        }
    }
}
