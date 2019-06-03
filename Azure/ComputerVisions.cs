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

        /// <summary>
        /// Making this method virtual so that it can be mocked
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public TextExtractionResults Extract(byte[] image)
        {
            string json = GetJsonFromAzure(image);
            OcrDemo.Contracts.entity.TextExtractionResults rs = ExtractEntitiesFromJson(json);
            return rs;
        }
        /// <summary>
        /// This is the one and only method which goes out to the internet
        /// Markthing method virtual so that it can be mocked for better testability
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public virtual string GetJsonFromAzure(byte[] image)
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
            return json;
        }
        public TextExtractionResults ExtractEntitiesFromJson(string jsonText)
        {

            //TextExtractionResults rs = new TextExtractionResults();
            //JObject json = JObject.Parse(jsonText);
            //IEnumerable<JToken> words = json.SelectTokens("$..words");
            //JToken[] arr = words.ToArray();
            //JToken[] arrAllChildren = arr.SelectMany(t => t).ToArray();
            //rs.Blocks = arrAllChildren.
            //                    Select(r => CreateFromJtoken(r)).
            //                    ToArray();

            TextExtractionResults rs = new TextExtractionResults();
            JObject json = JObject.Parse(jsonText);
            List<Paragraph> lstParas = new List<Paragraph>();
            IEnumerable<JToken> sections = json.
                            SelectTokens("$..regions").
                            SelectMany(l => l).ToArray();
            foreach(JToken tSection in sections)
            {
                string boxCoordinates = tSection["boundingBox"].Value<string>();
                float[] dblCoordinates =
                                boxCoordinates.Split(',').
                                Select(frag => float.Parse(frag)).
                                ToArray();
                Paragraph paraNew = new Paragraph();
                var rect = new System.Drawing.RectangleF(
                                                dblCoordinates[0], dblCoordinates[1],
                                                dblCoordinates[2], dblCoordinates[3]);
                paraNew.Rectangle = rect;
                lstParas.Add(paraNew);
            }
            rs.Paragraphs = lstParas.ToArray();
            ///
            /// Use the sentences approach - May ,2019
            ///
            IEnumerable<JToken> lines = json.
                            SelectTokens("$..lines").
                            SelectMany(l=>l).ToArray();
            List<TextBlock> lstAllBlocks = new List<TextBlock>();
            List<Sentence> lstAllSentences = new List<Sentence>();
            foreach(JToken line in lines)
            {
                string boxCoordinates= line["boundingBox"].Value<string>();
                float[] dblCoordinates =
                                boxCoordinates.Split(',').
                                Select(frag => float.Parse(frag)).
                                ToArray();
                Sentence sentNew = new Sentence();
                sentNew.Rectangle = new System.Drawing.RectangleF(
                                                dblCoordinates[0], dblCoordinates[1], 
                                                dblCoordinates[2], dblCoordinates[3]);
                JToken[] arrAllWords = line.
                                        SelectTokens("$..words").
                                        SelectMany(t=>t).ToArray();
                TextBlock[] blockFromWords = arrAllWords.
                            Select(r => CreateFromJtoken(r)).
                            ToArray();
                sentNew.Blocks = blockFromWords;
                lstAllSentences.Add(sentNew);
                lstAllBlocks.AddRange(blockFromWords);
            }
            rs.Sentences = lstAllSentences.ToArray();
            rs.Blocks = lstAllBlocks.ToArray();
            return rs;
        }
        TextBlock CreateFromJtoken(JToken token)
        {
            TextBlock r = new TextBlock();
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
