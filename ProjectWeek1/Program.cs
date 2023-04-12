EventProduct e = new EventProduct();
e.ProcessCompleted += OnProses;

void OnProses(object sender, string pesan)
{
    Console.WriteLine(pesan);
}

void Display()
{
    Console.Clear();
    garis();
    Console.WriteLine(" Selamat datang di toko kami");
    garis();
    Console.WriteLine(" 1. Tambah produk");
    Console.WriteLine(" 2. Edit produk");
    Console.WriteLine(" 3. Tampilkan semua produk");
    Console.WriteLine(" 4. Hapus produk");
    Console.WriteLine(" 5. Tambah ke keranjang");
    Console.WriteLine(" 6. Hapus produk di keranjang");
    Console.WriteLine(" 7. Lihat keranjang");
    Console.WriteLine(" 8. Checkout");
    Console.WriteLine(" 9. Keluar");
    garis();
    Console.Write("Pilihan ? : ");
}

var cart = new Cart();
var produk = new List<Produk>();
int pilihan = 0;
while (pilihan != 9)
{
    Display();
    pilihan = int.Parse(Console.ReadLine());
    switch (pilihan)
    {
        case 1:
            garis();
            e.Trigger("||      Tambah Produk      ||");
            garis();
            Console.Write("SKU:\n ");
            string sku = Console.ReadLine();
            if (produk.Any(p => p.SKU == sku))
            {
                garis();
                e.Trigger($"Kode produk {sku} sudah ada");
                Console.ReadLine();
                break;
            }

            Console.Write("Nama:\n ");
            string nama = Console.ReadLine();
            Console.Write("Stock:\n ");
            int stock = int.Parse(Console.ReadLine());
            Console.Write("Harga:\n ");
            int harga = int.Parse(Console.ReadLine());

            produk.Add(new Produk(sku, nama, stock, harga));
            garis();
            e.Trigger($"Produk {sku} berhasil ditambahkan.");
            Console.ReadLine();
            break;
        case 2:
            ShowProduk(e, produk);
            garis();
            e.Trigger("||       Edit Produk       ||");
            garis();
            Console.Write("SKU:\n ");
            string skuBaru = Console.ReadLine();
            Produk produkLama = produk.Find(p => p.SKU == skuBaru);
            if (produkLama == null)
            {
                garis();
                e.Trigger($"Produk dengan SKU {skuBaru} tidaK ditemukan.");
                Console.ReadLine();
                break;
            }
            Console.Write("Nama ({0}):\n ", produkLama.Nama);
            string namaBaru = Console.ReadLine();
            if (!string.IsNullOrEmpty(namaBaru))
            {
                produkLama.Nama = namaBaru;
            }
            Console.Write("Stock ({0}):\n ", produkLama.Stock);
            string stockStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(stockStr))
            {
                int stockBaru = int.Parse(stockStr);
                produkLama.Stock = stockBaru;
            }
            Console.Write("Harga ({0}):\n ", produkLama.Harga);
            string hargaStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(hargaStr))
            {
                int hargaBaru = int.Parse(hargaStr);
                produkLama.Harga = hargaBaru;
            }
            garis();
            e.Trigger($"Produk dengan SKU {skuBaru} berhasil diupdate.");
            Console.ReadLine();
            break;
        case 3:
            garis();
            e.Trigger("||      Daftar Produk      ||");
            garis();
            foreach (var item in produk)
            {
                Console.WriteLine("SKU: {0}\nNama: {1}\nStock: {2}\nHarga: {3}", item.SKU, item.Nama, item.Stock, item.Harga);
                garis();
            }
            Console.ReadLine();
            break;
        case 4:
            ShowProduk(e, produk);
            garis();
            e.Trigger("||      Hapus Produk       ||");
            garis();
            Console.Write("SKU:\n ");
            string skuHapus = Console.ReadLine();
            Produk produkHapus = produk.Find(p => p.SKU == skuHapus);
            if (produkHapus == null)
            {
                garis();
                e.Trigger($"Produk dengan SKU {skuHapus} tidak ditemukan.");
                Console.ReadLine();
                break;
            }
            produk.Remove(produkHapus);
            garis(); ;
            e.Trigger($"Produk dengan SKU {skuHapus} berhasil dihapus.");
            Console.ReadLine();
            break;
        case 5:
            ShowProduk(e, produk);
            garis();
            e.Trigger("||  Masukkan ke Keranjang  ||");
            garis();
            Console.Write("SKU:\n ");
            string skuCart = Console.ReadLine();
            Produk productCart = produk.Find(p => p.SKU == skuCart);
            if (productCart == null)
            {
                garis();
                e.Trigger($"Produk dengan SKU {skuCart} tidak ditemukan.");
                Console.ReadLine();
                break;
            }
            Console.Write("Jumlah:\n ");
            int quantity = int.Parse(Console.ReadLine());

            if (quantity > productCart.Stock)
            {
                garis();
                e.Trigger("Stok tidak mencukupi.");
                Console.ReadLine();
                break;
            }

            Produk existingProductInCart = cart.GetProducts().Find(p => p.SKU == skuCart);

            if (existingProductInCart != null)
            {
                existingProductInCart.Quantity += quantity;
                productCart.Stock -= quantity;
                garis();
                e.Trigger($"{quantity} {productCart.Nama} berhasil ditambahkan ke keranjang.");
                Console.ReadLine();
                break;
            }

            productCart.Quantity = quantity;
            cart.AddProduct(productCart);
            productCart.Stock -= quantity;

            garis();
            e.Trigger($"{quantity} {productCart.Nama} berhasil ditambahkan ke keranjang.");
            Console.ReadLine();
            break;
        case 6:
            IsiKeranjang(e, cart);
            garis();
            e.Trigger("||      Hapus Produk       ||");
            garis();
            Console.Write("SKU:\n ");
            string skuHapusCart = Console.ReadLine();
            Produk produkHapusCart = cart.GetProducts().Find(p => p.SKU == skuHapusCart);
            if (produkHapusCart == null)
            {
                garis();
                e.Trigger($"Produk dengan SKU {skuHapusCart} tidak ditemukan.");
                Console.ReadLine();
                break;
            }
            Console.Write("Jumlah:\n ");
            int quantityRemove = int.Parse(Console.ReadLine());

            if (quantityRemove > produkHapusCart.Quantity)
            {
                garis();
                e.Trigger("Jumlah barang di keranjang tidak mencukupi.");
                Console.ReadLine();
                break;
            }

            if (quantityRemove == produkHapusCart.Quantity)
            {
                cart.RemoveProduct(produkHapusCart);
            }
            else
            {
                produkHapusCart.Quantity -= quantityRemove;
                produkHapusCart.Stock += quantityRemove;
                cart.UpdateProduct(produkHapusCart);
            }

            garis();
            e.Trigger($"{quantityRemove} {produkHapusCart.Nama} berhasil dihapus dari keranjang.");
            Console.ReadLine();
            break;
        case 7:
            garis();
            e.Trigger("||       Keranjang         ||");
            garis();
            foreach (var p in cart.GetProducts())
            {
                Console.WriteLine("SKU: {0}\nNama: {1}\nJumlah: {2}\nHarga Total: {3}", p.SKU, p.Nama, p.Quantity, p.Harga * p.Quantity);
                garis();
            }
            Console.ReadLine();
            break;
        case 8:
            IsiKeranjang(e, cart);
            garis();
            e.Trigger("||       Check Out         ||");
            garis();
            if (cart.GetProducts().Count == 0)
            {
                e.Trigger("Keranjang masih kosong.");
                garis();
                Console.ReadLine();
                break;
            }
            int total = 0;
            foreach (var p in cart.GetProducts())
            {
                total += p.Harga*p.Quantity;
                p.Stock -= p.Quantity;
            }
            Console.WriteLine("Total belanja: {0}", total);
                
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("Pilih aksi:");
                Console.WriteLine("1. Bayar");
                Console.WriteLine("2. Batal");
                garis();
                Console.Write("Pilihan ? : ");
                string input = Console.ReadLine();

            if (input == "1")
            {
                garis();
                e.Trigger("Pembayaran sejumlah " + total + " telah diterima. \nTerima kasih telah berbelanja.");
                cart.Clear();
                Console.ReadLine();
                isValidInput = true;
            }
            else if (input == "2")
            {
                garis();
                e.Trigger("Transaksi dibatalkan. \nSilakan lanjutkan belanja.");
                Console.ReadLine();
                isValidInput = true;
            }
            else
            {
                garis();
                e.Trigger("Pilihan tidak valid. \nSilakan masukkan pilihan yang benar.");
                Console.ReadLine();
            }
            }
            break;
        case 9:
            garis();
            e.Trigger("Terimakasih telah berkunjung.");
            garis();

            break;
        default:
            garis();
            e.Trigger("Pilihan tidak valid.");
            Console.ReadLine();
            break;
    }
}


void garis()
{
    Console.WriteLine("=============================");
}

void ShowProduk(EventProduct e, List<Produk> produk)
{
    garis();
    e.Trigger("||      Daftar Produk      ||");
    garis();
    Console.WriteLine("SKU   \tNama  \tJumlah \tHarga");
    foreach (var item in produk)
    {
        Console.WriteLine($"{item.SKU} \t{item.Nama} \t{item.Stock} \t{item.Harga}");
    }
}

void IsiKeranjang(EventProduct e, Cart cart)
{
    garis();
    e.Trigger("||      Isi Keranjang      ||");
    garis();
    Console.WriteLine("SKU   \tNama  \tJumlah \tHarga");
    foreach (var item in cart.GetProducts())
    {
        Console.WriteLine($"{item.SKU} \t{item.Nama} \t{item.Quantity} \t{item.Harga}");
    }
}
