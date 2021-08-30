namespace BuildingBlocks.Web.Startup.Options
{
    public class SwaggerOpenApiOptions
    {
        public const string Section = "Swagger";

        public bool UseSwagger { get; set; }

        public bool UseAuth { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}