namespace MyWebApi.Models
{
    public class GoodsVM
    {
        public string GoodsName { get; set; }
        public double GoodsPrice { get; set; }
    }

    public class Goods : GoodsVM
    {
        public Guid GoodsId { get; set; }
    }
}
