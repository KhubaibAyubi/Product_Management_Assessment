using Microsoft.AspNetCore.Mvc;




namespace Product_Management_Assessment.DTO
{
    public class GetByIdProductRequest
    {
        //[QueryParam]
        //[FromRoute]
        //[FromRoute(Name = "id")]
        //[SwaggerParameter(Description = "The ID of the product to retrieve")]
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
    