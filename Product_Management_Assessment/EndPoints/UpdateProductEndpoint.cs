using FastEndpoints;
using Product_Management_Assessment.DTO;
using System.Net;

namespace Product_Management_Assessment.EndPoints
{
    public class UpdateProductEndpoint : Endpoint<ProductResponse, ProductResponse>
    {
        private readonly AppDbContext _context; 
        private readonly ILogger<UpdateProductEndpoint> _logger;
        public UpdateProductEndpoint(AppDbContext context, ILogger<UpdateProductEndpoint> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override void Configure()
        {
            Put("/UpdateProducts");
            AllowAnonymous();
            //Options(o => o.WithTags("Products"));
        }

        public override async Task HandleAsync(ProductResponse req, CancellationToken ct)
        {

            try
            {
                // Fetch the product using the ID provided in the body
                var product = await _context.Products.FindAsync(req.Id);
                if (product is null)
                {
                    await SendNotFoundAsync();
                    return;
                }

                // Update product properties
                product.Name = req.Name;
                product.Description = req.Description;
                product.Price = req.Price;

                // Save changes to the database
                await _context.SaveChangesAsync(ct);

                // Send the updated product as the response
                await SendAsync(new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CreatedDate = DateTime.Now
                });
            }
            catch (Exception ex) {
                _logger.LogError($"An error occurred while deleting the product: {ex.Message}");
                var customerrorResponse = new CustomErrorResponse
                {
                    Error = "An error occurred while processing your request.",
                    Exception = ex.Message
                };
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await HttpContext.Response.WriteAsync(
                    $"{{\"error\":\"An error occurred while processing your request.\",\"exception\":\"{ex.Message}\"}}",
                    ct
                    );

            }
           
        }
    }
}