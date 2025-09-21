using Craft.Utils;
using Newtonsoft.Json;

var dummy1 = new LineSegmentD(new PointD(7, 9), new PointD(13, 24));
var dummy2 = JsonConvert.SerializeObject(dummy1);

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
