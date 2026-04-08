using System.ComponentModel.DataAnnotations;

namespace ASPNetDashboard.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Transaction description is required.")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "Description must be between 3 and 200 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, 10_000_000,
            ErrorMessage = "Amount must be greater than ₱0.00.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount (₱)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please select a transaction type.")]
        [Display(Name = "Transaction Type")]
        public string Type { get; set; } = string.Empty; // Credit | Debit

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
        public string Category { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
