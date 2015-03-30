using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RedBit.Samples.GZip
{
    public class MainPage : ContentPage
    {
        Button _btnRequest;
        Label _lblLog;
        public MainPage()
        {
            // create the log label
            _lblLog = new Label
            {
                Text = "",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            // create the button to make request
            _btnRequest = new Button
            {
                Text = "Make Request",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _btnRequest.Clicked += (s, e) => MakeRequest(); ;

            // create the content view
            var content =  new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = 
                {
                    _btnRequest,
                    new ScrollView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Content = _lblLog
                    }
                }
            };

            // set the content
            this.Content = content;
        }

        private string _url = "http://www.windowsphone.com/en-us";
        private async void MakeRequest()
        {
            var response = default(HttpResponseMessage);
            var client = CreateHttpClient();
            var request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(_url) };
            try
            {
                // make the request to the url
                var start = DateTime.Now.Ticks;
                response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
                var end = DateTime.Now.Ticks;

                this.Log("request time = {0}ms", TimeSpan.FromTicks(end).Subtract(TimeSpan.FromTicks(start)).TotalMilliseconds);

                // act on response
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    this.Log("Content-Length = {0}", response.Content.Headers.ContentLength);
                }
                else
                {
                    this.Log("could not get content from {0}", _url);
                }
            }
            catch (HttpRequestException e)
            {
                this.Log(e.ToString());
            }
            catch (WebException e)
            {
                this.Log(e.ToString());
            }
            catch (TaskCanceledException e)
            {
                this.Log(e.ToString());
            }
            catch (Exception e)
            {
                this.Log(e.ToString());
            }
            finally
            {
                if (response != null)
                    response.Dispose();
                if(client!=null)
                    client.Dispose();
            }
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient();
        }

        private void Log(string data)
        {
            _lblLog.Text = string.Format("{0}: {1}\r\n{2}", DateTime.Now.ToString("ddd hh:mm:ss:fff"), data, _lblLog.Text);
        }

        private void Log(string data, params object[] items)
        {
            this.Log(string.Format(data, items));
        }

     
    }
}
