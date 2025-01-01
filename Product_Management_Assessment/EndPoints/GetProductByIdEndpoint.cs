using FastEndpoints;
using Product_Management_Assessment.DTO;
namespace Product_Management_Assessment.EndPoints
{
    public class GetProductByIdEndpoint : Endpoint<GetByIdProductRequest, ProductResponse>
    {
        private readonly AppDbContext _context;

        public GetProductByIdEndpoint(AppDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("/GetProductById/{id:int}");
            AllowAnonymous(); 
            //Options(o => o.WithTags("Products"));

        }

        public override async Task HandleAsync(GetByIdProductRequest req, CancellationToken ct)
        {
            try
            {
                // Fetch product using the ID from the route parameter
                var product = await _context.Products.FindAsync(req.Id);

                if (product == null)
                {
                    // Send a response indicating the product was not found
                    await SendAsync(new ProductResponse
                    {
                        Id = 0, // Use 0 or some default value to indicate "not found"
                        Name = "Not Found",
                        Description = "No product found with the given ID.",
                        Price = 0
                    }, statusCode: 404);
                    return;
                }

                // Return the product response
                await SendAsync(new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CreatedDate = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                // Log the exception (replace Console with proper logging in production)
                Console.Error.WriteLine($"Error occurred while fetching product with ID {req.Id}: {ex.Message}");

                // Send a generic error response
                await SendAsync(new ProductResponse
                {
                    Id = 0,
                    Name = "Error",
                    Description = "An unexpected error occurred.",
                    Price = 0
                }, statusCode: 500);
            }
        }

    }
}