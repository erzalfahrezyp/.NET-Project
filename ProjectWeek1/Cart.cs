public class Cart
{
    private List<Produk> _products = new List<Produk>();

    public void AddProduct(Produk product)
    {
        _products.Add(product);
    }

    public void RemoveProduct(Produk product)
    {
        _products.Remove(product);
        product.Stock += product.Quantity;
        product.Quantity = 0;
    }

    public void UpdateProduct(Produk product)
    {
        
    }

    public List<Produk> GetProducts()
    {
        return _products;
    }

    public void Clear()
    {
        _products.Clear();
    }
    
}