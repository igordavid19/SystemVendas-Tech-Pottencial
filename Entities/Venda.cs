namespace tech_test_payment_api.Entities
{
  public class Venda
    {
      public int Id { get; set; }
      public DateTime Data { get; set; }
      public int VendedorId { get; set; }
      public string Status {get; set;}
      public string StatusDisponíveis { get; set; }
      public string Itens { get; set; }
      

    }
}