using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using Newtonsoft.Json;
using System.Text;


namespace mvc.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly HttpClient _httpClient;
    public UserController(ILogger<UserController> logger,
    IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5184");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("/User");
        if(response.IsSuccessStatusCode){
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(content);
            return View("Index", users);
        }
        return View(new List<UserViewModel>());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult>Create(UserViewModel user)
    {
        if (ModelState.IsValid)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/User", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }else{
                ModelState.AddModelError(string.Empty,"Error creating User");
            }
        }
        return View(user);
    }

    public async Task<IActionResult>Edit(int id)
    {
        var response = await _httpClient.GetAsync($"/User/{id}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(content);
            return View(user);
        }else{
            return RedirectToAction("Details");
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.GetAsync($"/User/{id}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(content);
            return View(user);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await _httpClient.DeleteAsync($"/User/{id}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return NotFound();
        }
    }



    [HttpPost]
    public async Task<IActionResult>Edit (int id, UserViewModel user)
    {
        if (ModelState.IsValid)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"User/{id}",content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index",new{id});
            }else{
                ModelState.AddModelError(string.Empty,"Error to update");
            }
        }
        return View(user);
    }

}
