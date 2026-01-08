using System.Text.Json;
using System.Text.Json.Serialization;

namespace ColdDream.Api.Services;

public interface IWeChatService
{
    Task<WeChatSessionResponse?> Code2Session(string code);
    Task<WeChatPhoneResponse?> GetPhoneNumber(string code);
}

public class WeChatService : IWeChatService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public WeChatService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<WeChatSessionResponse?> Code2Session(string code)
    {
        var appId = _configuration["WeChat:AppId"];
        var secret = _configuration["WeChat:AppSecret"];
        var url = $"https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={secret}&js_code={code}&grant_type=authorization_code";

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeChatSessionResponse>(content);
        }
        return null;
    }

    public async Task<WeChatPhoneResponse?> GetPhoneNumber(string code)
    {
        // Get Access Token first
        var accessToken = await GetAccessToken();
        if (string.IsNullOrEmpty(accessToken)) return null;

        var url = $"https://api.weixin.qq.com/wxa/business/getuserphonenumber?access_token={accessToken}";
        var payload = new { code = code };
        
        var response = await _httpClient.PostAsJsonAsync(url, payload);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeChatPhoneResponse>(content);
        }
        return null;
    }

    private async Task<string?> GetAccessToken()
    {
        // In production, cache this token!
        var appId = _configuration["WeChat:AppId"];
        var secret = _configuration["WeChat:AppSecret"];
        var url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={secret}";

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WeChatTokenResponse>(content);
            return result?.AccessToken;
        }
        return null;
    }
}

public class WeChatSessionResponse
{
    [JsonPropertyName("openid")]
    public string OpenId { get; set; } = string.Empty;
    
    [JsonPropertyName("session_key")]
    public string SessionKey { get; set; } = string.Empty;
    
    [JsonPropertyName("unionid")]
    public string? UnionId { get; set; }
    
    [JsonPropertyName("errcode")]
    public int ErrCode { get; set; }
    
    [JsonPropertyName("errmsg")]
    public string? ErrMsg { get; set; }
}

public class WeChatPhoneResponse
{
    [JsonPropertyName("errcode")]
    public int ErrCode { get; set; }
    
    [JsonPropertyName("phone_info")]
    public PhoneInfo? PhoneInfo { get; set; }
}

public class PhoneInfo
{
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("purePhoneNumber")]
    public string PurePhoneNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; } = string.Empty;
}

public class WeChatTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}
