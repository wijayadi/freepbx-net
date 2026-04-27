using System.Text.Json;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

namespace Sengsara.Freepbx.Tools.SchemaFetcher;

/// <summary>
/// Tool to fetch and save the GraphQL schema from FreePBX
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("FreePBX GraphQL Schema Fetcher");
        Console.WriteLine("==============================\n");

        // Get configuration
        var endpoint = args.Length > 0 ? args[0] : Environment.GetEnvironmentVariable("FREEPBX_ENDPOINT");
        var apiKey = Environment.GetEnvironmentVariable("FREEPBX_API_KEY");
        var outputPath = args.Length > 1 ? args[1] : "schema.graphql";

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            Console.Error.WriteLine("Error: FreePBX endpoint is required");
            Console.Error.WriteLine("Usage: SchemaFetcher <endpoint> [output-path]");
            Console.Error.WriteLine("Or set FREEPBX_ENDPOINT environment variable");
            return;
        }

        Console.WriteLine($"Fetching schema from: {endpoint}");

        try
        {
            var serializer = new SystemTextJsonSerializer(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            using var client = new GraphQLHttpClient(endpoint, serializer);

            // Add authentication if provided
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                client.HttpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            }

            // Introspection query
            var request = new GraphQLRequest
            {
                Query = @"
                    query IntrospectionQuery {
                        __schema {
                            types {
                                kind
                                name
                                description
                                fields {
                                    name
                                    description
                                    args {
                                        name
                                        description
                                        type {
                                            kind
                                            name
                                            ofType {
                                                kind
                                                name
                                                ofType {
                                                    kind
                                                    name
                                                    ofType {
                                                        kind
                                                        name
                                                    }
                                                }
                                            }
                                        }
                                        defaultValue
                                    }
                                    type {
                                        kind
                                        name
                                        ofType {
                                            kind
                                            name
                                            ofType {
                                                kind
                                                name
                                                ofType {
                                                    kind
                                                    name
                                                }
                                            }
                                        }
                                    }
                                }
                                inputFields {
                                    name
                                    description
                                    type {
                                        kind
                                        name
                                        ofType {
                                            kind
                                            name
                                        }
                                    }
                                }
                                interfaces {
                                    kind
                                    name
                                }
                                enumValues {
                                    name
                                    description
                                }
                                possibleTypes {
                                    kind
                                    name
                                }
                            }
                            queryType { name }
                            mutationType { name }
                            subscriptionType { name }
                            directives {
                                name
                                description
                                locations
                                args {
                                    name
                                    description
                                    type {
                                        kind
                                        name
                                    }
                                    defaultValue
                                }
                            }
                        }
                    }"
            };

            var response = await client.SendQueryAsync<JsonElement>(request);

            if (response.Errors?.Any() == true)
            {
                Console.Error.WriteLine("GraphQL Errors:");
                foreach (var error in response.Errors)
                {
                    Console.Error.WriteLine($"  - {error.Message}");
                }
                return;
            }

            // Save the full introspection result
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var schemaJson = JsonSerializer.Serialize(response.Data, jsonOptions);
            await File.WriteAllTextAsync(outputPath + ".json", schemaJson);

            // Generate simplified SDL schema
            var schema = response.Data.GetProperty("__schema");
            var sdl = GenerateSDL(schema);

            await File.WriteAllTextAsync(outputPath, sdl);

            Console.WriteLine($"\nSchema saved to:");
            Console.WriteLine($"  - SDL: {outputPath}");
            Console.WriteLine($"  - JSON: {outputPath}.json");
            Console.WriteLine("\nDone!");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
        }
    }

    private static string GenerateSDL(JsonElement schema)
    {
        var lines = new List<string> { "schema {", "  query: Query", "  mutation: Mutation", "}", "" };

        // Get query type
        var queryType = schema.GetProperty("queryType");
        lines.Add($"type Query {queryType.GetProperty("name").GetString()?.Replace("Query", "") ?? ""} {GetBraceBlock()}");
        lines.Add("");

        // This is a simplified SDL generator
        // A full implementation would traverse all types

        return string.Join(Environment.NewLine, lines);
    }

    private static string GetBraceBlock() => "{ }";
}