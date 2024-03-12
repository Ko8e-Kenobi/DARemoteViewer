using DARemoteViewer.Domain;
using DARemoteViewer.Domain.Services;
using DARemoteViewer.Domain.Services.ConfigServices;
using DARemoteViewer.Domain.Models;
using System.Xml.Linq;
internal class Program
{
    public static void Main(string[] args)
    {
        Config defaultConfig = new Config()
        {
            Name = "Default",
            fileName = $"{Directory.GetCurrentDirectory()}\\DefaultConfig.xml",
            Description = "Default user config"
        };
        ICommandService<Config> createConfig = new CreateConfigService();
        createConfig.Execute(defaultConfig);
        Console.ReadLine();
    }
}