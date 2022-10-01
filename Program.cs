using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Program
{


    class Program
    {
        static void Main(String[] args)
        {
            string source = File.ReadAllText("C:\\Users\\Patryk\\source\\repos\\proba\\proba\\Praca.csv");

            var row_separator = new[] { "\r\n" };
            string[] rows = source.Split(row_separator, StringSplitOptions.RemoveEmptyEntries);


            char[] month = new char[2] {rows[1][32], rows[1][33]};

            char[] year = new char[4] {rows[1][35], rows[1][36], rows[1][37], rows[1][38]};


            char item_separator = ';';

            string[] keys = new string[rows.Length - 3];

            char[,] status = new char[rows.Length - 3, 31];

            for(int i = 3; i < rows.Length; i++)
            {
                keys[i - 3] = "";

                int position0 = 0;

                do
                {
                    keys[i - 3] += rows[i][position0];

                    position0++;

                } while (rows[i][position0] != item_separator);

                int position1 = position0 + 1;

                for (int j = 0; j < 31; j++)
                {
                    if(rows[i][position1] != item_separator)
                    {
                        status[i - 3, j] = rows[i][position1];

                        position1 += 2;
                    }
                }
            }

            using (var writer = new StreamWriter("C:\\Users\\Patryk\\source\\repos\\proba\\proba\\test.xml"))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"Unicode\" ?>");
                writer.WriteLine("<Root xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");

                for (int i = 0; i < keys.Length; i++)
                {
                    writer.WriteLine("\t<DniPlanu>");

                    for(int j = 1; j < 32; j++)
                    {
                        if (status[i, j - 1] != ' ')
                        {
                            writer.WriteLine("\t\t<DzienPlanu>");

                            writer.WriteLine("\t\t\t<Pracownik>" + keys[i] + "</Pracownik>");

                            if (j >= 10) writer.WriteLine("\t\t\t<Data>" + year[0] + year[1] + year[2] + year[3] + "/" + month[0] + month[1] + "/" + j + "</Data>");

                            else writer.WriteLine("\t\t\t<Data>" + year[0] + year[1] + year[2] + year[3] + "/" + month[0] + month[1] + "/" + 0 + j + "</Data>");

                            if (status[i, j - 1] == 'X') writer.WriteLine("\t\t\t<Definicja>Wolny</Definicja>");

                            else
                            {
                                writer.WriteLine("\t\t\t<Definicja>Praca</Definicja>");

                                if(status[i, j - 1] == '1') writer.WriteLine("\t\t\t<OdGodziny>06:00</OdGodziny>");

                                else if(status[i, j - 1] == 'X') writer.WriteLine("\t\t\t<OdGodziny>14:00</OdGodziny>");

                                else writer.WriteLine("\t\t\t<OdGodziny>22:00</OdGodziny>");

                                writer.WriteLine("\t\t\t<Czas>08:00</Czas>");
                            }

                            writer.WriteLine("\t\t</DzienPlanu>");
                        }
                        
                    }

                    writer.WriteLine("\t</DniPlanu>");
                }

                writer.WriteLine("</Root>");
            }
        }
    }

}