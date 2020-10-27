using System;
using System.Collections.Generic;
using Nest;

namespace nest_test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = InitializeClient();

            var aliases = FetchAliases(client);
            var transactions = FetchTransactions(client, aliases, SortOrder.Descending);
        }

        private static ElasticClient InitializeClient()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);

            return client;
        }
        private static IReadOnlyCollection<Transaction> FetchTransactions(ElasticClient client, IReadOnlyCollection<AccountAlias> aliases, SortOrder sortOrder = SortOrder.Descending)
        {
            var index = Indices.Parse("transactions");

            var request = new SearchRequest(index)
            {
                From = 0,
                Size = 2,
                Query = new MatchAllQuery()
            };

            var @params = new Dictionary<string, object>();
            var scores = new Dictionary<string, string>();
            foreach(var item in aliases)
            {
                scores.Add(item.Iban, item.Alias);
            }
            @params.Add("scores", scores);

            var script = @"if(params.scores.containsKey(doc['iban_to.keyword'].value)) { return params.scores[doc['iban_to.keyword'].value]; } return 'zzzzzz';";

            var sort = new List<ISort>
            {
                new ScriptSort
                {
                    Type = "string",
                    Order = sortOrder,
                    Script = new InlineScript(script)
                    {
                        Lang = "painless",
                        Params = @params
                    }
                }
            };
            request.Sort = sort;

            var response = client.Search<Transaction>(request);

            return response.Documents;
        }

        private static IReadOnlyCollection<AccountAlias> FetchAliases(ElasticClient client)
        {
            var index = Indices.Parse("account-aliases");

            var request = new SearchRequest(index)
            {
                Query = new MatchAllQuery()
            };

            var response = client.Search<AccountAlias>(request);

            return response.Documents;
        }
    }
}