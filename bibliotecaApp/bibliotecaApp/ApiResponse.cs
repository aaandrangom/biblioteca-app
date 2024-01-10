using System;
using System.Collections.Generic;
using System.Text;

namespace bibliotecaApp
{
    public class ApiResponse
    {
        public string Mensaje { get; set; }
        public List<Libro> Libros { get; set; }
        public Libro Libro { get; set; }
    }
}
