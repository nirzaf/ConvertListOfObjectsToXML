// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

using Bogus;

Console.WriteLine("Convert List of HC Numbers to XML");

var hcNumbers = new Faker<HCNumberList>()
    .RuleFor(x => x.HCNumbers, f => f.Random.WordsArray(3).ToList()).Generate();

//Convert hcNumbers to XML 

var xml = new XElement("HC_NUMBERList", hcNumbers.HCNumbers!.Select(x => new XElement("HC_NUMBER", x))).ToString();
xml = Regex.Replace(xml, @"\s+", "");

var xml2 = XMLConverter.ConvertListOfQIDsToXML(hcNumbers);

Console.WriteLine("XML 1");
Console.WriteLine(xml);

Console.WriteLine("XML 2");
Console.WriteLine(xml2);

Console.ReadLine();


public class HCNumberList
{
    public List<string>? HCNumbers { get; set; }
}

public static class XMLConverter
{
    public static string ConvertListOfQIDsToXML(HCNumberList qIdList)
    {
        StringBuilder builder = new();

        //input xml to be used for SP_GET_MULTI_USER_CONFIRMED_APPTS_QID

        XmlWriterSettings xmlWriterSettings = new()
        {
            OmitXmlDeclaration = true
        };

        using (StringWriter stringWriter = new(builder))
        {
            using (XmlWriter writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("HC_NUMBERList");
                foreach (var item in qIdList.HCNumbers!)
                {
                    writer.WriteElementString("HC_NUMBER", item);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        return builder.ToString();
    }
}