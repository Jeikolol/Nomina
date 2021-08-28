using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Nomina.Pages
{
    public class NominaModel : PageModel
    {
        public List<UserData> UserList { get; set; } = new List<UserData>();

        public void OnGet()
        {
            UserList.Add(new UserData
            {
                Nombres = "Juan",
                Apellidos = "Martinez",
                Cargo = "Gerente de Banco",
                SalarioMensual = 100000,
                DescuentoAfp = 0,
                DescuentoArs = 0,
                DescuentoIsr = 0,
                TotalDescuentos = 0,
                SalarioNeto = 0
            });

            UserList.Add(new UserData
            {
                Nombres = "Pedro",
                Apellidos = "Santana",
                Cargo = "Sub Gerente de Banco",
                SalarioMensual = 100000,
                DescuentoAfp = 0,
                DescuentoArs = 0,
                DescuentoIsr = 0,
                TotalDescuentos = 0,
                SalarioNeto = 0
            });

            UserList.Add(new UserData
            {
                Nombres = "Mario",
                Apellidos = "Perez",
                Cargo = "Cajero",
                SalarioMensual = 50000,
                DescuentoAfp = 0,
                DescuentoArs = 0,
                DescuentoIsr = 0,
                TotalDescuentos = 0,
                SalarioNeto = 0
            });

            UserList.Add(new UserData
            {
                Nombres = "Maria",
                Apellidos = "Matias",
                Cargo = "Servicio al Cliente",
                SalarioMensual = 60000,
                DescuentoAfp = 0,
                DescuentoArs = 0,
                DescuentoIsr = 0,
                TotalDescuentos = 0,
                SalarioNeto = 0
            });

            UserList.Add(new UserData
            {
                Nombres = "Joan",
                Apellidos = "Sanchez",
                Cargo = "Cajero",
                SalarioMensual = 50000,
                DescuentoAfp = 0,
                DescuentoArs = 0,
                DescuentoIsr = 0,
                TotalDescuentos = 0,
                SalarioNeto = 0
            });

            UserList.Add(new UserData
            {
                Nombres = "Luisa",
                Apellidos = "Trinidad",
                Cargo = "Servicio al Cliente",
                SalarioMensual = 60000,
                DescuentoAfp = 0,
                DescuentoArs = 0,
                DescuentoIsr = 0,
                TotalDescuentos = 0,
                SalarioNeto = 0
            });

            UserList.Add(new UserData
            {
                Nombres = "Ana",
                Apellidos = "Ventura",
                Cargo = "Cajera",
                SalarioMensual = 40000,
                DescuentoAfp = 0,
                DescuentoArs = 0,
                DescuentoIsr = 0,
                TotalDescuentos = 0,
                SalarioNeto = 0
            });

            foreach (var item in UserList)
            {
                item.DescuentoAfp = CalcularAfp(item.SalarioMensual);
                item.DescuentoArs = CalcularArs(item.SalarioMensual);
                item.DescuentoIsr = CalcularIsr(item.SalarioMensual);
                item.TotalDescuentos = CalcularDescuentos(item.DescuentoAfp, item.DescuentoArs, item.DescuentoIsr);
                item.SalarioNeto = CalcularNeto(item.DescuentoAfp, item.DescuentoArs, item.DescuentoIsr, item.SalarioMensual);
            }
        }

        private double CalcularAfp(double monto)
        {
            var total = monto * (2.87 / 100);

            if (total > 7738.67)
            {
                total = 7738.67;
            }

            return total;
        }

        private double CalcularArs(double monto)
        {
            double total = monto * (3.04 / 100);

            if (total > 4098.53)
            {
                total = 4098.53;
            }

            return total;
        }

        private double CalcularIsr(double monto)
        {
            double montoAnual = monto * 12;

            double LISR2 = 416220.01 / 12;    // Limite inferior de la escala 2.
            double LISR3 = 624329.01 / 12;    // Limite inferior de la escala 3.
            double LISR4 = 867123.01 / 12;    // Limite inferior de la escala 4.
            double PCISR2 = 15.0 / 12;       // Porciento aplicado a la escala 2.
            double PCISR3 = 20.0 / 12;        // Porciento aplicado a la escala 3.
            double PCISR4 = 25.0 / 12;        // Porciento aplicado a la escala 4.
            double incr3 = 31216.00 / 12;      // Valor fijo aplicado a la escala 3.
            double incr4 = 79776.00 / 12;      // Valor fijo aplicado a la escala 4.   

            double total = monto * (27 / 100);
            // determinar el valor de ISR segun la escala salarial

            //No hay decuento, segun la escala 1
            if ((montoAnual <= LISR2))
            {
                total += total;

            } // 15% anual, segun la escala 2
            else
            {
                double excedente;

                if ((montoAnual <= LISR3))
                {
                    // como es al excedente de LISR2, debemos hacer una resta con el sueldo
                    excedente = (monto - LISR2);

                    // determino el descuento segun escala 2
                    total = (excedente * PCISR2) / 100;

                } // 20% anual, , segun la escala 3
                else
                {
                    if ((montoAnual <= LISR4))
                    {
                        // como es al excedente de LISR3, debemos hacer una resta con el sueldo
                        excedente = (monto - LISR3);

                        // determino el descuento mas incr3, segun la escala 3
                        total = ((excedente * PCISR3 / 100) + (incr3));

                    } // 25% anual, , segun la escala 4
                    else
                    {
                        // como es al excedente de LISR4, debemos hacer una resta con el sueldo
                        excedente = (montoAnual - LISR4);

                        // determino el descuento mas incr4, segun la escala 4
                        total = ((excedente * PCISR4 / 100) + (incr4));
                    }
                }
            }

            return total / 12;
        }

        private double CalcularNeto(double afp, double ars, double isr, double monto)
        {
            var n1 = afp + ars;
            var n2 = n1 + isr;
            var total = monto - n2;

            return (double)total;
        }

        private double CalcularDescuentos(double afp, double ars, double isr)
        {
            var n1 = afp + ars;
            var total = n1 + isr;

            return total;
        }
    }

    public class UserData
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cargo { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public double SalarioMensual { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public double DescuentoAfp { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public double DescuentoArs { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public double DescuentoIsr { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public double TotalDescuentos { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public double SalarioNeto { get; set; }
    }
}
