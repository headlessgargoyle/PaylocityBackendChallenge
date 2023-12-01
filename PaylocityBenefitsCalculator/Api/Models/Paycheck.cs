namespace Api.Models;

public class Paycheck
{
    public decimal Gross { get; set; }
    public decimal Deductions { get; set; }
    public decimal Net => Gross - Deductions;
}
