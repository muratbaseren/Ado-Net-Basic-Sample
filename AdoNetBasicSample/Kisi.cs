using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetBasicSample
{
    // Veritabanındaki kişiler tablosundaki satırları temsil eden nesne
    public class Kisi
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TcNo { get; set; }
        public string EPosta { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public DateTime DogumTarihi { get; set; }

        public override string ToString()
        {
            return Ad + " " + Soyad;
        }
    }
}
