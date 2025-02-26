using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

namespace NovaConnect.Runtimes.Models.Net
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public T Data { get; set; }
    }

    public class Session
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTime CreateTime { get; set; }
        public string ClientIp { get; set; }
        public string ClientPort { get; set; }
        public List<Message> Historys { get; set; }
    }

    public class ClientInfo
    {
        public string ClientID { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string SystemVer { get; set; }
        public DateTime ConnectTime { get; set; }
        public int ProcessID { get; set; }
        public string RunPath { get; set; }
        public string FileName { get; set; }
    }

    public class Message
    {
        public int State { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }
        public string Result { get; set; }
        public DateTime Time { get; set; }
    }

    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public ApiClient(string baseApiUrl)
        {
            Debug.WriteLine(baseApiUrl);
            _httpClient = new HttpClient();
            _baseApiUrl = baseApiUrl;
        }

        public ApiResponse<string> BuildToken(string key)
        {
            try
            {
                var response = _httpClient.GetAsync($"{_baseApiUrl}/auth/build?key={key}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<ApiResponse<string>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error building token: " + ex.Message);
                return null;
            }
        }

        public ApiResponse<bool> AuthToken(string token)
        {
            try
            {
                var response = _httpClient.GetAsync($"{_baseApiUrl}/auth?token={token}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<ApiResponse<bool>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error authenticating token: " + ex.Message);
                return null;
            }
        }

        public ApiResponse<List<Session>> GetAllSessions(string token)
        {
            try
            {
                var response = _httpClient.GetAsync($"{_baseApiUrl}/connect/sessions?token={token}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<ApiResponse<List<Session>>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching sessions: " + ex.Message);
                return null;
            }
        }

        public ApiResponse<List<ClientInfo>> GetAllClients(string token)
        {
            try
            {
                var url = $"{_baseApiUrl}/connect/clients?token={token}";
                Debug.WriteLine(url);
                var response = _httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<ApiResponse<List<ClientInfo>>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching clients: " + ex.Message);
                return null;
            }
        }

        public ApiResponse<string> ConnectClient(string token, string ip, int port)
        {
            try
            {
                var response = _httpClient.GetAsync($"{_baseApiUrl}/connect/connect?token={token}&ip={ip}&port={port}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<ApiResponse<string>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error connecting client: " + ex.Message);
                return null;
            }
        }

        public ApiResponse<string> DisconnectSession(string token, string id)
        {
            try
            {
                var response = _httpClient.GetAsync($"{_baseApiUrl}/connect/disconnect?token={token}&id={id}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<ApiResponse<string>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error disconnecting session: " + ex.Message);
                return null;
            }
        }

        public ApiResponse<string> AddMessage(string token, string id, string text)
        {
            try
            {
                var content = new StringContent($"token={token}&id={id}&text={text}", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = _httpClient.PostAsync($"{_baseApiUrl}/connect/addmsg", content).Result;
                response.EnsureSuccessStatusCode();

                return response.Content.ReadFromJsonAsync<ApiResponse<string>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding message: " + ex.Message);
                return null;
            }
        }

        public ApiResponse<Session> GetSession(string token, string id)
        {
            try
            {
                var response = _httpClient.GetAsync($"{_baseApiUrl}/connect/getsession?token={token}&id={id}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<ApiResponse<Session>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching session: " + ex.Message);
                return null;
            }
        }

        public ApiResponse<ClientInfo> GetClient(string token, string ip, int port)
        {
            try
            {
                var response = _httpClient.GetAsync($"{_baseApiUrl}/connect/getclient?token={token}&ip={ip}&port={port}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<ApiResponse<ClientInfo>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching client: " + ex.Message);
                return null;
            }
        }

        public ApiResponse<Message> GetWork(string token, string id, string code)
        {
            try
            {
                var response = _httpClient.GetAsync($"{_baseApiUrl}/connect/getwork?token={token}&id={id}&code={code}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<ApiResponse<Message>>().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching work: " + ex.Message);
                return null;
            }
        }
    }

    public class AuthModel
    {
        public static string Token() => Build("2077576874888" + DateTime.Now.ToString("yyyyMMddHH"));
        public static string Build(string Key)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(Key + DateTime.Now.ToString("yyyyMMddHH"));
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}