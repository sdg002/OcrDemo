using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using OcrDemo.Contracts.entity;
using System.Linq;
using System.Collections.Generic;

namespace Azure
{
    public class ComputerVisions : OcrDemo.Contracts.interfaces.IExtractTextFromImage
    {
        public ComputerVisions()
        {
        }
        public string Url { get; set; }
        public string AppKey { get; set; }

        public TextExtractionResults Extract(byte[] image)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{this.AppKey}");


            // Request parameters

            //If this parameter is set to “true” or is not specified, handwriting recognition is performed. If “false” is specified, printed text recognition is performed by calling OCR operation.

            queryString["handwriting"] = "false";
            //https://uksouth.api.cognitive.microsoft.com/
            //var uri = "https://westus.api.cognitive.microsoft.com/vision/v1.0/recognizeText?" + queryString;
            var uri = "https://uksouth.api.cognitive.microsoft.com/vision/v1.0/recognizeText?" + queryString;
            queryString["handwriting"] = "true";
            //var url =this.Url +"?" + queryString;
            HttpResponseMessage response;

            // Request body
            byte[] byteData = image;

            string json = null;
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.Add("Content-Type", "application/octet-stream");
                //content.Headers.ContentType = new MediaTypeHeaderValue("< your content type, i.e. application/json >");
                response = client.PostAsync(uri, content).Result;
                HttpContent ct = response.Content;
                json = ct.ReadAsStringAsync().Result;
            }

            OcrDemo.Contracts.entity.TextExtractionResults rs = ExtractEntitiesFromJson(json);
            return rs;
        }

        public TextExtractionResults ExtractEntitiesFromJson(string jsonText)
        {
            JObject json = JObject.Parse(jsonText);
            IEnumerable<JToken> words = json.SelectTokens("$..words");
            JToken[] arr = words.ToArray();
            JToken[] arrAllChildren = arr.SelectMany(t => t).ToArray();
            TextExtractionResults rs = new TextExtractionResults();
            rs.Results = arrAllChildren.
                                Select(r => CreateFromJtoken(r)).
                                ToArray();
            return rs;
        }
        TextResult CreateFromJtoken(JToken token)
        {
            TextResult r = new TextResult();
            r.Text = token["text"].Value<string>();
            string coordinates = token["boundingBox"].Value<string>();
            double[] dblCoordinates = 
                            coordinates.Split(',').
                            Select(frag => double.Parse(frag)).
                            ToArray();
            r.X1 = dblCoordinates[0];
            r.Y1 = dblCoordinates[1];
            r.X2 = dblCoordinates[0]+dblCoordinates[2];
            r.Y2 = dblCoordinates[1] + dblCoordinates[3];
            return r;
        }
    }
}
