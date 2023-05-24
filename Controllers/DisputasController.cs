using System.Net.Http.Headers;
using RpgMvc.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RpgMvc.Controllers
{
    public class DisputasController : Controller
    {
    public string uriBase = "http://gustavoleite.somee.com/RpgApi/Disputas/";
    
    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        try{
            HttpClient httpClient = new HttpClient();
            string token = HttpContext.Session.GetString("SessionTokenUsuario");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            
            string uriBuscaPersonagens = "http://gustavoleite.somee.com/RpgApi/Personagens/GetAll";
            HttpResponseMessage response = await httpClient.GetAsync(uriBuscaPersonagens);
            string serialized = await response.Content.ReadAsStringAsync();

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<PersonagemViewModel> listaPersonagens = await Task.Run(() =>
                JsonConvert.DeserializeObject<List<PersonagemViewModel>>(serialized));  
                
                ViewBag.ListaAtacantes = listaPersonagens;
                ViewBag.ListaOponentes = listaPersonagens;
                return  View();
            }
            else
                throw new System.Exception(serialized);


        }
        catch(System.Exception ex)
        {
            TempData["MensagemErro"] = ex.Message;
            return RedirectToAction("Index","Usuarios");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> IndexAsync(DisputaViewModel disputa)
    {
        try
        {
            HttpClient httpClient = new HttpClient();
            string uriComplementar = "Arma";

            var content = new StringContent(JsonConvert.SerializeObject(disputa));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);
            string serialized = await response.Content.ReadAsStringAsync();


            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                disputa = await Task.Run(() => JsonConvert.DeserializeObject<DisputaViewModel>(serialized));
                TempData["Mensagem"]= disputa.Narracao;
                return RedirectToAction("Index", "Personagens");
            }
            else
                throw new System.Exception(serialized);

        }
        catch(System.Exception ex)
        {
           TempData["MensagemErro"] = ex.Message;
            return RedirectToAction("Index");
        }
    }
    


    [HttpGet]
    public async Task<IActionResult> IndexHabilidadesAsync(int? id)
    {
        try
        { 
             HttpClient httpClient = new HttpClient();
            string token = HttpContext.Session.GetString("SessionTokenUsuario");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            
            string uriBuscaPersonagens = "http://gustavoleite.somee.com/RpgApi/Personagens/GetAll";
            HttpResponseMessage response = await httpClient.GetAsync(uriBuscaPersonagens);
            string serialized = await response.Content.ReadAsStringAsync();

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<PersonagemViewModel> listaPersonagens = await Task.Run(() =>
                JsonConvert.DeserializeObject<List<PersonagemViewModel>>(serialized));  
                
                ViewBag.ListaAtacantes = listaPersonagens;
                ViewBag.ListaOponentes = listaPersonagens;
            }
            else
                throw new System.Exception(serialized);
            

            string uriBuscaHabilidades = "http://gustavoleite.somee.com/RpgApi/PersonagensHabilidades/GetHabilidades";
            response = await httpClient.GetAsync(uriBuscaHabilidades);
            serialized = await response.Content.ReadAsStringAsync();
            if( response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<HabilidadeViewModel> listaHabilidades = await Task.Run(() => JsonConvert.DeserializeObject<List<HabilidadeViewModel>>(serialized));
                ViewBag.ListaHabilidades = listaHabilidades;
            }
            else
                throw new System.Exception(serialized);
            return View("IndexHabilidades");
        }
        catch(System.Exception ex)
        {
            TempData["MensagemErro"] = ex.Message;
            return RedirectToAction("Index");
        }

    }
    
    [HttpPost]
    public async Task<IActionResult> IndexHabilidadesAsync(DisputaViewModel disputa)
    {
        try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "HabilidadeViewModel";
                var content = new StringContent(JsonConvert.SerializeObject(disputa));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);
                string serialized = await response.Content.ReadAsStringAsync();
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    disputa = await Task.Run(() => JsonConvert.DeserializeObject<DisputaViewModel>(serialized));
                    TempData["Mensagem"] = disputa.Narracao;
                    return RedirectToAction("Index", "Personagens");
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MenssagemErro"] = ex.Message;
                  return RedirectToAction("Index");
            }
               
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}