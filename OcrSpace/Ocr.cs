using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OcrDemo.Contracts.entity;
using System.Linq;
using System.Collections.Generic;

namespace OcrSpace
{
    public class Ocr : OcrDemo.Contracts.interfaces.IExtractTextFromImage
    {
        public string ApiKey { get; set; }

        public TextExtractionResults Extract(byte[] image)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(1, 1, 1);
            string lang = "eng";
            MultipartFormDataContent form = new MultipartFormDataContent();
            //form.Add(new StringContent("helloworld"), "apikey"); //This works ! I am not sure how?
            form.Add(new StringContent(this.ApiKey), "apikey"); //Added api key in form data
            form.Add(new StringContent(lang), "language");
            form.Add(new StringContent("True"), "isOverlayRequired");
            
            form.Add(new ByteArrayContent(image, 0, image.Length), "image", "image.jpg");

            HttpResponseMessage response = httpClient.PostAsync("https://api.ocr.space/Parse/Image", form).Result;

            string strContent = response.Content.ReadAsStringAsync().Result;
            entity.Rootobject ocrResult = JsonConvert.DeserializeObject<entity.Rootobject>(strContent);

            TextExtractionResults rs = new TextExtractionResults();
            entity.Word[] allWords = ocrResult.
                                ParsedResults.
                                SelectMany(r => r.TextOverlay.Lines).
                                SelectMany(ln => ln.Words).
                                ToArray();
            List<TextBlock> lstTextWords = new List<TextBlock>();
            foreach(var word in allWords)
            {
                TextBlock rsTxt = new TextBlock
                {
                    Text = word.WordText,                    
                    X1=word.Left,
                    Y1=word.Top
                };
                rsTxt.X2 = word.Left + word.Width;
                rsTxt.Y2 = word.Top + word.Height;
                lstTextWords.Add(rsTxt);
            }
            rs.Blocks = lstTextWords.ToArray();
            return rs;
        }

    }
}
