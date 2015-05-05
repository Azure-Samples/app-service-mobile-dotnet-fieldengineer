using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;


namespace FieldEngineerLite.Helpers
{
    public class MobileServiceSQLiteStoreWithLogging : MobileServiceSQLiteStore
    {
        private bool logResults;
        private bool logParameters;
    
        public MobileServiceSQLiteStoreWithLogging(string fileName, bool logResults = false, bool logParameters = false) 
            : base(fileName) 
        {
            this.logResults = logResults;
            this.logParameters = logParameters;
        }

        protected override IList<Newtonsoft.Json.Linq.JObject> ExecuteQuery(string tableName, string sql, IDictionary<string, object> parameters)
        {
            Console.WriteLine (sql);   

            if(logParameters)
                PrintDictionary (parameters);

            var result = base.ExecuteQuery(tableName, sql, parameters);

            if (logResults && result != null) 
            {
                foreach (var token in result)
                    Console.WriteLine (token);
            }

            return result;
        }

        protected override void ExecuteNonQuery(string sql, IDictionary<string, object> parameters)
        {
            Console.WriteLine (sql);

            if(logParameters)
                PrintDictionary (parameters);

            base.ExecuteNonQuery(sql, parameters);
        }

        private void PrintDictionary(IDictionary<string,object> dictionary)
        {
            if (dictionary == null)
                return;

            foreach (var pair in dictionary)
                Console.WriteLine ("{0}:{1}", pair.Key, pair.Value);
        }
    }

    public class LoggingHandler : DelegatingHandler
    {
        private bool logRequestResponseBody;

        public LoggingHandler(bool logRequestResponseBody = false)
        {
            this.logRequestResponseBody = logRequestResponseBody;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine("Request: {0} {1}", request.Method, request.RequestUri.ToString());

            if (logRequestResponseBody && request.Content != null) 
            {
                var requestContent = await request.Content.ReadAsStringAsync ();
                Console.WriteLine (requestContent);
            }

            var response = await base.SendAsync(request, cancellationToken);

            Console.WriteLine ("Response: {0}", response.StatusCode);

            if (logRequestResponseBody) 
            {
                var responseContent = await response.Content.ReadAsStringAsync ();
                Console.WriteLine (responseContent);
            }

            return response;
        }
    } 
}
