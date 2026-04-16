namespace SalesManagementAPI.Core.DTOs;

public class SalesReportDto
{
    public decimal TotalSalesAmount { get; set; }

    public string? TopCustomerName { get; set; } 
    public int TopCustomerOrderCount { get; set; }

    public string? TopProductName { get; set; } 
    public int TopProductQuantity { get; set; }
}