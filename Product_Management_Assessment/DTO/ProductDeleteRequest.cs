using Microsoft.AspNetCore.Mvc;

namespace Product_Management_Assessment.DTO
{
    public class ProductDeleteRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; } 
    }
}
