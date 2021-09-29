using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API_LAB2.Models;
using Library_LAB2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_LAB2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompressionsController : ControllerBase
    {
        private IHostingEnvironment _env;

        public CompressionsController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IEnumerable<CompressionInfo> Get()
        {
            var path = _env.ContentRootPath;
            path = Path.Combine(path, "Files");
            string pathInfo = Path.Combine(path, "Info");

            string pathHistorial = Path.Combine(pathInfo, "Historial.txt");
            if (!System.IO.File.Exists(pathHistorial))
            {
                System.IO.File.Create(pathHistorial);
                return null;
            }
            string cadenaHistorial = System.IO.File.ReadAllText(pathHistorial);

            if (cadenaHistorial.Length == 0) return null;

            string[] cadenas = cadenaHistorial.Split('/');
            List<CompressionInfo> InfoList = new List<CompressionInfo>();

            foreach (var item in cadenas)
            {
                CompressionInfo compInfo = new CompressionInfo();
                string[] valores = item.Split(';');

                compInfo.originalName = valores[0];
                compInfo.compressedFilePath = valores[1];
                compInfo.compressionRatio = Convert.ToDouble(valores[2]);
                compInfo.compressionFactor = Convert.ToDouble(valores[3]);
                compInfo.reductionPercentage = Convert.ToDouble(valores[4]);

                InfoList.Add(compInfo);
            }

            return InfoList;
        }
    }
}