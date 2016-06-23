using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Piast_Code_App.AdamCode
{
    class Parsers
    {
        public string getRequest(string from, string to, string date, string time, string change)
        {
            string mainURL = "http://www.mzkopole.pl/rozklady/ajax.php?p=ajax_search_results&";
            string completeURL = mainURL + "&from=" + from + "&to=" + to + "&time=" + time + "&date=" + date + "&change=" + change + "&direct=0&direction=0&s=1";
            string completeData = null;

            WebRequest request = WebRequest.Create(completeURL);
            WebResponse response = request.GetResponse();

            Stream requestData = response.GetResponseStream();
            StreamReader sr = new StreamReader(requestData, Encoding.GetEncoding("iso-8859-2"));

            completeData = sr.ReadToEnd();

            sr.Close();
            response.Close();

            if (completeData != null)
            {
                return completeData;
            }

            return "1";
        }

        public List<string> getData(string from, string to, string date, string time, string change)
        {
            List<string> done = new List<string>();

            string request = getRequest(from, to, date, time, change);
            string tmp = "", first = "", tmp2 = "";

            int position = 0;

            int start = 0, stop = 0, len = 0, count = 0;
            bool end = false;

            while (request.IndexOf("return false", position) != -1)
            {

                start = request.IndexOf("return false", position) + 15;
                stop = request.IndexOf("<", start);
                len = stop - start;

                if (len > 0)
                {
                    tmp = request.Substring(start, len);
                    //   first = tmp;
                    tmp += ";";

                    start = stop + 13;
                    stop = request.IndexOf("</td>", start);
                    len = stop - start;


                    if (count == 0)
                    {
                        first = request.Substring(start, len);
                        tmp = tmp + first;
                        count++;
                    }
                    else
                    {
                        tmp2 = request.Substring(start, len);
                        tmp += tmp2;
                    }

                    if (first == tmp2)
                    {
                        end = true;
                    }

                    tmp += ";";

                    start = stop + 9;
                    stop = request.IndexOf("</td><td", start);
                    len = stop - start;

                    tmp += request.Substring(start, len);
                    tmp += ";";

                    start = stop;
                    start = request.IndexOf("Kierunek:", start) + 10;
                    stop = request.IndexOf("<", start);
                    len = stop - start;

                    tmp += request.Substring(start, len);
                    tmp += ";";

                    start = stop;
                    start = request.IndexOf("class=\"row", start) + 17;
                    stop = request.IndexOf("<", start);

                    len = stop - start;

                    tmp += request.Substring(start, len);

                    tmp += ";";

                    start = stop + 9;
                    stop = request.IndexOf("<", start);
                    len = stop - start;
                    tmp += request.Substring(start, len);




                    if (end)
                    {

                        tmp = "\n" + tmp; //znak miedzy polaczeniam
                        end = false;
                    }


                    done.Add(tmp);


                }
                position = stop;
                start = 0;
                stop = 0;
                len = 0;


                //done.Add("\n");
            }

            foreach (var item in done)
            {
                //  richTextBox1.Text += item;
                //richTextBox1.Text += item + "\n";

            }

            return done;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    getData(prepareString(textBox1.Text), prepareString(textBox2.Text), "13.05.2016", "19:20", "3");
        //}

        public string prepareString(string busstop)
        {
            StringBuilder sb = new StringBuilder(busstop);

            sb.Replace('ą', 'a')
              .Replace('ć', 'c')
              .Replace('ę', 'e')
              .Replace('ł', 'l')
              .Replace('ń', 'n')
              .Replace('ó', 'o')
              .Replace('ś', 's')
              .Replace('ż', 'z')
              .Replace('ź', 'z')
              .Replace('Ą', 'A')
              .Replace('Ć', 'C')
              .Replace('Ę', 'E')
              .Replace('Ł', 'L')
              .Replace('Ń', 'N')
              .Replace('Ó', 'O')
              .Replace('Ś', 'S')
              .Replace('Ż', 'Z')
              .Replace('Ź', 'Z');


            string tmp = sb.ToString();
            string[] data = tmp.Split(' ');

            tmp = null;

            for (int i = 0; i < data.Count(); i++)
            {
                tmp += data[i];
                if (i < data.Count() - 1)
                {
                    tmp += "%20";
                }
            }


            return tmp;
        }
    }
}
