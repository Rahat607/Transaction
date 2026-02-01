namespace Transaction.Models
{
    public class TR
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // Income or Expense
        public string Category { get; set; }
        public DateTime Date { get; set; }
    }
}