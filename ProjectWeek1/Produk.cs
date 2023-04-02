public class Produk
{
    public string sku;
    public string nama;
    public int stock;
    public int harga;
    public int quantity;

    public string SKU
    {
        get { return sku; }
        set { sku = value; }
    }

    public string Nama
    {
        get { return nama; }
        set { nama = value; }
    }

    public int Stock
    {
        get { return stock; }
        set { stock = value; }
    }

    public int Harga
    {
        get { return harga; }
        set { harga = value; }
    }

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }            

    public Produk(string sku, string nama, int stock, int harga)
    {
        SKU = sku;
        Nama = nama;
        Stock = stock;
        Harga = harga;
    }
}