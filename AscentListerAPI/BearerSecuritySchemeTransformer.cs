using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace AscentListerAPI;

internal sealed class BearerSecuritySchemeTransformer(Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            document.Components ??= new OpenApiComponents();

            if (document.Components.SecuritySchemes == null)
                document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();
            
            var securitySchemeId = "Bearer";

            document.Components.SecuritySchemes.Add(securitySchemeId, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                In = ParameterLocation.Header,
                BearerFormat = "Json Web Token"
            });

            document.Security ??= new List<OpenApiSecurityRequirement>();

            document.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(securitySchemeId)] = new List<string>()
            });
        }
    }
}