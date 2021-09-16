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
    public class CompressController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            // Se crean los directorios y archivos que servirán de apoyo para la compresión y descompresión
            var path = _env.ContentRootPath;
            path = Path.Combine(path, "Files");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string pathInfo = Path.Combine(path, "Info");
            if (!Directory.Exists(pathInfo))
            {
                Directory.CreateDirectory(pathInfo);
            }
            string pathHistorial = Path.Combine(pathInfo, "Historial.txt");
            using var saver = new FileStream(pathHistorial, FileMode.OpenOrCreate);
            saver.Close();

            // Mensaje de bienvenida
            string mensaje = "Laboratorio 2 - Inicio Exitoso...\n * Comprimir un archivo - Suba un archivo de texto y coloque el nombre que desee en el archivo nuevo en la ruta '/api/compress/{name}' \n * Descomprimir un archivo - Suba un archivo .huff a la ruta '/api/decompress' y se le devolverá el archivo original \n * Visualice el listado de compresiones en la ruta '/api/compressions' \n * Los archivos devueltos los podrá descargar al terminar la petición";
            return mensaje;
        }

        private IHostingEnvironment _env;

        public CompressController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Route("{name}")]
        [HttpPost]
        public async Task<ActionResult> CompressFile([FromForm] IFormFile file, string name)
        {
            try
            {
                // Escribir archivo subido hacia el servidor para trabajar con ese
                var path = _env.ContentRootPath;
                path = Path.Combine(path, "Files");

                string pathInfo = Path.Combine(path, "Info");


                if (System.IO.File.Exists($"{pathInfo}/archivoCargado.txt"))
                {
                    System.IO.File.Delete($"{pathInfo}/archivoCargado.txt");
                }

                using var saver = new FileStream($"{pathInfo}/archivoCargado.txt", FileMode.OpenOrCreate);
                await file.CopyToAsync(saver);
                saver.Close();


                // Leer el archivo en el servidor para trabajar sobre él
                using var fileWritten = new FileStream($"{pathInfo}/archivoCargado.txt", FileMode.OpenOrCreate);
                using var reader = new BinaryReader(fileWritten);
                var buffer = new byte[2000000];
                while (fileWritten.Position < fileWritten.Length)
                {
                    buffer = reader.ReadBytes(2000000);
                }
                reader.Close();
                fileWritten.Close();

                var nodeString = ByteGenerator.ConvertToString(buffer);

                Huffman huff = new Huffman();
                string cadena_cifrada = huff.Compress(nodeString);


                // Escribir el archivo {name}.huff en el servidor

                using (var fs = new FileStream($"{path}/{name}.huff", FileMode.OpenOrCreate))
                {
                    fs.Write(ByteGenerator.ConvertToBytes(cadena_cifrada), 0, cadena_cifrada.Length);
                }


                // Obtener la info de compresión
                CompressionInfo cInfo = new CompressionInfo();
                cInfo.originalName = file.FileName;
                cInfo.compressedFilePath = Path.Combine(path, $"{name}.huff");
                cInfo.compressionRatio = Math.Round( Convert.ToDouble(cadena_cifrada.Length) / Convert.ToDouble(nodeString.Length),2 );
                cInfo.compressionFactor = Math.Round( 1 / cInfo.compressionRatio, 2 );
                cInfo.reductionPercentage = Math.Round( (1 - cInfo.compressionRatio) * 100, 2 );


                // Escribir en el archivo Historial.txt
                string pathHistorial = Path.Combine(pathInfo, "Historial.txt");
                string cadenaHistorial = System.IO.File.ReadAllText(pathHistorial);
                StreamWriter newfile = new StreamWriter(pathHistorial);

                if (cadenaHistorial.Length != 0)
                {
                    newfile.Write(cadenaHistorial);
                    newfile.Write("/");
                }
                newfile.Write(cInfo.originalName + ";" + cInfo.compressedFilePath + ";" + (cInfo.compressionRatio).ToString() + ";" + (cInfo.compressionFactor).ToString() + ";" + (cInfo.reductionPercentage).ToString());

                newfile.Close();


                //Archivo a mandar de regreso
                return PhysicalFile($"{path}/{name}.huff", "text/plain", $"{name}.huff");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

            

            
        }

    }
}