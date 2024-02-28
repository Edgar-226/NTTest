using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTest.Models
{
    public class Company
    {
        [Key]
        public string company_id { get; set; }
        public string company_name { get; set; }
    }

    public class Charges
    {

        [Key]
        public string id { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal amount { get; set; }

        public string status { get; set; }

        public DateTime created_at { get; set; }

        public DateTime? paid_at { get; set; }

        [ForeignKey("company_Id")]
        public string company_id { get; set; }
    }

}
