using System;
using System.ComponentModel.DataAnnotations;

public class Altin
{
    [Key]
    public int altin_id { get; set; }

    public decimal gram_altin_miktari { get; set; }

    public decimal alis_fiyati { get; set; }

    public decimal satis_fiyati { get; set; }

    public DateTime tarih { get; set; }

    public DateTime createdAt { get; set; }

    public DateTime updatedAt { get; set; }
}
