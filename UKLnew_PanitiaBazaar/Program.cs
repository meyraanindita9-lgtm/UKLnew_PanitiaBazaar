using System;
using System.Collections.Generic;
using System.Linq;
   
namespace SistemManajemenStandBazaar
{
    // =========================================================================
    // POIN 1: CLASS INDUK (Stand.cs) -> ENKAPSULASI & PONDASI UTAMA
    // =========================================================================

    public class Stand
    {
        protected string _namaStand;
        protected double _hargaSewaPerHari;
        protected bool _isAvailable;

        public Stand(string namaStand, double hargaSewaPerHari)
        {
            NamaStand = namaStand;
            HargaSewaPerHari = hargaSewaPerHari;
            _isAvailable = true;
        }

        public string NamaStand
        {
            get { return _namaStand; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nama stand tidak boleh kosong.");
                _namaStand = value;
            }
        }

        public double HargaSewaPerHari
        {
            get { return _hargaSewaPerHari; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Harga harus lebih besar dari 0.");
                _hargaSewaPerHari = value;
            }
        }

        public bool IsAvailable
        {
            get { return _isAvailable; }
        }

        public void DisplayInfo()
        {
            string statusStr = _isAvailable ? "Tersedia" : "Tidak tersedia";
            Console.WriteLine($"{_namaStand,-15} | Rp {_hargaSewaPerHari,-12} / hari    | {statusStr}");
        }

        public void UbahStatus()
        {
            _isAvailable = !_isAvailable;
        }

        public virtual double HitungTotal(int jumlahHari)
        {
            return _hargaSewaPerHari * jumlahHari;
        }
    }


    // =========================================================================
    // POIN 2: CLASS STAND OUTDOOR (StandOutdoor.cs) -> INHERITANCE & OVERRIDE 1
    // =========================================================================
    
    public class StandOutdoor : Stand
    {
        protected double _biayaTenda = 75000;

        public StandOutdoor(string namaStand, double hargaSewaPerHari) : base(namaStand, hargaSewaPerHari)
        {
        }

        public double BiayaTenda
        {
            get { return _biayaTenda; }
        }

        public override double HitungTotal(int jumlahHari)
        {
            return (HargaSewaPerHari * jumlahHari) + (_biayaTenda * jumlahHari);
        }
    }


    // =========================================================================
    // POIN 3: CLASS STAND INDOOR (StandIndoor.cs) -> INHERITANCE & OVERRIDE 2
    // =========================================================================
    
    public class StandIndoor : Stand
    {
        protected double _biayaListrik = 100000;

        public StandIndoor(string namaStand, double hargaSewaPerHari) : base(namaStand, hargaSewaPerHari)
        {
        }

        public double BiayaListrik
        {
            get { return _biayaListrik; }
        }

        public override double HitungTotal(int jumlahHari)
        {
            return (HargaSewaPerHari * jumlahHari) + (_biayaListrik * jumlahHari);
        }
    }


    // =========================================================================
    // POIN 4: CLASS STAND PREMIUM (StandPremium.cs) -> ATURAN BIAYA FLAT
    // =========================================================================
    
    public class StandPremium : Stand
    {
        protected double _biayaKeamanan = 300000;

        public StandPremium(string namaStand, double hargaSewaPerHari) : base(namaStand, hargaSewaPerHari)
        {
        }

        public double BiayaKeamanan
        {
            get { return _biayaKeamanan; }
        }

        public override double HitungTotal(int jumlahHari)
        {
            return (HargaSewaPerHari * jumlahHari) + _biayaKeamanan;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Stand> daftarStand = new List<Stand>()
            {
                new StandOutdoor("Outdoor-1", 450000),
                new StandOutdoor("Outdoor-2", 500000),
                new StandIndoor("Indoor-1", 600000),
                new StandIndoor("Indoor-2", 700000),
                new StandPremium("Premium-1", 1800000),
                new StandPremium("Premium-2", 2000000)
            };

            bool berjalan = true;
            while (berjalan)
            {
                Console.Clear();
                Console.WriteLine("=== Moklet Expo Management Center ===");
                Console.WriteLine("Daftar Stand Tersedia:\n");

                foreach (var stand in daftarStand)
                {
                    if (stand.IsAvailable)
                    {
                        stand.DisplayInfo();
                    }
                }

                Console.WriteLine("\n1. Sewa Stand");
                Console.WriteLine("2. Akhiri Sewa Stand");
                Console.WriteLine("3. Keluar");
                Console.Write("\nPilih Menu: ");
                string pilihan = Console.ReadLine();

                if (pilihan == "1") MenuSewaStand(daftarStand);
                else if (pilihan == "2") MenuAkhiriSewa(daftarStand);
                else if (pilihan == "3")
                {
                    Console.WriteLine("\nTerima kasih...");
                    Console.WriteLine("\nTekan ENTER untuk keluar...");
                    Console.ReadLine();
                    berjalan = false;
                }
            }
        }

        static void MenuSewaStand(List<Stand> daftarStand)
        {
            Console.Write("\nMasukkan nama stand yang ingin disewa: ");
            string namaInput = Console.ReadLine();

            Stand standDicari = daftarStand.FirstOrDefault(s => s.NamaStand.Equals(namaInput, StringComparison.OrdinalIgnoreCase));

            if (standDicari == null)
                Console.WriteLine("Stand tidak ditemukan.");
            else if (!standDicari.IsAvailable)
                Console.WriteLine("Stand sedang tidak tersedia.");
            else
            {
                Console.Write("Masukkan jumlah hari sewa: ");
                if (int.TryParse(Console.ReadLine(), out int hari) && hari > 0)
                {
                    Console.WriteLine($"\nTotal Biaya: Rp {standDicari.HitungTotal(hari)}");
                    standDicari.UbahStatus();
                    Console.WriteLine($"Stand {standDicari.NamaStand} berhasil disewa!");
                }
                else
                {
                    Console.WriteLine("Jumlah hari harus berupa angka positif.");
                }
            }
            Console.WriteLine("\nTekan ENTER untuk kembali ke menu...");
            Console.ReadLine();
        }

        static void MenuAkhiriSewa(List<Stand> daftarStand)
        {
            Console.WriteLine("\nDaftar Stand yang Sedang Disewakan:");
            bool adaSewa = false;
            foreach (var stand in daftarStand)
            {
                if (!stand.IsAvailable)
                {
                    stand.DisplayInfo();
                    adaSewa = true;
                }
            }
            if (!adaSewa) Console.WriteLine("Tidak ada stand yang sedang disewa.");

            Console.Write("\nMasukkan nama stand yang ingin diselesaikan: ");
            string namaInput = Console.ReadLine();

            Stand standDicari = daftarStand.FirstOrDefault(s => s.NamaStand.Equals(namaInput, StringComparison.OrdinalIgnoreCase));

            if (standDicari == null)
                Console.WriteLine("Stand tidak ditemukan.");
            else if (standDicari.IsAvailable)
                Console.WriteLine("Stand belum disewa.");
            else
            {
                standDicari.UbahStatus();
                Console.WriteLine($"Sewa stand {standDicari.NamaStand} berhasil diakhiri.");
            }
            Console.WriteLine("\nTekan ENTER untuk kembali ke menu...");
            Console.ReadLine();
        }
    }
}