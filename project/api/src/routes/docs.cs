using Routers;

public static class DocsRouter {

    public static void Register(WebApplication app) {
        
        var api = app.MapGroup("/docs");

        api.MapGet("", () => Results.Redirect("/docs/redoc"));

        api.MapGet("/docs.json", async context => {

            var filePath = "./src/docs/docs.json";
            if (!File.Exists(filePath))
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Swagger file not found.");
                return;
            }

            context.Response.ContentType = "application/json";
            await context.Response.SendFileAsync(filePath);
        });

        api.MapGet("/redoc", async context => {

            const string html = """
                <!DOCTYPE html>
                <html>
                <head>
                <title>Documentation</title>
                <!--style>
                    .sidebar-item.group > .menu-item {
                    border-bottom: 2px solid #007bff;
                    padding-bottom: 8px;
                    margin-bottom: 12px;
                    font-weight: 700;
                    font-size: 18px;
                    color: #004085;
                    }

                    .sidebar-item.group {
                    margin-bottom: 24px;
                    padding-bottom: 12px;
                    border-bottom: 1px solid #ddd;
                    }
                </style-->
                </head>
                <body>
                <div id="redoc-container"></div>

                <script src="https://cdn.jsdelivr.net/npm/redoc@next/bundles/redoc.standalone.js"></script>
                <script>
                    Redoc.init('./docs.json', {
                    theme: {
                        colors: {
                        http: {
                            get: '#28a745',
                            delete: '#dc3545',
                            post: '#ffc107', 
                            put: '#007bff' 
                        }
                        }
                    }
                    }, document.getElementById('redoc-container'))
                </script>
                </body>
                </html>
                """;

            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(html);

        });


        api.MapGet("/swagger", async context => {

            const string html = """
                <!DOCTYPE html>
                <html lang="en">
                <head>
                <meta charset="UTF-8">
                <title>Documentation (Swagger)</title>
                <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/swagger-ui-dist/swagger-ui.css" />
                </head>
                <body>
                <div id="swagger-ui"></div>
                <script src="https://cdn.jsdelivr.net/npm/swagger-ui-dist/swagger-ui-bundle.js"></script>
                <script>
                    window.onload = () => {
                    SwaggerUIBundle({
                        url: './docs.json',
                        dom_id: '#swagger-ui',
                        presets: [
                        SwaggerUIBundle.presets.apis,
                        SwaggerUIBundle.SwaggerUIStandalonePreset
                        ],
                        layout: "BaseLayout"
                    });
                    };
                </script>
                </body>
                </html>
                """;

            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(html);

        });

    }

}
