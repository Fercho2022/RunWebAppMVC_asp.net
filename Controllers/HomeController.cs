using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunWebAppGroup.Helpers;
using RunWebAppGroup.Interfaces;
using RunWebAppGroup.Models;
using RunWebAppGroup.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;

namespace RunWebAppGroup.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IClubRepository _clubRepository;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger, IClubRepository clubRepository)
        {
            _logger = logger;
            _clubRepository = clubRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPinfo();
            var homeViewModel = new HomeViewModel();
            //try
            //{
            // Se define la URL de la API.
            //    string url = "https://ipinfo.io?817218db6bac05";

            //Se usa WebClient para descargar los datos JSON de la API.
            //    var info= new WebClient().DownloadString(url);

            //Se deserializan los datos JSON en un objeto IPinfo usando JsonConvert.
            //    ipInfo=JsonConvert.DeserializeObject<IPinfo>(info);

            //Se convierte el código del país a su nombre en inglés utilizando RegionInfo.
            //    RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
            //    ipInfo.Country = myRI1.EnglishName;



            //Se asignan la ciudad y el estado obtenidos a las propiedades del HomeViewModel.
            //    homeViewModel.City = ipInfo.City;
            //    homeViewModel.State = ipInfo.Region;


            //Si la ciudad no es null, se recupera una lista de clubes desde el
            //repositorio _clubRepository basado en la ciudad y se asigna a la
            //propiedad Clubs del HomeViewModel.
            //    if (homeViewModel.City != null)
            //    {

            //        homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
            //    }

            //Si la ciudad es null, se asigna null a la propiedad Clubs.
            //    else
            //    {
            //        homeViewModel.Clubs = null;
            //    }
            //    return View(homeViewModel);
            //}
            //catch (Exception ex)
            //{
            //    homeViewModel.Clubs = null;

            //}

            //return View(homeViewModel);
            try
            {
                string url = "https://ipinfo.io?token=817218db6bac05"; // Usa el parámetro token en la URL


                //Inyección de Dependencias: Utiliza IHttpClientFactory para crear
                //instancias de HttpClient. Esto permite gestionar mejor la vida útil
                //de los HttpClient y reutilizarlos, evitando problemas de agotamiento
                //de sockets.

                //CreateClient(): Crea una nueva instancia de HttpClient utilizando
                //HttpClientFactory.

                var client = _httpClientFactory.CreateClient();

                //GetAsync(url): Realiza una solicitud GET asíncrona a la URL especificada.
                HttpResponseMessage response = await client.GetAsync(url);


                //IsSuccessStatusCode: Verifica si la respuesta HTTP fue exitosa
                //(código de estado 2xx).
                if (response.IsSuccessStatusCode)
                {

                    //ReadAsStringAsync(): Lee el contenido de la respuesta como una cadena
                    //JSON.
                    string info = await response.Content.ReadAsStringAsync();

                    //Convierte la cadena JSON en un objeto IPinfo.
                    ipInfo = JsonConvert.DeserializeObject<IPinfo>(info);

                    //RegionInfo: Se usa para obtener el nombre del país en inglés
                    //a partir del código del país.
                    RegionInfo myRI1 = new RegionInfo(ipInfo.Country);

                    //Asigna el nombre del país en inglés a la propiedad Country de ipInfo.
                    ipInfo.Country = myRI1.EnglishName;

                    //Asigna la ciudad obtenida al modelo de vista.
                    homeViewModel.City = ipInfo.City;

                    //Asigna el estado o región obtenida al modelo de vista.
                    homeViewModel.State = ipInfo.Region;


                    //Si homeViewModel.City no es null, se obtiene una lista de
                    //clubes basada en la ciudad utilizando _clubRepository.
                    if (homeViewModel.City != null)
                    {
                        homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
                    }
                    else
                    {
                        homeViewModel.Clubs = null;
                    }
                }
                else
                {
                    // Manejo de error en caso de respuesta no exitosa
                    homeViewModel.Clubs = null;
                }

                return View(homeViewModel);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                homeViewModel.Clubs = null;
                return View(homeViewModel);
            
        }
    }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
